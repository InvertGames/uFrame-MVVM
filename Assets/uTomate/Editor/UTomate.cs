//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

/// <summary>
/// Class providing the main interface into uTomate.
/// </summary> 
public class UTomate
{
	
	private const string ProjectPropertiesPath = "Assets/uTomateProjectProperties.asset";
	private const string ProjectStatisticsPath = "Assets/uTomateProjectStatistics.asset";
	private static UTProjectProperties projectProperties;

#if UTOMATE_DEMO
	public const int MaxPlansForDemo = 2;
	public const int MaxEntriesForDemo = 5;
	public const string AssetStoreUrl = "https://www.assetstore.unity3d.com/#/content/7703";
#endif 

	/// <summary>
	/// Run the specified plan.
	/// </summary>
	/// <param name='plan'>
	/// Plan to run.
	/// </param>
	public static void Run(UTAutomationPlan plan) {
		Run (plan, null);
	}
	
	
	/// <summary>
	/// Runs the specified plan.
	/// </summary>
	public static void Run (UTAutomationPlan plan, Dictionary<string,string> additionalProperties)
	{
		if (UTPreferences.ClearConsoleBeforeStart) {
			UTils.ClearConsole();
		}
		var context = new UTContext();
		if (additionalProperties != null) {
			foreach(var entry in additionalProperties) {
				string name = entry.Key;
				string val = entry.Value;
				if (UTPreferences.DebugMode) {
					Debug.Log("Setting additional property: " + name + " = " + val);
				}
				context[name] = val;
			}
		}
		
		UTMainWindow.Init();
		var runner = UTomateRunner.Instance;
		runner.RequestRun(plan, context);
	}
	
	
	/// <summary>
	/// Gets the project properties for the current project. If they don't exist they will be created.
	/// </summary>
	public static UTProjectProperties ProjectProperties {
		
		get {
			if (projectProperties == null) {
				try {
					projectProperties = (UTProjectProperties)AssetDatabase.LoadAssetAtPath (ProjectPropertiesPath, typeof(UTProjectProperties));
				} catch (UnityException) {
					// ok asset might not exist yet.
				}
				if (projectProperties == null) {
					Debug.Log ("Creating uTomate Project Properties.");
					// create it
					projectProperties = ScriptableObject.CreateInstance<UTProjectProperties> ();
					AssetDatabase.CreateAsset (projectProperties, ProjectPropertiesPath);
					EditorUtility.SetDirty(projectProperties);
				}
			}
			return projectProperties;
		}
	}
	
	public static List<UTAutomationPlan> AllUTAutomationPlans ()
	{
		return UTils.AllVisibleAssetsOfType<UTAutomationPlan> ();
	}
	
	public static UTAutomationPlan UTAutomationPlanByName(string name) {
		var allPlans = AllUTAutomationPlans();
		foreach(var plan in allPlans) {
			if (plan.name == name) {
				return plan;
			}
		}
		return null;
	}
	
	[Obsolete("With no replacement. Will be removed in a future version.")]
	public static List<UTAction> AllUTActions ()
	{
		return UTils.AllVisibleAssetsOfType<UTAction> ();
	}

#if UTOMATE_DEMO
	public static bool CheckPlanCountExceeded() {
		var planCount = AllUTAutomationPlans().Count;
		if (planCount > UTomate.MaxPlansForDemo) {
			if (!EditorUtility.DisplayDialog("Locked in demo version", 
			                 "The demo version is limited to " + UTomate.MaxPlansForDemo + " automation plans however you have currently " + planCount +
			                 " automation plans in the project. " +
			                 "Please remove additional automation plans from the project and then try again.", 
			                            "Ok", "Buy uTomate now!")) {
				Application.OpenURL(UTomate.AssetStoreUrl);
			}
			return true;
		}
		return false;
	}

	[MenuItem("Window/uTomate/Buy now!", false, 5000)]
	public static void OpenInAssetStore() {
		Application.OpenURL(UTomate.AssetStoreUrl);
	}
#endif

}



;
