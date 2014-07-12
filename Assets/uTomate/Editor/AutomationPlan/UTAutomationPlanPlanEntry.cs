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
/// An entry which executes another plan as part of this plan.
/// </summary>
[Serializable]
[UTDoc(title="Run Plan", description="Runs another plan as part of this plan.")]
public class UTAutomationPlanPlanEntry : UTAutomationPlanEntryBase
{
	[UTDoc(description="The plan to run.")]
	[UTInspectorHint(required=true)]
	public UTAutomationPlan plan;
	
	public override string Label {
		get {
			return plan != null ? plan.name : "[No Plan]";
		}
	}
	
	public override IEnumerator Execute(UTContext context) {
		return plan.Execute(context);
	}
	
}

