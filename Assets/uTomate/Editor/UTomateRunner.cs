//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// The runner which executes an automation plan, provides timing information and progress.
/// </summary>
public class UTomateRunner
{
	private static UTomateRunner runner;
	private UTAutomationPlan plan;
	private UTContext context;
	private IEnumerator enumerator;
	private DateTime startTime;
	private TimeSpan expectedTime;
	private bool planWasRunBefore;
	private bool planLookupDone = false;
	private UTAutomationPlan lastPlan;
	
	public delegate void RunnerStarted ();

	public event RunnerStarted OnRunnerStarted;
	public delegate void RunnerFinished (bool canceled,bool failed);

	public event RunnerFinished OnRunnerFinished;
	private bool reloadOfAssembliesLocked;
	private bool playerSettingsRunInBackgroundValue;
	
	private UTomateRunner ()
	{
	}
	
	public static UTomateRunner Instance {
		get {
			if (runner == null) {
				runner = new UTomateRunner ();
			}
			return runner;
		}
	}
	
	/// <summary>
	/// Gets the entry that is currently being executed.
	/// </summary>
	public UTAutomationPlanEntry CurrentEntry {
		get {
			return context != null ? context.CurrentEntry : null;
		}
	}
	
	public UTAutomationPlan CurrentPlan {
		get {
			return plan;
		}
	}
	
	public UTAutomationPlan LastPlan {
		get {
			if (!planLookupDone) {
				string plan = UTPreferences.LastRunPlan;
				lastPlan = UTomate.UTAutomationPlanByName (plan);
				planLookupDone = true;
			}
			return lastPlan;
		}
	}
	
	/// <summary>
	/// Requests to run a certain plan. Only one plan can run at a time.
	/// </summary>
	/// <param name='plan'>
	/// The plan to run.
	/// </param>
	/// <param name='context'>
	/// Context in which the plan should run.
	/// </param>
	public void RequestRun (UTAutomationPlan plan, UTContext context)
	{
#if UTOMATE_DEMO
		// when developing utomate demo locally we want to allow our own plans, so we set another flag which
		// removes this restriction for us.
#if !UTOMATE_DEVELOPMENT_MODE 
		if (UTomate.CheckPlanCountExceeded()) {
			return;
		}
#endif 
#endif

		if (IsRunning) {
			throw new InvalidOperationException ("The runner is currently busy. Use IsRunning to check if the runner is busy, before invoking this.");
		}
		this.plan = plan;
		this.context = context;
		float expectedTimeInSeconds = 0;
		this.planWasRunBefore = UTStatistics.GetExpectedRuntime (plan, out expectedTimeInSeconds);
		this.expectedTime = TimeSpan.FromSeconds (expectedTimeInSeconds);
		UTPreferences.LastRunPlan = plan.name;
		planLookupDone = false;
		
		if (OnRunnerStarted != null) {
			OnRunnerStarted ();
		}
		// save all changes to assets before run.
		AssetDatabase.SaveAssets();
		EditorApplication.SaveAssets();
		
		// keep stuff running in background
		// this will overwrite playersettings implicitely, therefore we store the previous value here..
		playerSettingsRunInBackgroundValue = PlayerSettings.runInBackground;
		Application.runInBackground = true;
		// and set it back here... so we don't fuck up the settings that were set before running utomate.
		PlayerSettings.runInBackground = playerSettingsRunInBackgroundValue;
		this.reloadOfAssembliesLocked = false;
	}
	
	public bool IsRunning {
		get {
			return this.plan != null || this.context != null;
		}
	}
	
