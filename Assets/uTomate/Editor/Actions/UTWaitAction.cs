//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[UTDoc(title="Wait", description="An action which simply waits for some time.")]
[UTActionInfo(sinceUTomateVersion="1.1.0", actionCategory="General")]
[UTDefaultAction]
public class UTWaitAction : UTAction
{
	
	[UTDoc(description="The number of seconds to wait. You may also enter fractional seconds.")]
	public UTFloat secondsToWait;
	
	public override IEnumerator Execute (UTContext context)
	{
		
		float theSeconds = secondsToWait.EvaluateIn (context);
		
		DateTime start = DateTime.Now;
		
		if (UTPreferences.DebugMode) {
			Debug.Log ("Waiting for " + theSeconds.ToString ("00") + " seconds.");
		}
		
		do {
			if (context.CancelRequested) {
				yield break;
			}
			yield return "";
		} while((DateTime.Now-start).TotalSeconds < theSeconds);
		
	}
	
	[MenuItem("Assets/Create/uTomate/General/Wait",  false, 260)]
	public static void AddAction ()
	{
		Create<UTWaitAction> ();
	}
	
}
