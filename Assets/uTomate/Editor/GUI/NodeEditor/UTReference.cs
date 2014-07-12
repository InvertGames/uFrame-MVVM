//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// A reference is the graphical representation of a connection between two automation plan entries.
/// </summary>
[Serializable]
public class UTReference {

	[SerializeField]
	private UTAutomationPlanEntry source;
	
	public UTAutomationPlanEntry Source {
		get { return source; }
		set { source = value; }
	}
	
	[SerializeField]
	private string sourceProperty;
	
	public string SourceProperty {
		get { return sourceProperty; }
		set { sourceProperty = value; }
	}
	
	[SerializeField]
	private UTAutomationPlanEntry target;
	
	public UTAutomationPlanEntry Target {
		get { return target; }
		set { target = value; }
	}
	
}

