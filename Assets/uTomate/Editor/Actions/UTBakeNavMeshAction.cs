//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

[UTActionInfo(actionCategory = "Bake")]
[UTDoc(title="Bake Nav Mesh", description="Bakes the navigation mesh for the current scene.")]
[UTRequiresLicense(UTLicense.UnityPro)]
[UTInspectorGroups(groups=new string[]{"General", "Generated Off Mesh Links", "Advanced"})]
[UTDefaultAction]
public class UTBakeNavMeshAction : UTAction
{
	
	[UTDoc(description="radius of the \"typical\" agent (preferrably the smallest).")]
	[UTInspectorHint(required=true, order=1, group="General")]
	public UTFloat agentRadius;
	[UTDoc(description="height of the \"typical\" agent (the \"clearance\" needed to get a character through).")]
	[UTInspectorHint(required=true, order=2, group="General")]
	public UTFloat agentHeight;
	[UTDoc(description="all surfaces with higher slope than this, will be discarded.")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.Slider, minValue=0f, maxValue=90f, required=true, group="General", order=3)]
	public UTFloat maxSlope;
	[UTDoc(description="the height difference below which navmesh regions are considered connected.")]
	[UTInspectorHint(required=true, order=4, group="General")]
	public UTFloat stepHeight;
	[UTDoc(description="If the value of this property is positive, off-mesh links will be placed for adjacent navmesh surfaces where the height difference is below this value.")]
	[UTInspectorHint(required=true, order=1, group="Generated Off Mesh Links")]
	public UTFloat dropHeight;
	[UTDoc(description="If the value of this property is positive, off-mesh links will be placed for adjacent navmesh surfaces where the horizontal distance is below this value.")]
	[UTInspectorHint(required=true, order=2, group="Generated Off Mesh Links")]
	public UTFloat jumpDistance;
	[UTDoc(description="Regions with areas below this threshold will be discarded.")]
	[UTInspectorHint(required=true, order=1, group="Advanced")]
	public UTFloat minRegionArea;
	[UTDoc(description="Allowable width inaccuracy")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.Slider, minValue=0f, maxValue=100f, order=2, group="Advanced", required=true)]
	public UTFloat widthInaccuracy;
	[UTDoc(description="Allowable height inaccuracy")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.Slider, minValue=0f, maxValue=100f,  order=3, group="Advanced", required=true)]
	public UTFloat heightInaccuracy;
	[UTDoc(description="If this options is on, original height information is stored. This has performance implications for speed and memory usage.")]
	[UTInspectorHint(required=true, order=4, group="Advanced")]
	public UTBool heightMesh;

	public override IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			Debug.Log ("Setting up nav mesh settings.");
		}
		var settingsObject = new SerializedObject (NavMeshBuilder.navMeshSettingsObject);
		var agentRadiusSetting = settingsObject.FindProperty ("m_BuildSettings.agentRadius");
		var agentHeightSetting = settingsObject.FindProperty ("m_BuildSettings.agentHeight");
		var agentSlopeSetting = settingsObject.FindProperty ("m_BuildSettings.agentSlope");
		var ledgeDropHeightSetting = settingsObject.FindProperty ("m_BuildSettings.ledgeDropHeight");
		var agentClimbSetting = settingsObject.FindProperty ("m_BuildSettings.agentClimb");
		var maxJumpAcrossDistanceSetting = settingsObject.FindProperty ("m_BuildSettings.maxJumpAcrossDistance");
		var accuratePlacementSetting = settingsObject.FindProperty ("m_BuildSettings.accuratePlacement");
		var minRegionAreaSetting = settingsObject.FindProperty ("m_BuildSettings.minRegionArea");
		var widthInaccuracySetting = settingsObject.FindProperty ("m_BuildSettings.widthInaccuracy");
		var heightInaccuracySetting = settingsObject.FindProperty ("m_BuildSettings.heightInaccuracy");
		
		agentRadiusSetting.floatValue = agentRadius.EvaluateIn(context);
		agentHeightSetting.floatValue = agentHeight.EvaluateIn(context);
		agentSlopeSetting.floatValue = maxSlope.EvaluateIn(context);
		ledgeDropHeightSetting.floatValue = dropHeight.EvaluateIn(context);
		agentClimbSetting.floatValue = stepHeight.EvaluateIn(context);
		maxJumpAcrossDistanceSetting.floatValue = jumpDistance.EvaluateIn(context);
		accuratePlacementSetting.boolValue = heightMesh.EvaluateIn(context);
		minRegionAreaSetting.floatValue = minRegionArea.EvaluateIn(context);
		widthInaccuracySetting.floatValue = widthInaccuracy.EvaluateIn(context);
		heightInaccuracySetting.floatValue = heightInaccuracy.EvaluateIn(context);
		
		settingsObject.ApplyModifiedProperties();
		
		Debug.Log ("Starting baking of nav mesh.");
		NavMeshBuilder.BuildNavMeshAsync ();
		do {
			yield return "";
			if (context.CancelRequested) {
				NavMeshBuilder.Cancel ();
			}
		} while(NavMeshBuilder.isRunning);
		Debug.Log ("Nav mesh bake process finished.");
	}
	
	[MenuItem("Assets/Create/uTomate/Bake/Bake Nav Mesh", false, 220)]
	public static void AddAction ()
	{
		var action = Create<UTBakeNavMeshAction> ();
		LoadFromSettings(action);
	}
	

	public static void LoadFromSettings(UTBakeNavMeshAction action) {
		var settingsObject = new SerializedObject (NavMeshBuilder.navMeshSettingsObject);
		var agentRadiusSetting = settingsObject.FindProperty ("m_BuildSettings.agentRadius");
		var agentHeightSetting = settingsObject.FindProperty ("m_BuildSettings.agentHeight");
		var agentSlopeSetting = settingsObject.FindProperty ("m_BuildSettings.agentSlope");
		var ledgeDropHeightSetting = settingsObject.FindProperty ("m_BuildSettings.ledgeDropHeight");
		var agentClimbSetting = settingsObject.FindProperty ("m_BuildSettings.agentClimb");
		var maxJumpAcrossDistanceSetting = settingsObject.FindProperty ("m_BuildSettings.maxJumpAcrossDistance");
		var accuratePlacementSetting = settingsObject.FindProperty ("m_BuildSettings.accuratePlacement");
		var minRegionAreaSetting = settingsObject.FindProperty ("m_BuildSettings.minRegionArea");
		var widthInaccuracySetting = settingsObject.FindProperty ("m_BuildSettings.widthInaccuracy");
		var heightInaccuracySetting = settingsObject.FindProperty ("m_BuildSettings.heightInaccuracy");
	
		action.agentRadius.UseExpression = false;
		action.agentRadius.Value = agentRadiusSetting.floatValue;
		
		action.agentHeight.UseExpression = false;
		action.agentHeight.Value = agentHeightSetting.floatValue;
		
		action.stepHeight.UseExpression = false;
		action.stepHeight.Value = agentClimbSetting.floatValue;
		
		action.maxSlope.UseExpression = false;
		action.maxSlope.Value = agentSlopeSetting.floatValue;
		
		action.dropHeight.UseExpression = false;
		action.dropHeight.Value = ledgeDropHeightSetting.floatValue;
		
		action.jumpDistance.UseExpression = false;
		action.jumpDistance.Value = maxJumpAcrossDistanceSetting.floatValue;
		
		action.heightMesh.UseExpression = false;
		action.heightMesh.Value = accuratePlacementSetting.boolValue;
		
		action.minRegionArea.UseExpression = false;
		action.minRegionArea.Value = minRegionAreaSetting.floatValue;
		
		action.widthInaccuracy.UseExpression = false;
		action.widthInaccuracy.Value = widthInaccuracySetting.floatValue;
		
		action.heightInaccuracy.UseExpression = false;
		action.heightInaccuracy.Value = heightInaccuracySetting.floatValue;
	}

}

