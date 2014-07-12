//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

/// <summary>
/// This class represents an automation plan. An automation plan contains a number of automation plan
/// entries.
/// </summary>
/// <seealso cref="UTAutomationPlanEntry"/>
[UTDoc(title="Plan")]
public class UTAutomationPlan : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	private string automationPlanId;
	
	/// <summary>
	/// The first entry of this plan.
	/// </summary>
	[HideInInspector]
	public UTAutomationPlanEntry firstEntry;
	
	[UTDoc(description="Should this plan be hidden in the execution window.")]
	public bool hideInExecutionWindow;
		
	/// <summary>
	/// The GUID of this plan. This is used as a key to record running time of plans, since plan names may change.
	/// </summary>
	public string Guid {
		get {
			if (string.IsNullOrEmpty(automationPlanId)) {
				automationPlanId = System.Guid.NewGuid().ToString();
				if (UTPreferences.DebugMode) {
					Debug.Log("Assigning guid: " + automationPlanId);
				}
				EditorUtility.SetDirty(this);
			}
			return automationPlanId;
		}
	}
	
	
	/// <summary>
	/// Creates a new automation plan from the editor's context menu.
	/// </summary>
	[MenuItem("Assets/Create/uTomate/Automation Plan",false, 0)]
	public static void Create ()
	{
#if UTOMATE_DEMO
		if (UTomate.CheckPlanCountExceeded()) {
			return;
		}
#endif
		UTils.CreateAssetOfType<UTAutomationPlan> ("Automation Plan");
	}
	
	/// <summary>
	/// Allows to run this plan quickly from the inspector GUI.
	/// </summary>
	[ContextMenu("Execute this plan")]
	public void Execute ()
	{
		UTomate.Run (this);
	}
	
	/// <summary>
	/// Executes this plan in the given context. Use <see cref="UTomate.Run"/> for executing a plan. 
	/// </summary>
	/// <param name='context'>
	/// Context.
	/// </param> 
	public IEnumerator Execute (UTContext context)
	{
#if UTOMATE_DEMO
		// we put the check here, because then it catches super-sized sub-plans as well.
		UTNodeEditorModel model = new UTNodeEditorModel();
		model.LoadPlan(this);
		if (model.ExceedsDemoEntryLimit) {
			if (!EditorUtility.DisplayDialog("Locked in demo version", 
			    "The demo version is limited to " + UTomate.MaxEntriesForDemo + 
			                 " entries per automation plan. Please remove additional entries from " + name + " and try again.", 
			                                 "Ok", "Buy uTomate now!")) {
				Application.OpenURL(UTomate.AssetStoreUrl);
			}
			return EmptyEnumerator();
		}
#endif
		if (firstEntry != null) {
			return ExecutePath(firstEntry, context);
		}
		return EmptyEnumerator();
	}
	
	/// <summary>
	/// Helper function providing an empty enumerator.
	/// </summary>
	public static IEnumerator EmptyEnumerator() {
		yield break;
	}
	
	
	/// <summary>
	/// Executes the given entry and it's followers (hence 'ExecutePath'). Automatically breaks execution if
	/// the context's CancelRequested flag is set. All UTAutomationPlanEntries should use this function if they
	/// have to execute subtrees.
	/// </summary>
	public static IEnumerator ExecutePath (UTAutomationPlanEntry entry, UTContext context)
	{
		while (entry != null) {
			if (context.CancelRequested) {
				yield break;
			}
			// can be overwritten by the entry or it's substructures.
			context.LocalProgress = -1;	
			context.CurrentEntry = entry;
			context.Me = entry.Me;
			var enumerator = entry.Execute (context);
			yield return "";
			while (enumerator.MoveNext()) {
				yield return "";
			}
			if (context.CancelRequested) {
				yield break;
			}
			entry = entry.NextEntry;
		}
	}
}
