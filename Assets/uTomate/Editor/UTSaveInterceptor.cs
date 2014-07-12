//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Interceptor which cleans up unused entries in automation plans.
/// </summary> 
#if UNITY_3_5
public class UTSaveInterceptor : AssetModificationProcessor
#else
public class UTSaveInterceptor : UnityEditor.AssetModificationProcessor  // moved in Unity4
#endif
{
	public static void OnWillSaveAssets(string[] paths) {
		foreach(var asset in paths) {
			UTils.ClearUnusedEntriesIn(asset);
		}
	}
	
}

