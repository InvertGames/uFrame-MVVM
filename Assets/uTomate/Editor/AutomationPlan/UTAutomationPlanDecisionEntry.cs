//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// An entry which makes a decision and then performs one or another entry.
/// </summary>
[Serializable]
[UTDoc(title="Decision", description="Allows you to run a certain path of the automation plan based on a decision.")]
public class UTAutomationPlanDecisionEntry : UTAutomationPlanEntry
{
	[UTDoc(description="The entry to perform if the decision yields 'true'.")]
	[HideInInspector]
	[UTConnectorHint(connectorLocation=UTConnectorHint.ConnectorLocation.Bottom)]
	public UTAutomationPlanEntry entryIfTrue;
	
	[UTDoc(description="The entry to perform if the decision yields 'false'.")]
	[HideInInspector]
	[UTConnectorHint(connectorLocation=UTConnectorHint.ConnectorLocation.Bottom)]
	public UTAutomationPlanEntry entryIfFalse;
	
	[UTDoc(description="The decision to be made.")]
	public UTBool decision;
	
	private UTAutomationPlanEntry result;
	
	public override string Label {
		get {
			return "Decision";
		}
	}
	
	public override object Me {
		get {
			return null;
		}
	}
	
	public override IEnumerator Execute(UTContext context) {
		if (decision.EvaluateIn(context)) {
			if(UTPreferences.DebugMode) {
				Debug.Log("Decision yielded true", this);
			}
			result = entryIfTrue;
		}
		else {
			if (UTPreferences.DebugMode) {
				Debug.Log("Decision yielded false", this);
			}
			result = entryIfFalse;
		}
		yield return "";
	}
	
	public override UTAutomationPlanEntry NextEntry {
		get {
			return result;
		}
	}
}

