//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[Serializable]
public class UTAutomationPlanPopup : EditorWindow, CUListExecutionListener {
	
	private static int Width = 300;
	private static int Height = 300;
	
	private static UTAutomationPlanPopup instance;	
	
	private static bool visible = false;
	private static bool requestFocus = false;
	private static bool forceDelayedClose = false;
	private static bool wasGuiPaintedAtLeastOnce = false;
	
	private string caption;
	private CUListData listData;
	private List<UTAutomationPlan> allItems;
	private List<UTAutomationPlan> visibleItems;
	private List<UTAutomationPlan> currentItems;
	private bool showAllItems;
	private AutomationPlanSelected selectionCallback;
	private UTAutomationPlanListItemRenderer itemRenderer;
	
	private static UTAutomationPlanPopup Instance {
		get {
			if (instance == null) {
				instance = ScriptableObject.CreateInstance<UTAutomationPlanPopup>();
			}
			return instance;
		}
	}
	
	public static void ShowDialog(string caption, List<UTAutomationPlan> allItems, List<UTAutomationPlan> visibleItems, AutomationPlanSelected selectionCallback) {
		UTAutomationPlanPopup popup = Instance;
		popup.caption = caption;
		popup.allItems = allItems;
		popup.visibleItems = visibleItems;
		popup.currentItems = visibleItems;
		popup.showAllItems = false;
		popup.selectionCallback = selectionCallback;
		popup.listData = new CUListData(false);
		popup.listData.ExecutionListener = popup;
		
		if (popup.currentItems.Count > 0) {
			// select first entry
			popup.listData[0] = true;
		}
		
		if (!visible) {
			requestFocus = true;
			visible = true;
			wasGuiPaintedAtLeastOnce = false;
			forceDelayedClose = false;
			popup.itemRenderer = new UTAutomationPlanListItemRenderer();
			popup.position = new Rect(Mathf.Max(0, (Screen.width - Width) / 2), Mathf.Max(0, (Screen.height - Height) / 2), Width, Height);
			popup.ShowPopup();
			popup.Focus();
		}
	}

	public void OnInspectorUpdate() {
		// closing the dialog within OnGUI or OnLostFocus leads to several exceptions but this position seems perfect
		if (forceDelayedClose) {
			CloseDialog();
		}
	}
	
	public void OnGUI() {		
		if (requestFocus && Event.current.type == EventType.Layout) {
			requestFocus = false;
			EditorWindow.FocusWindowIfItsOpen<UTAutomationPlanPopup>();
		}

		EditorGUILayout.LabelField(caption);
		EditorGUILayout.Space();

		GUI.SetNextControlName("AutomationPlanList");
		listData = CUListControl.SelectionList<UTAutomationPlan>(listData, currentItems, itemRenderer, "Plans", GUILayout.ExpandHeight(true));	
		if (GUI.GetNameOfFocusedControl() == string.Empty) {
			// move focus to the textfield after the dialog is visible
		    GUI.FocusControl("AutomationPlanList");
		}		
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		
		EditorGUI.BeginChangeCheck ();
		showAllItems = GUILayout.Toggle(showAllItems, "Show hidden plans");
		if (EditorGUI.EndChangeCheck ()) {
			currentItems = showAllItems ? allItems : visibleItems;
			listData.ClearSelection();
			if (currentItems.Count > 0) {
				listData[0] = true;
			}
		}
		
		GUILayout.FlexibleSpace();
		GUI.enabled = !listData.Empty;
		if (GUILayout.Button("Ok")) {
			Apply(listData.First);
		}
		GUI.enabled = true;
		if (GUILayout.Button("Cancel")) {
			CloseDialog();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) {
			CloseDialog();
		}	
		wasGuiPaintedAtLeastOnce = true;
	}
	
	private void Apply(int index) {
		selectionCallback(currentItems[index]);
		CloseDialog();
	}
	
	private void CloseDialog() {
		allItems = null;
		visibleItems = null;
		currentItems = null;
		selectionCallback = null;
		visible = false;
		forceDelayedClose = false;
		Close();
		if (Event.current != null) {
			Event.current.Use();	
		}
	}
	
	public bool HandleExecution (List<int> indices) {
		if (indices != null && indices.Count == 1) {
			Apply(indices[0]);
			return true;
		}
		return false;
	}
	
	public void OnLostFocus() {
		forceDelayedClose = wasGuiPaintedAtLeastOnce;
	}
	
	public delegate void AutomationPlanSelected(UTAutomationPlan plan);

}
