//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Class for collecting statistics.
/// </summary>
public class UTStatistics
{
	
	const string PlanPrefix = "utStatistics.plan.";
	
	/// <summary>
	/// Records the runtime of an automation plan which allows making an estimation on the run time when the plan
	/// runs next time.
	/// </summary>
	/// <param name='plan'>
	/// The plan
	/// </param>
	/// <param name='seconds'>
	/// The time in seconds that the plan needed to run.
	/// </param>
	public static void RecordRuntime (UTAutomationPlan plan, float seconds)
	{
		var i = 0;
		var found = false;
		do {
			var entry = EditorPrefs.GetString (PlanPrefix + i, "");
			if (string.IsNullOrEmpty (entry)) {
				break;
			}
			if (entry == plan.Guid) {
				found = true;
				break;
			}
			i++;
		} while (true);
		
		
		var currentTime = EditorPrefs.GetFloat (PlanPrefix + i + ".time", 0f);
		if (UTPreferences.DebugMode) {
			Debug.Log ("Recording " + seconds.ToString ("0.00") + " for plan " + plan.Guid);
		}
		var newTime = seconds;
		if (found) {
			newTime = (currentTime + seconds) / 2f;
		}
		EditorPrefs.SetString (PlanPrefix + i, plan.Guid);
		EditorPrefs.SetFloat (PlanPrefix + i + ".time", newTime);
		EditorPrefs.SetString (PlanPrefix + i + ".lastModified", ToString (DateTime.Now));
		EditorPrefs.SetString (PlanPrefix + i + ".project", Application.dataPath);
		
	}
	
	/// <summary>
	/// Returns an estimation of the plan's run time from the history of previous runtimes.
	/// </summary>
	/// <returns>
	/// The expected runtime. If the plan never run before 0 is returned.
	/// </returns>
	/// <param name='plan'>
	/// The plan.
	/// </param>
	public static bool GetExpectedRuntime (UTAutomationPlan plan, out float time)
	{
		var i = 0;
		var found = false;
		do {
			var entry = EditorPrefs.GetString (PlanPrefix + i, "");
			if (string.IsNullOrEmpty (entry)) {
				break;
			}
			if (entry == plan.Guid) {
				found = true;
				break;
			}
			i++;
		} while (true);
		if (found) {
			time = EditorPrefs.GetFloat (PlanPrefix + i + ".time", 0f);
		} else {
			time = 0;
		}
		return found;
	}
	
	/// <summary>
	/// Cleans up the entries in this statistics. This accounts for plans that have been deleted.
	/// </summary>
	public static void CleanUp ()
	{
		var allPlans = UTils.AllVisibleAssetsOfType<UTAutomationPlan> ();
		
		var knownGuids = new HashSet<string> ();
		knownGuids.UnionWith (allPlans.ConvertAll (plan => plan.Guid));
		var knownEntries = new List<StatEntry> ();
		var i = 0;
		StatEntry statEntry = null;
		do {
			statEntry = new StatEntry ();
			statEntry.guid = EditorPrefs.GetString (PlanPrefix + i, "");
			if (!string.IsNullOrEmpty (statEntry.guid)) {
				statEntry.project = EditorPrefs.GetString (PlanPrefix + i + ".project", "");
				statEntry.lastModified = FromString (EditorPrefs.GetString (PlanPrefix + i + ".lastModified"));
				statEntry.time = EditorPrefs.GetFloat (PlanPrefix + i + ".time", 0f);
			
				if (knownGuids.Contains (statEntry.guid)) {
					knownEntries.Add (statEntry);
				} else {
					// not known, check if it's from this project
					if (statEntry.project != Application.dataPath) {
						// different project, candidate for keeping it
						var age = DateTime.Now - statEntry.lastModified;
						if (age.TotalDays < 90) {
							// age is recent, keep it.
							knownEntries.Add (statEntry);
						}
					}
					// in all other cases, kill it.
				}
			
			
				EditorPrefs.DeleteKey (PlanPrefix + i);
				EditorPrefs.DeleteKey (PlanPrefix + i + ".time");
				EditorPrefs.DeleteKey (PlanPrefix + i + ".project");
				EditorPrefs.DeleteKey (PlanPrefix + i + ".lastModified");
			}
			i++;	
		} while (!string.IsNullOrEmpty(statEntry.guid));
		
		i = 0;
		foreach (var entry in knownEntries) {
			EditorPrefs.SetString (PlanPrefix + i, entry.guid);
			EditorPrefs.SetFloat (PlanPrefix + i + ".time", entry.time);
			EditorPrefs.SetString (PlanPrefix + i + ".project", entry.project);
			EditorPrefs.SetString (PlanPrefix + i + ".lastModified", ToString (entry.lastModified));
			i++;
		}
	}
	/// <summary>
	/// Checks if statistics for at least one plan exist.
	/// </summary>
	/// <returns>
	/// <c>true</c> if statistics exist; otherwise, <c>false</c>.
	/// </returns>
	public static bool HasStatistics ()
	{
		return EditorPrefs.HasKey ("utStatistics.plan.0");
	}
	
	
	/// <summary>
	/// Clears all statistics from the editor prefs.
	/// </summary>
	public static void Clear ()
	{
		var i = 0;
		string entry;
		do {
			entry = EditorPrefs.GetString (PlanPrefix + i, "");
			EditorPrefs.DeleteKey (PlanPrefix + i);
			EditorPrefs.DeleteKey (PlanPrefix + i + ".time");
			EditorPrefs.DeleteKey (PlanPrefix + i + ".project");
			EditorPrefs.DeleteKey (PlanPrefix + i + ".lastModified");
			
			i++;	
		} while (!string.IsNullOrEmpty(entry));
	}
	
	/// <summary>
	/// Converts a datetime to a string.
	/// </summary>
	public static string ToString (DateTime dateTime)
	{
		return dateTime.ToString ("MM/dd/yyyy hh:mm:ss.fff");
	}
	
	/// <summary>
	/// Converts a string back to a datetime.
	/// </summary>
	public static DateTime FromString (string input)
	{
		if (input != null) {
			try {
				return DateTime.ParseExact (input, "MM/dd/yyyy hh:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
			} catch (Exception) {
				return DateTime.Now;	
			}
		}
		return DateTime.Now;
	}
	
	/// <summary>
	/// Stat entry helper class used when clearing statistics.
	/// </summary>
	private class StatEntry
	{
		public string guid;
		public string project;
		public float time;
		public DateTime lastModified;
	}
}

