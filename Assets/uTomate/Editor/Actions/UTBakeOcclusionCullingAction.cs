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
[UTDoc(title="Bake Occlusion Culling", description="Bakes the occlusion culling for the current scene.")]
[UTRequiresLicense(UTLicense.UnityPro)]
[UTDefaultAction]
public class UTBakeOcclusionCullingAction : UTAction
{
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
	[UTDoc(description="Select between the types of occlusion culling baking.")]
	[UTInspectorHint(order=0, required=true)]
	public UTStaticOcclusionCullingMode mode;

	[UTDoc(description="Size of each view area cell. A smaller value produces more accurate occlusion culling. The value is a tradeoff between occlusion accuracy and storage size")]
	[UTInspectorHint(order=1, required=true)]
	public UTFloat viewCellSize;
	
	[UTDoc(description="Near clip plane should be set to the smallest near clip plane that will be used in the game of all the cameras.")]
	[UTInspectorHint(order=2, required=true)]
	public UTFloat nearClipPane;
	
	[UTDoc(description="Far Clip Plane used to cull the objects. Any object whose distance is greater than this value will be occluded automatically.Should be set to the largest far clip planed that will be used in the game of all the cameras")]
	[UTInspectorHint(order=3, required=true)]
	public UTFloat farClipPane;
	
	[UTDoc(description="This is a hint for the PVS-based baking, not available in Automatic Portal Generation mode")]
	[UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.Slider, minValue=500000, maxValue=30000000, order=4, required=true)]
	public UTInt memoryLimit;
#else 	
	// Unity 4.3+
	[UTDoc(description="The size of the smallest object that will be used to hide other objects when doing occlusion culling.")]
	[UTInspectorHint(order=1, required=true)]
	public UTFloat smallestOccluder;
	[UTDoc(description="The smallest hole in the geometry through which the camera is supposed to see.")]
	[UTInspectorHint(order=2, required=true)]
	public UTFloat smallestHole;
	[UTDoc(description="The backface threshold is a size optimization that reduces unneccessary details by testing backfaces.")]
	[UTInspectorHint(displayAs = UTInspectorHint.DisplayAs.Slider, minValue=5.0f, maxValue=100f, order=3, required=true)]
	public UTFloat backfaceThreshold;
#endif

	public override IEnumerator Execute (UTContext context)
	{
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
		var realCellSize = viewCellSize.EvaluateIn(context);
		var realNearClipPane = nearClipPane.EvaluateIn(context);
		var realFarClipPane = farClipPane.EvaluateIn(context);
		var realMemoryLimit = memoryLimit.EvaluateIn(context);
		var realMode = mode.EvaluateIn(context);
		
		Debug.Log("Starting baking of occlusion culling.");
		
		StaticOcclusionCulling.GenerateInBackground(realCellSize, realNearClipPane, realFarClipPane, realMemoryLimit, realMode);
#else
		var realSmallestOccluder = smallestOccluder.EvaluateIn(context);
		var realSmallestHole = smallestHole.EvaluateIn(context);
		var realBackfaceThreshold = backfaceThreshold.EvaluateIn(context);

		Debug.Log("Starting baking of occlusion culling.");

		StaticOcclusionCulling.smallestOccluder = realSmallestOccluder;
		StaticOcclusionCulling.smallestHole = realSmallestHole;
		StaticOcclusionCulling.backfaceThreshold = realBackfaceThreshold;

		StaticOcclusionCulling.GenerateInBackground();
#endif
		do {
			yield return "";
			if (context.CancelRequested) {
				StaticOcclusionCulling.Cancel();
			}
		}
		while(StaticOcclusionCulling.isRunning);
		Debug.Log("Occlusion culling bake process finished.");
	}
	
	[MenuItem("Assets/Create/uTomate/Bake/Bake Occlusion Culling", false, 230)]
	public static void AddAction ()
	{
		var action = Create<UTBakeOcclusionCullingAction> ();
		LoadDefaults(action);
	}
	
	public static void LoadDefaults(UTBakeOcclusionCullingAction action) {
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
		action.viewCellSize.UseExpression = false;
		action.viewCellSize.Value = 1f;
		
		action.nearClipPane.UseExpression  = false;
		action.nearClipPane.Value  = 0.3f;

		action.farClipPane.UseExpression  = false;
		action.farClipPane.Value  = 1000f;

		action.memoryLimit.UseExpression  = false;
		action.memoryLimit.Value  = 10000000;
#else
		action.smallestOccluder.UseExpression = false;
		action.smallestOccluder.Value = 5f;

		action.smallestHole.UseExpression = false;
		action.smallestHole.Value = 0.25f;

		action.backfaceThreshold.UseExpression = false;
		action.backfaceThreshold.Value = 100f;

#endif
	}
}

