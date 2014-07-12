using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;

[UTActionInfo(actionCategory="General", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Select Asset", description="Selects the asset at the given path.")]
[UTDefaultAction]
public class UTSelectAssetAction : UTAction
{
	
	[UTDoc(description="The path to the asset that should be selected.")]
	[UTInspectorHint(required=true)]
	public UTString assetPath;
	
	public override IEnumerator Execute (UTContext context)
	{
		var pathToInstance = assetPath.EvaluateIn (context);
		pathToInstance = UTFileUtils.FullPathToProjectPath (pathToInstance);
		
		var theAsset = AssetDatabase.LoadMainAssetAtPath (pathToInstance);
		Selection.activeObject = theAsset;
		
		yield return "";
		
	}
	
	
	[MenuItem("Assets/Create/uTomate/General/Select Asset", false, 380)]
	public static void AddAction ()
	{
		Create<UTSelectAssetAction> ();
	}
	
	
}
