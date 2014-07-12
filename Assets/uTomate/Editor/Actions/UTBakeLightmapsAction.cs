//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections;


[UTActionInfo(actionCategory = "Bake")]
[UTDoc(title="Bake Lightmaps", description="Bakes the lightmaps for the currently open scene using the given lightmap settings.")]
[UTDefaultAction]
public class UTBakeLightmapsAction : UTAction, UTICanLoadSettingsFromEditor
{
	
	[UTDoc(description="What should be baked?")]
	[UTInspectorHint(required=true, order=0)]
	public UTBakeType whatToBake;

	[UTDoc(description="When this is ticked, the bake settings from the current scene will be used. Otherwise, the settings from this action will be used.")]
	[UTInspectorHint(required=true, order=1)]
	public UTBool useSettingsFromScene;
	
	[UTDoc(description="The lightmaps mode.")]
	[UTInspectorHint(required=true, order=2)]
	public UTLightmapsMode lightMapsMode;
	
	[UTDoc(description="Boosts indirect light (Beast's diffuseBoost property, pow(colorComponent, (1.0 / diffuseBoost)))")]
	[UTInspectorHint(required=true, order=3)]
	public UTFloat bounceBoost;
	
	[UTDoc(description="Indirect light intensity multiplier.")]
	[UTInspectorHint(required=true, order=4)]
	public UTFloat bounceIntensity;
	
	[UTDoc(description="The maximum width of an individual lightmap texture.")]
	[UTInspectorHint(required=true, order=5)]
	public UTInt maxAtlasWidth;
	
	[UTDoc(description="The maximum height of an individual lightmap texture.")]
	[UTInspectorHint(required=true, order=6)]
	public UTInt maxAtlasHeight;
	
	[UTDoc(description="Lightmap resolution in texels per world unit.")]
	[UTInspectorHint(required=true, order=7)]
	public UTFloat resolution;
	
	[UTDoc(description="Sky light color.")]
	[UTInspectorHint(required=true, order=8)]
	public UTColor skyLightColor;

	[UTDoc(description="Sky light intensity.")]
	[UTInspectorHint(required=true, order=9)]
	public UTFloat skyLightIntensity;
	
	[UTDoc(description="Quality of the bake.")]
	[UTInspectorHint(required=true, order=10)]
	public UTLightmapBakeQuality quality;
	
	[UTDoc(description="Use DXT1 compression on the generated lightmaps?")]
	[UTInspectorHint(required=true, order=11)]
	public UTBool textureCompression;
	
	[UTDoc(description="Number of light bounces in the global illumination computation (with 0 meaning direct light only).")]
	[UTInspectorHint(required=true, order=12)]
	public UTInt bounces;
	
	[UTDoc(title="Gather Rays", description="Number of rays used in the final gather integrator.")]
	[UTInspectorHint(required=true, order=13)]
	public UTInt finalGatherRays;
	
	[UTDoc(title="Gather Contrast Threshold", description="Contrast threshold between neighbouring surface points.")]
	[UTInspectorHint(required=true, order=14)]
	public UTFloat finalGatherContrastTreshold;
	
	[UTDoc(title="Gather Gradient Threshold", description="Controls how the irradiance gradient is used in the interpolation.")]
	[UTInspectorHint(required=true, order=15)]
	public UTFloat finalGatherGradientTreshold;
	
	[UTDoc(title="Gather Interpolation Pts.", description="The number of final gather points to interpolate between.")]
	[UTInspectorHint(required=true, order=16)]
	public UTInt finalGatherInterpolationPoints;
	
	[UTDoc(description="Controls how much Ambient Occlusion to blend into the Final Gather solution.")]
	[UTInspectorHint(required=true, order=17)]
	public UTFloat aoAmount;
	
	[UTDoc(description="Beyond this distance a ray is considered to be unoccluded.")]
	[UTInspectorHint(required=true, order=18)]
	public UTFloat aoMaxDistance;
	
	[UTDoc(description="Controls the look of the transition from black to white.")]
	[UTInspectorHint(required=true, order=19)]
	public UTFloat aoContrast;
	
	[UTDoc(description="If enabled, Beast atlasing won't be run and lightmap indices, tiling and offset won't be modified on Mesh Renderers.")]
	[UTInspectorHint(required=true, order=20)]
	public UTBool lockAtlas;
	
	[UTDoc(description="Texel separation between shapes.")]
	[UTInspectorHint(required=true, order=21)]
	public UTInt padding;
	
