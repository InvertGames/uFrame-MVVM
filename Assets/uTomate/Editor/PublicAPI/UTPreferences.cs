//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;


/// <summary>
/// uTomate preferences.
/// </summary>
public class UTPreferences
{
	/// <summary>
	/// Whether or not plans that have been marked as  "hide in main window" should be displayed in the main
	/// window.
	/// </summary>
	public static bool ShowHiddenPlansInRunner {
		get {
			return EditorPrefs.GetBool("uTomate.showHiddenPlansInRunner");
		}
		set {
			EditorPrefs.SetBool("uTomate.showHiddenPlansInRunner", value);
		}
	}
	
	/// <summary>
	/// Whether or not debug mode is enabled. When debug mode is enabled, actionss log more diagnostic information.
	/// </summary>
	public static bool DebugMode {
		get {
			return EditorPrefs.GetBool("uTomate.debugMode");
		}
		set {
			EditorPrefs.SetBool("uTomate.debugMode", value);
		}
	}
	
	/// <summary>
	/// Whether or not the console should be cleared before a plan is executed.
	/// </summary>
	public static bool ClearConsoleBeforeStart {
		get {
			return EditorPrefs.GetBool("uTomate.clearConsoleBeforeStart");
		}
		set {
			EditorPrefs.SetBool("uTomate.clearConsoleBeforeStart", value);
		}
	}
	
	/// <summary>
	/// Whether or not the main window should show the plan list after a plan is run. If set to true, the main window
	/// will show the plan list after a plan is run, otherwise it will stay in the runner view.
	/// </summary>
	public static bool ReturnToPlanListAfterPlanIsRun {
		get {
			return EditorPrefs.GetBool("uTomate.returnToPlanListAfterPlanIsRun");
		}
		set {
			EditorPrefs.SetBool("uTomate.returnToPlanListAfterPlanIsRun", value);
		}
	}
	
	/// <summary>
	/// The name of the plan that has been executed last. 
	/// </summary>
	public static string LastRunPlan {
		get {
			return EditorPrefs.GetString("uTomate.lastRunPlan", "");
		}
		set {
			EditorPrefs.SetString("uTomate.lastRunPlan", value);
		}
	}

}

