//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Base class for entries which contain at least a next entry field.
/// </summary>
[HideInInspector]
public abstract class UTAutomationPlanEntryBase : UTAutomationPlanEntry
{

	[UTDoc(description="The entry that should be executed after this.")]
	[UTConnectorHint(connectorLocation=UTConnectorHint.ConnectorLocation.Bottom)]
	[HideInInspector]
	public UTAutomationPlanEntry nextEntry;

	public override UTAutomationPlanEntry NextEntry {
		get {
			return nextEntry;
		}
	}
	
	public override object Me {
		get {
			return null;
		}
	}
}