	public override IEnumerator Execute (UTContext context)
	{
		if (!useSettingsFromScene.EvaluateIn(context)) {
			if (UTPreferences.DebugMode) {
				Debug.Log("Applying lightmap settings", this);
			}
			LightmapEditorSettings.aoAmount = aoAmount.EvaluateIn(context);
			LightmapEditorSettings.aoContrast = aoContrast.EvaluateIn(context);
			LightmapEditorSettings.aoMaxDistance = aoMaxDistance.EvaluateIn(context);
			LightmapEditorSettings.bounceBoost = bounceBoost.EvaluateIn(context);
			LightmapEditorSettings.bounceIntensity = bounceIntensity.EvaluateIn(context);
			LightmapEditorSettings.bounces = bounces.EvaluateIn(context);
			LightmapEditorSettings.finalGatherContrastThreshold = finalGatherContrastTreshold.EvaluateIn(context);
			LightmapEditorSettings.finalGatherGradientThreshold = finalGatherGradientTreshold.EvaluateIn(context);
			LightmapEditorSettings.finalGatherRays = finalGatherRays.EvaluateIn(context);
			LightmapEditorSettings.lockAtlas = lockAtlas.EvaluateIn(context);
			LightmapEditorSettings.maxAtlasHeight = maxAtlasHeight.EvaluateIn(context);
			LightmapEditorSettings.maxAtlasWidth = maxAtlasWidth.EvaluateIn(context);
			LightmapEditorSettings.padding = padding.EvaluateIn(context);
			LightmapEditorSettings.quality = quality.EvaluateIn(context);
			LightmapEditorSettings.resolution = resolution.EvaluateIn(context);
			LightmapEditorSettings.skyLightColor = skyLightColor.EvaluateIn(context);
			LightmapEditorSettings.skyLightIntensity = skyLightIntensity.EvaluateIn(context);
			LightmapEditorSettings.textureCompression = textureCompression.EvaluateIn(context);
			LightmapSettings.lightmapsMode = lightMapsMode.EvaluateIn(context);
			if (UTPreferences.DebugMode) {
				Debug.Log ("Lightmap settings applied.", this);		
			}
		}
		else {
			if (UTPreferences.DebugMode) {
				Debug.Log("Using the lightmap settings configured in the currently open scene.");
			}
		}
		var whatReallyToBake = whatToBake.EvaluateIn(context);
		switch(whatReallyToBake) {
		case UTTypeOfBake.Everything:
			Debug.Log("Building lightmaps for current scene. This may take a while.", this);
			if(! Lightmapping.BakeAsync()) {
				throw new UTFailBuildException("Lightmapping was not finished successfully.", this);
			}
			break;
		case UTTypeOfBake.SelectionOnly:
			Debug.Log("Building lightmaps for current selection. This may take a while.", this);
			if(! Lightmapping.BakeSelectedAsync()) {
				throw new UTFailBuildException("Lightmapping was not finished successfully.", this);
			}
			break;
		case UTTypeOfBake.LightProbesOnly:
			Debug.Log("Building light probes for current scene. This may take a while.", this);
			if (! Lightmapping.BakeLightProbesOnlyAsync()) {
				throw new UTFailBuildException("Lightmapping was not finished successfully.", this);
			}
			break;
		}
		do {
			yield return "";
			if (context.CancelRequested) {
				Lightmapping.Cancel();
			}
		} while (Lightmapping.isRunning);
		Debug.Log("Lightmapping finished.", this);
	}

	[MenuItem("Assets/Create/uTomate/Bake/Bake Lightmaps", false, 210)]
	public static void AddAction() {
		var action = Create<UTBakeLightmapsAction>();
		action.LoadSettings();
	}
	
	public enum UTTypeOfBake {
		Everything,
		SelectionOnly,
		LightProbesOnly
	}
	
	
	public string LoadSettingsUndoText {
		get {
			return "Load current lightmapping settings.";
		}
	}
	
	public void LoadSettings() {
		var action = this;
		
		action.aoAmount.Value = LightmapEditorSettings.aoAmount;
		action.aoAmount.UseExpression = false;
		
		action.aoContrast.Value = LightmapEditorSettings.aoContrast;
		action.aoContrast.UseExpression = false;
		
		action.aoMaxDistance.Value = LightmapEditorSettings.aoMaxDistance;
		action.aoMaxDistance.UseExpression = false;
		
		action.bounceBoost.Value = LightmapEditorSettings.bounceBoost;
		action.bounceBoost.UseExpression = false;
		
		action.bounceIntensity.Value = LightmapEditorSettings.bounceIntensity;
		action.bounceIntensity.UseExpression = false;
		
		action.bounces.Value = LightmapEditorSettings.bounces;
		action.bounces.UseExpression = false;
		
		action.finalGatherContrastTreshold.Value = LightmapEditorSettings.finalGatherContrastThreshold;
		action.finalGatherContrastTreshold.UseExpression = false;
		
		action.finalGatherGradientTreshold.Value = LightmapEditorSettings.finalGatherGradientThreshold;
		action.finalGatherGradientTreshold.UseExpression = false;

		action.finalGatherInterpolationPoints.Value = LightmapEditorSettings.finalGatherInterpolationPoints;
		action.finalGatherInterpolationPoints.UseExpression = false;
		
		action.finalGatherRays.Value = LightmapEditorSettings.finalGatherRays;
		action.finalGatherRays.UseExpression = false;
		
		action.lightMapsMode.Value = LightmapSettings.lightmapsMode;
		action.lightMapsMode.UseExpression = false;
		
		action.lockAtlas.Value = LightmapEditorSettings.lockAtlas;
		action.lockAtlas.UseExpression = false;
		
		action.maxAtlasHeight.Value = LightmapEditorSettings.maxAtlasHeight;
		action.maxAtlasHeight.UseExpression = false;
		
		action.maxAtlasWidth.Value = LightmapEditorSettings.maxAtlasWidth;
		action.maxAtlasWidth.UseExpression = false;
		
		action.padding.Value = LightmapEditorSettings.padding;
		action.padding.UseExpression = false;
		
		action.quality.Value = LightmapEditorSettings.quality;
		action.quality.UseExpression = false;
		
		action.resolution.Value = LightmapEditorSettings.resolution;
		action.resolution.UseExpression = false;
		
		action.skyLightColor.Value = LightmapEditorSettings.skyLightColor;
		action.skyLightColor.UseExpression = false;
		
		action.skyLightIntensity.Value = LightmapEditorSettings.skyLightIntensity;
		action.skyLightIntensity.UseExpression = false;
		
		action.textureCompression.Value = LightmapEditorSettings.textureCompression;
		action.textureCompression.UseExpression = false;
	}
}

