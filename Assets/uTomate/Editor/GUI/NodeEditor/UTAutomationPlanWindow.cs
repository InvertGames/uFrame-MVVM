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
using UObject = UnityEngine.Object;

/// <summary>
/// uTomate's automation plan editor window.
/// </summary>
public class UTAutomationPlanWindow : EditorWindow
{
	
	private const string InspectorWidth = "SUTAutomationPlanWindow.InspectorWidth";
	private const int minInspectorWidth = 300;
	private UTAutomationPlan plan;
	private UTNodeEditorModel editorModel = new UTNodeEditorModel ();
	private UTNodeEditorData editorData = new UTNodeEditorData ();
	private float inspectorWidth = minInspectorWidth;
	private UTInspectorRenderer inspectorRenderer;
	
	[MenuItem("Window/uTomate/Automation Plan Editor")]
	public static void Init ()
	{
		UTAutomationPlanWindow window = EditorWindow.GetWindow<UTAutomationPlanWindow> ("Plan Editor");	
		window.minSize = new Vector2(800, 400);
		window.plan = null;
	}
	
 	[MenuItem ("Assets/Edit in Automation Plan Editor", true)]
    static bool ValidateEditAutomationPlan () {
    	return Selection.activeObject is UTAutomationPlan;
	}
	
 	[MenuItem ("Assets/Edit in Automation Plan Editor", false, 710)]
    static void EditAutomationPlan () {
		EditAutomationPlan(Selection.activeObject as UTAutomationPlan);
	}
	
	public static void EditAutomationPlan(UTAutomationPlan plan) {
		UTAutomationPlanWindow window = EditorWindow.GetWindow<UTAutomationPlanWindow> ();	
		window.title = "Plan Editor";			
		window.plan = plan;
		window.RefreshInternals();
	}
	
	public void OnEnable ()
	{
		inspectorWidth = EditorPrefs.GetFloat (InspectorWidth, minInspectorWidth);
		editorData.DropTarget = new UTNodeEditorDropTarget (DropActions, editorModel);
		inspectorRenderer = new UTInspectorRenderer ();
		RefreshInternals ();
	}
	
	public void OnDisable ()
	{
		EditorPrefs.SetFloat (InspectorWidth, inspectorWidth);
	}
	
	private void RefreshInternals ()
	{
		if (plan != null) {
			editorModel.LoadPlan (plan);
		}
	}
	
	public void OnGUI ()
	{
		EditorGUI.BeginChangeCheck ();
		plan = EditorGUILayout.ObjectField (plan, typeof(UTAutomationPlan), false) as UTAutomationPlan;	
		if (EditorGUI.EndChangeCheck ()) {
			RefreshInternals ();
		}
			
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical();
		UTNodeEditor.NodeEditor (editorData, editorModel, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

#if UTOMATE_DEMO
		if(Event.current.type == EventType.Repaint) {
			var lastRect = GUILayoutUtility.GetLastRect();
			var content = new GUIContent("Demo Version");
			var size = UTEditorResources.DemoVersionLabelStyle.CalcSize(content);
			lastRect.Set(lastRect.xMin + lastRect.width - size.x, lastRect.yMin, size.x, size.y);
			GUI.Label(lastRect, content, UTEditorResources.DemoVersionLabelStyle);
		}
#endif

		EditorGUILayout.BeginHorizontal();
		GUI.enabled = editorModel.HasPlan;
		if (GUILayout.Button("Layout")) {
			editorModel.RelayoutPlan();
		}
		GUI.enabled = true;		
		GUILayout.FlexibleSpace();		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();		
		EditorGUILayout.EndVertical();

		// Inspector
		inspectorWidth = CUResizableContainer.BeginHorizontal (inspectorWidth, minInspectorWidth, 3 * minInspectorWidth, CUResizableContainer.HandlePosition.Before);	
		EditorGUILayout.BeginVertical ();
		
		editorData.inspectorScrollPosition = EditorGUILayout.BeginScrollView (editorData.inspectorScrollPosition);
		// current action.
		if (editorModel.SelectedEntry != null) {
			inspectorRenderer.target = editorModel.SelectedEntry;
			inspectorRenderer.OnInspectorGUI ();
		}
		EditorGUILayout.EndScrollView ();

		GUILayout.FlexibleSpace ();
		
		// Render plan settings
		if (plan != null) {
			inspectorRenderer.target = plan;
			inspectorRenderer.OnInspectorGUI();
		}
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		GUI.enabled = plan != null;
		if (GUILayout.Button ("Run this plan", EditorStyles.miniButton)) {
			plan.Execute ();
			GUIUtility.ExitGUI ();
		}
		GUI.enabled = true;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
		CUResizableContainer.EndHorizontal ();
		// Inspector
		
		EditorGUILayout.EndHorizontal ();
		
		if (UTomateRunner.Instance.IsRunning) {
			editorModel.HighlightedEntry = UTomateRunner.Instance.CurrentEntry;
			Repaint();
		}
		else {
			editorModel.HighlightedEntry = null;
		}
	}
	
	private void DropActions (UObject[] items, Vector2 position)
	{
		foreach (var item in items) {
#if UTOMATE_DEMO
			if (!editorModel.CanAddEntriesToPlan) {
				if (!EditorUtility.DisplayDialog("Locked in demo version", 
				                                 "The demo version is limited to " + UTomate.MaxEntriesForDemo + 
				                                 " entries per automation plan. Please remove a few entries from " + name + " and try again.", 
				                                 "Ok", "Buy uTomate now!")) {
					Application.OpenURL(UTomate.AssetStoreUrl);
				}
				return; // don't add the rest
			}
#endif
			
			if (item is UTAction) {
				editorModel.AddAction (item as UTAction, position);
			}
			if (item is UTAutomationPlan) {
				editorModel.AddSubPlan(item as UTAutomationPlan, position);
			}
		}
	}
	
	
	public void OnInspectorUpdate() {
		Repaint();
	}
}