	public void DrawGUI ()
	{
		EditorGUILayout.BeginVertical ();
		if (IsRunning) {
			EditorGUILayout.LabelField ("Running plan", plan.name);
			if (context.CurrentEntry != null) {
				EditorGUILayout.LabelField ("Entry", context.CurrentEntry.Label);
			} 

			var difference = (DateTime.Now - startTime);
			string remainingTime;
			float progress = 0f;
			if (!context.CancelRequested) {
				if (planWasRunBefore) {
					if (difference.CompareTo (expectedTime) > 0) {
						remainingTime = "overtime: " + FormatTime (difference.Subtract (expectedTime), true);
						progress = 1f;
					} else {
						remainingTime = "remaining: " + FormatTime (expectedTime.Subtract (difference), true);
						if (expectedTime.TotalSeconds > 0) {
							progress = (float)(difference.TotalSeconds / expectedTime.TotalSeconds);
						}
					}
				} else {
					remainingTime = "running for: " + FormatTime (difference, true);
				}
			} else {
				remainingTime = "Cancelling current run...";
			}

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			var rect = GUILayoutUtility.GetRect (new GUIContent (""), UTEditorResources.ProgressBarStyle, GUILayout.ExpandWidth (true));
			EditorGUI.ProgressBar (rect, progress, remainingTime);
			
			GUI.enabled = context.CancelRequested == false;
			if (GUILayout.Button (UTEditorResources.CancelIcon, UTEditorResources.CancelButtonStyle)) {
				context.CancelRequested = true;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal ();
		} else {
			GUILayout.Label ("Currently no plan is running.");
		}
		EditorGUILayout.EndVertical ();
	}
	
	public void ContinueRunning ()
	{
		if (plan == null || context == null) {
			return;
		}
		
		// don't reload assemblies during build, this will break some builds
		if (EditorApplication.isCompiling && !reloadOfAssembliesLocked) {
			EditorApplication.LockReloadAssemblies ();
			reloadOfAssembliesLocked = true;
			Debug.Log("Detected project recompile while executing automation. Locking reloading of assemblies until uTomate is finished.");
		}
		
		try {
			if (enumerator == null) {
				// clear selection to avoid exceptions for missing resources due to reload
				Selection.objects = new UnityEngine.Object[0];
				
				startTime = DateTime.Now;
				Debug.Log ("uTomate - Running plan: " + plan.name);
				enumerator = plan.Execute (context);
			} else {
				if (!enumerator.MoveNext ()) {
					CleanUp ();
				}
			}
		} catch (UTFailBuildException e) {
			context.Failed = true;
			e.LogToConsole ();
			Debug.LogError ("Fail: Execution of plan " + plan.name + " failed.");
			CleanUp ();	
		} catch (Exception e) {
			context.Failed = true;
			Debug.LogError ("Error: unexpected exception has been thrown. " + e.Message);
			CleanUp ();
		}

	}
	
	private static string FormatTime (TimeSpan duration, bool shortLayout)
	{
		if (shortLayout) {
			return duration.Minutes.ToString ("00") + ":" + duration.Seconds.ToString ("00");
		} else {
			return (duration.Minutes > 0 ? duration.Minutes + "minutes " : "") + duration.Seconds + " seconds";
		}
	}
	
	private void CleanUp ()
	{
		UTils.ClearAsyncProgressBar ();
		var cancelled = context.CancelRequested;
		var failed = context.Failed;
		context = null;
		enumerator = null;
		
		var endTime = DateTime.Now;
		var duration = endTime - startTime;
		if (!cancelled && !failed) { // recording cancelled runs will greatly diminish the accuracy of the statistics, so don't record them
			UTStatistics.RecordRuntime (plan, (float)duration.TotalSeconds);
		}
		plan = null;
		Debug.Log ("Automation finished in " + FormatTime (duration, false) + ".");
		if (cancelled) {
			Debug.LogWarning ("Run was canceled by user.");
		}
		if (failed) {
			Debug.LogError ("Run failed with error.");
		}
		
		if (OnRunnerFinished != null) {
			OnRunnerFinished (cancelled, failed);
		}

		// now check if the player settings were modified by an action, in this case we don't reset them
		var newBackgroundSetting = PlayerSettings.runInBackground;
		Application.runInBackground = false;

		// and reset it back if it wasn't so we don't fuck up what was set before..
		if (newBackgroundSetting == playerSettingsRunInBackgroundValue) {
			// not modified by an action, in this case reset it
			PlayerSettings.runInBackground = playerSettingsRunInBackgroundValue;
		}

		if (reloadOfAssembliesLocked) {
			Debug.Log("Releasing assembly reload lock now.");
			EditorApplication.UnlockReloadAssemblies ();
		}
		AssetDatabase.Refresh (); // make sure updates are shown in the editor.
	}
}

