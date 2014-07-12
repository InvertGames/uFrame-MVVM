//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;

[UTActionInfo(actionCategory = "Build", sinceUTomateVersion="1.1.0")]
[UTDoc(title="Set Cross-Platform Player Settings", description="Sets the player settings shared by all platforms.")]
[UTDefaultAction]
public class UTSetPlayerSettingsSharedAction : UTAction, UTICanLoadSettingsFromEditor
{
	[UTDoc(description="The company name.")]
	public UTString companyName;
	
	[UTDoc(description="The product name.")]
	public UTString productName;
	
	[UTDoc(description="The default icon.")]
	public UTTexture2D defaultIcon;
	
	public override IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			Debug.Log ("Modifying cross platform player settings.", this);
		}
	
		PlayerSettings.companyName = companyName.EvaluateIn(context);
		PlayerSettings.productName = productName.EvaluateIn(context);
		PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[]{defaultIcon.EvaluateIn(context)});
		if (UTPreferences.DebugMode) {
			Debug.Log ("Cross platform player settings modified.", this);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Set Cross-Platform Player Settings",  false, 230)]
	public static void AddAction ()
	{
		var result = Create<UTSetPlayerSettingsSharedAction> ();
		result.LoadSettings ();
	}
	
	public string LoadSettingsUndoText {
		get {
			return "Load cross platform player settings.";
		}
	}
	
	/// <summary>
	/// Loads current player settings.
	/// </summary>
	public void LoadSettings ()
	{
		companyName.Value = PlayerSettings.companyName;
		companyName.UseExpression = false;
		
		productName.Value = PlayerSettings.productName;
		productName.UseExpression = false;
	
		defaultIcon.Value = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown)[0];
		defaultIcon.UseExpression = false;
	}
}
