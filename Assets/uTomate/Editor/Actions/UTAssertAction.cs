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

[UTActionInfo(sinceUTomateVersion="1.2.0", actionCategory="General")]
[UTDoc(title="Assert", description="Checks if one or more conditions are true. If one condition is false, aborts the current plan execution.")]
[UTDefaultAction]
public class UTAssertAction : UTAction
{
	
	[UTInspectorHint(required=true, order=0)]
	[UTDoc(description="The conditions to be checked. Write one condition per line. You will almost always want to use expression mode for these.")]
	public UTBool[] conditions;
	
	[UTDoc(description="A message that should be printed when one of the conditions doesn't hold true. If not set, no special message will be displayed.")]
	[UTInspectorHint(order=1)]
	public UTString messageOnFail;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theMessage = messageOnFail.EvaluateIn(context);
		
		foreach (var condition in conditions) {
			if (UTPreferences.DebugMode) {
				if (condition.UseExpression) {
					Debug.Log ("Evaluating expression " + condition.Expression);
				}
			}
			
			var result = condition.EvaluateIn (context);
			if (result == false) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Condition was not true. Aborting.");
				}
				if (!string.IsNullOrEmpty(theMessage)) {
					Debug.LogError(theMessage);
				}
				throw new UTFailBuildException ("Assertion failed. Aborting plan execution.", this);
			}
			else {
				if (UTPreferences.DebugMode) {
					Debug.Log("Condition was true. Continuing.");
				}
			}
		}
		
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/General/Assert", false, 250)]
	public static void AddAction ()
	{
		Create<UTAssertAction> ();
	}
}

