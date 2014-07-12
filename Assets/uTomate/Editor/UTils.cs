//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UObject = UnityEngine.Object;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditorInternal;

/// <summary>
/// UTility class (pun intended) for various stuff that doesn't fit in somehwere else.
/// </summary> 
public class UTils
{
	/// <summary>
	/// Creates an asset of the given type and writes it to a new file at the currently active selection point.
	/// </summary>
	/// <returns>
	/// The generated asset.
	/// </returns>
	public static T CreateAssetOfType<T> (string preferredName) where T:ScriptableObject
	{
		var name = string.IsNullOrEmpty (preferredName) ? typeof(T).Name : preferredName;
		
		var path = "Assets";
		foreach (UObject obj in Selection.GetFiltered(typeof(UObject), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);

            }
            break;
        }		
		
		
		path = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name + ".asset");
		T item = ScriptableObject.CreateInstance<T> ();
		AssetDatabase.CreateAsset (item, path);
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = item;
		return item;
	
	}
	
	/// <summary>
	/// Adds an asset to the asset at the given path.
	/// </summary>
	/// <returns>
	/// The added asset.
	/// </returns>
	/// <param name='path'>
	/// Path.
	/// </param>
	/// <param name='hide'>
	/// Hide the asset? 
	/// </param>
	/// <typeparam name='T'>
	/// The type of asset to be added.
	/// </typeparam>
	public static T AddAssetOfType<T> (string path, bool hide) where T:ScriptableObject
	{
		T item = ScriptableObject.CreateInstance<T> ();
		if (hide) {
			item.hideFlags = HideFlags.HideInHierarchy;
		}
		AssetDatabase.AddObjectToAsset (item, path);
		EditorUtility.SetDirty (item);
		return item;
		
	}
	
	/// <summary>
	/// Clears the unused entries in the automation plan asset located at the given path. This is necessary
	/// because we support undo/redo with the visual editor and therefore don't delete automation plan entries
	/// when they are removed from the graphical view. The unused entries are cleared when the plan is saved.
	/// </summary>
	/// <param name='path'>
	/// Path.
	/// </param>
	/// <seealso cref="UTSaveInterceptor"/>
	public static void ClearUnusedEntriesIn (string path)
	{
		var asset = AssetDatabase.LoadMainAssetAtPath (path);
		if (asset is UTAutomationPlan) {
			var plan = (UTAutomationPlan)asset;
			UTGraph graph = null;
			var allAssets = AssetDatabase.LoadAllAssetsAtPath (path);
			var entries = new List<UTAutomationPlanEntry> ();
			foreach (var anAsset in allAssets) {
				if (anAsset is UTGraph) {
					graph = (UTGraph)anAsset;
				}
				if (anAsset is UTAutomationPlanEntry) {
					entries.Add ((UTAutomationPlanEntry)anAsset);
				}
			}
			
			var deps = EditorUtility.CollectDependencies (new UObject[] { plan, graph});
			foreach (var dep in deps) {
				if (dep is UTAutomationPlanEntry) {
					entries.Remove ((UTAutomationPlanEntry)dep);
				}
			}
			
			if (UTPreferences.DebugMode && entries.Count > 0) {
				Debug.Log ("Clearing " + entries.Count + " leaked entries from " + plan.name + ". This message is harmless.");
			}
			foreach (var entry in entries) {
				ScriptableObject.DestroyImmediate (entry, true);
			}
			
			UTStatistics.CleanUp ();
		}
	}
	
	private static object filteredHierarchy;
	
	private static object CreateFilteredHierarchy() {
		if (filteredHierarchy == null) {
			filteredHierarchy = UTInternalCall.CreateInstance("UnityEditor.FilteredHierarchy", HierarchyType.Assets);
		}
		UTInternalCall.Invoke(filteredHierarchy, "ResultsChanged");
		return filteredHierarchy;	
	}
	
	/// <summary>
	/// Finds all visible assets of the given type.
	/// </summary>
	public static List<T> AllVisibleAssetsOfType<T> () where T:ScriptableObject
	{

		List<T> result = new List<T> ();
		
		object filteredHierarchy = CreateFilteredHierarchy();
		
#if UNITY_3_5
		UTInternalCall.Set(filteredHierarchy, "filter", typeof(T).Name);
		UTInternalCall.Set(filteredHierarchy, "searchMode", UTInternalCall.EnumValue("UnityEditor.FilteredHierarchy+SearchMode", "Type"));
#else		
		var searchFilter = UTInternalCall.InvokeStatic("UnityEditor.SearchableEditorWindow", "CreateFilter", typeof(T).Name, UTInternalCall.EnumValue("UnityEditor.SearchableEditorWindow+SearchMode", "Type"));
		UTInternalCall.Set (filteredHierarchy, "searchFilter", searchFilter);
#endif
		var hierarchyProperty = UTInternalCall.InvokeStatic("UnityEditor.FilteredHierarchyProperty", "CreateHierarchyPropertyForFilter", filteredHierarchy);
		
		var emptyIntArray = new int[0];
		while((bool)UTInternalCall.Invoke(hierarchyProperty, "Next", emptyIntArray)) {
			var instanceId = (int) UTInternalCall.Get(hierarchyProperty, "instanceID");
			var path = AssetDatabase.GetAssetPath(instanceId);
			T t = AssetDatabase.LoadAssetAtPath (path, typeof(T)) as T;
			if (t != null) {
				result.Add (t);
			}	
		}
		
		return result;
	}
	
	/// <summary>
	/// Checks if this script is running inside Unity Pro.
	/// </summary>
	/// <value>
	/// <c>true</c> if this script runs inside Unity Pro otherwise, <c>false</c>.
	/// </value>
	public static bool IsUnityPro {
		get {
			return InternalEditorUtility.HasPro ();			
		}
	}
	/// <summary>
	/// Checks if the build pipeline supports the given build target.
	/// </summary>
	public static bool IsBuildTargetSupported(BuildTarget target) {
		return (bool) UTInternalCall.InvokeStatic("UnityEditor.BuildPipeline", "IsBuildTargetSupported", target);
	}
	
	/// <summary>
	/// Checks if the user has the advanced license (PRO-license) for the given build target.
	/// </summary>
	public static bool HasAdvancedLicenseOn(BuildTarget target) {
		return InternalEditorUtility.HasAdvancedLicenseOnBuildTarget(target);
	}

	/// <summary>
	/// Computes the SHA-1 hash of the given bytes.
	/// </summary>
	/// <returns>
	/// The hash.
	/// </returns>
	/// <param name='bytes'>
	/// Bytes.
	/// </param>
	public static string ComputeHash (byte[] bytes)
	{
		using (SHA1Managed sha1 = new SHA1Managed()) {
			byte[] hash = sha1.ComputeHash (bytes);
			StringBuilder formatted = new StringBuilder (2 * hash.Length);
			foreach (byte b in hash) {
				formatted.AppendFormat ("{0:X2}", b);
			}
			return formatted.ToString().ToLower();
		}
	}
	
	/// <summary>
	/// Builds an URL from the given parts and removes duplicate slashes.
	/// </summary>
	/// <returns>
	/// The URL. 
	/// </returns>
	/// <param name='parts'>
	/// Parts.
	/// </param>
	public static string BuildUrl (params string[] parts)
	{
		var result = string.Join ("/", parts);
		// replace duplicate slashes
		result = Regex.Replace (result, "(?<!:)//+", "/");
		return result;
	}
	
	/// <summary>
	/// Clears Unity's console.
	/// </summary>
	public static void ClearConsole ()
	{
		UTInternalCall.InvokeStatic ("UnityEditorInternal.LogEntries", "Clear");
	}
	
	/// <summary>
	/// Shows an async progress bar in the lower right corner of Unity's window (like the one shown when
	/// rendering lightmaps).
	/// </summary>
	/// <param name='text'>
	/// Text to show.
	/// </param>
	/// <param name='progress'>
	/// Progress to show (anything between 0f and 1f)
	/// </param>
	public static void ShowAsyncProgressBar (string text, float progress)
	{
		UTInternalCall.InvokeStatic ("UnityEditor.AsyncProgressBar", "Display", text, progress);
	}
	
	/// <summary>
	/// Hides the async progress bar.
	/// </summary>
	public static void ClearAsyncProgressBar ()
	{
		UTInternalCall.InvokeStatic ("UnityEditor.AsyncProgressBar", "Clear");
	}
	
	/// <summary>
	/// Gets the editor executable.
	/// </summary>
	/// <returns>
	/// The editor executable.
	/// </returns>
	public static string GetEditorExecutable ()
	{
		if (Application.platform == RuntimePlatform.OSXEditor) {
			return EditorApplication.applicationPath + "/Contents/MacOS/Unity";
		} else {
			return EditorApplication.applicationPath;
		}
	}

	/// <summary>
	/// Completes the editor executable path depending on the current operating system.
	/// </summary>
	public static string CompleteEditorExecutable(string executablePath) {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			if (executablePath.EndsWith(".app")) {
				return executablePath+"/Contents/MacOS/Unity";
			}
		}
		return executablePath;
	}


}

