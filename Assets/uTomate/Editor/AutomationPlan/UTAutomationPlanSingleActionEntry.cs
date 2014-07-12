//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// An entry which executes a single action. This is the bread-and-butter entry that is used to execute actions
/// that the user defined.
/// </summary>
[Serializable]
[UTDoc(title="Run Action", description="Runs a single action.")]
public class UTAutomationPlanSingleActionEntry : UTAutomationPlanEntryBase
{
	[UTDoc(description="The action to perform.")]
	public UTAction action;
	
	public override string Label {
		get {
			return action != null ? action.name : "[No Action]";
		}
	}
	
	public override object Me {
		get {
			return action;
		}
	}
	
	public override IEnumerator Execute(UTContext context) {
		return action.Execute(context);
	}
}

