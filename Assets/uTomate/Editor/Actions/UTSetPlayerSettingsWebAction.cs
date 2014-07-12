//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Set Web Player Settings", description="Sets the player settings for Web player builds.")]
[UTInspectorGroups(groups=new string[]{"Resolution & Presentation", "Rendering", "Streaming", "Optimization"})]
[UTDefaultAction]
public class UTSetPlayerSettingsWebAction : UTAction,UTICanLoadSettingsFromEditor
{
	[UTDoc(description="Default screen width of the web player window.")]
	[UTInspectorHint(group="Resolution & Presentation", order=1)]
	public UTInt defaultScreenWidth;
	[UTDoc(description="Default screen height of the web player window.")]
	[UTInspectorHint(group="Resolution & Presentation", order=2)]
	public UTInt defaultScreenHeight;
	[UTDoc(description="Continue running when application loses focus?")]
	[UTInspectorHint(group="Resolution & Presentation", order=3)]
	public UTBool runInBackground;
	//
#if !UNITY_3_5	
	[UTDoc(description="The rendering path to use")]
	[UTInspectorHint(group="Rendering", order=1)]
	public UTRenderingPath renderingPath;
#endif	
	[UTDoc(description="The color space for lightmaps.")]
	[UTInspectorHint(group="Rendering", order=2)]
	public UTColorSpace colorSpace;
	
#if !UNITY_3_5	
	[UTDoc(description="Use Direct3D 11?", title="Use Direct3D 11")]
	[UTInspectorHint(group="Rendering", order=3)]
	public UTBool useDirect3D11;
#endif 	
	//
	[UTDoc(description="First Streamed Level")]
	[UTInspectorHint(group="Streaming", order=1)]
	public UTInt firstStreamedLevel;
	
	//
	[UTDoc(description="Should unused Mesh components be excluded from game build?")]
	[UTInspectorHint(group="Optimization", order=6)]
	public UTBool optimizeMeshData;
#if UNITY_3_5	
	[UTDoc(description="Outputs profiling information about Resources.UnloadUnusedAssets,")]
	[UTInspectorHint(group="Optimization", order=7)]
	public UTDebugUnloadMode debugUnloadMode;
#endif 
	
	public override IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			Debug.Log ("Modifying Web player settings.", this);
		}
	
		PlayerSettings.defaultWebScreenWidth = defaultScreenWidth.EvaluateIn (context);
		PlayerSettings.defaultScreenHeight = defaultScreenHeight.EvaluateIn (context);
		PlayerSettings.runInBackground = runInBackground.EvaluateIn (context);

#if !UNITY_3_5
		PlayerSettings.renderingPath = renderingPath.EvaluateIn(context);
#endif
		PlayerSettings.colorSpace = colorSpace.EvaluateIn(context);
		
#if !UNITY_3_5
		PlayerSettings.useDirect3D11 = useDirect3D11.EvaluateIn(context);
#endif
		PlayerSettings.firstStreamedLevelWithResources = firstStreamedLevel.EvaluateIn(context);
		
		PlayerSettings.stripUnusedMeshComponents = optimizeMeshData.EvaluateIn (context);
#if UNITY_3_5
		PlayerSettings.debugUnloadMode = debugUnloadMode.EvaluateIn(context);
#endif		
		
		if (UTPreferences.DebugMode) {
			Debug.Log ("Web player settings modified.", this);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Set Web Player Settings",  false, 260)]
	public static void AddAction ()
	{
		var result = Create<UTSetPlayerSettingsWebAction> ();
		result.LoadSettings ();
	}
	
	
	/// <summary>
	/// Loads current player settings.
	/// </summary>
	public void LoadSettings ()
	{
		
		defaultScreenWidth.Value = PlayerSettings.defaultWebScreenWidth;
		defaultScreenWidth.UseExpression = false;
		
		defaultScreenHeight.Value = PlayerSettings.defaultWebScreenHeight;
		defaultScreenHeight.UseExpression = false;
		
		runInBackground.Value = PlayerSettings.runInBackground;
		runInBackground.UseExpression = false;
		
#if !UNITY_3_5
		renderingPath.Value = PlayerSettings.renderingPath;
		renderingPath.UseExpression = false;
#endif 
		colorSpace.Value = PlayerSettings.colorSpace;
		colorSpace.UseExpression = false;

#if !UNITY_3_5
		useDirect3D11.Value = PlayerSettings.useDirect3D11;
		useDirect3D11.UseExpression = false;
#endif 
		
		firstStreamedLevel.Value = PlayerSettings.firstStreamedLevelWithResources;
		firstStreamedLevel.UseExpression = false;
				
		optimizeMeshData.Value = PlayerSettings.stripUnusedMeshComponents;
		optimizeMeshData.UseExpression = false;

#if UNITY_3_5
		debugUnloadMode.Value = PlayerSettings.debugUnloadMode;
		debugUnloadMode.UseExpression = false;
#endif		
	}
	
	public string LoadSettingsUndoText {
		get {
			return "Load Web player settings";
		}
	}
}
