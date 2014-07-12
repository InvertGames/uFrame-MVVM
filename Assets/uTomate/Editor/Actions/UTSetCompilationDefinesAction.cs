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
using System.IO;

[UTDoc(title="Set Scripting Define Symbols", description="Sets scripting preprocessor define symbols.")]
[UTActionInfo(actionCategory = "Build", sinceUTomateVersion="1.2.0")]
[UTDefaultAction]
public class UTSetCompilationDefinesAction : UTAction
{
#if !UNITY_3_5
	[UTInspectorHint(required=true, order=0)]
	[UTDoc(description="The target group for which the defines should be set.")]
	public UTBuildTargetGroup buildTargetGroup;
#endif
	[UTInspectorHint(required=true, order=1)]
	[UTDoc(description="The defines to be set. Put one define per line. If the list is empty all currently set defines will be unset.")]
	public UTString[] defines;
	
	public override IEnumerator Execute (UTContext context)
	{
#if UNITY_3_5		
		if (defines.Length == 0) {
			DeleteFileWithMeta(UTFileUtils.ProjectAssets + "/smcs.rsp");
			DeleteFileWithMeta(UTFileUtils.ProjectAssets + "/gmcs.rsp");
			DeleteFileWithMeta(UTFileUtils.ProjectAssets + "/us.rsp");
			DeleteFileWithMeta(UTFileUtils.ProjectAssets + "/boo.rsp");
		} else {
	 		string contents = "-define:"+string.Join(",",EvaluateAll(defines, context));		
			File.WriteAllText(UTFileUtils.ProjectAssets + "/smcs.rsp", contents);
			File.WriteAllText(UTFileUtils.ProjectAssets + "/gmcs.rsp", contents);
			File.WriteAllText(UTFileUtils.ProjectAssets + "/us.rsp", contents);
			File.WriteAllText(UTFileUtils.ProjectAssets + "/boo.rsp", contents);
		}
#endif
		
#if !UNITY_3_5
		PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup.EvaluateIn(context), string.Join(";", EvaluateAll(defines, context)));
#endif		
		yield return "";
	}

	private void DeleteFileWithMeta(string path) {
		SafeDelete(path);
		SafeDelete(path+".meta");
	}

	private void SafeDelete (string path)
	{
		if (File.Exists (path)) {
			File.Delete (path);
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Scripting Define Symbols", false, 490)]
	public static void AddAction ()
	{
		Create<UTSetCompilationDefinesAction> ();
	}
}

