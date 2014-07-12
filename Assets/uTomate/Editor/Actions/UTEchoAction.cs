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

[UTDoc(title="Echo", description="Writes the given text to the console.")]
[UTActionInfo(actionCategory="General")]
[UTDefaultAction]
public class UTEchoAction : UTAction
{
	
	[UTInspectorHint(required=true, order=0)]
	[UTDoc(description="The text to be written.")]
	public UTString text;
	
	[UTDoc(description="Only write to console when in debug mode.")]
	[UTInspectorHint(order=1)]
	public UTBool onlyInDebugMode;
	
	public override IEnumerator Execute (UTContext context)
	{
		var debugMode = onlyInDebugMode.EvaluateIn(context);
		
		if (!debugMode || UTPreferences.DebugMode) {
			var evaluatedText = text.EvaluateIn(context);
			Debug.Log(evaluatedText, this);
		}
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/General/Echo", false, 240)]
	public static void AddAction() {
		Create<UTEchoAction>();
	}
}

