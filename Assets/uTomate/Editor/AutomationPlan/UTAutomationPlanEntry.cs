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
/// UT automation plan entry base class. Must derive from ScriptableObject, see http://forum.unity3d.com/threads/12849-Unhappy-Derived-Types!
/// All automation plan entries derive from this class.
/// </summary>
public abstract class UTAutomationPlanEntry : ScriptableObject
{
	
	/// <summary>
	/// The GUID of this entry. Used for persisting things.
	/// </summary>
	[HideInInspector]
	public string automationPlanEntryId;
	
	/// <summary>
	/// The label of the automation plan entry. This label is displayed in the node editor.
	/// </summary>
	public abstract string Label { get; }
	
	/// <summary>
	/// The text of the automation plan entry. Depending on the kind of entry this text might or 
	/// might not be displayed in the node editor.
	/// </summary>
	public virtual string Text { 
		get {
			return "";
		}
	}
	
	/// <summary>
	/// Executes this entry in the given context.
	/// </summary>
	/// <param name='context'>
	/// The context in which the entry is to be executed.
	/// </param>
	public abstract IEnumerator Execute (UTContext context);
	
	/// <summary>
	/// Gets the next entry to be executed. If this is the last entry, this will return null.
	/// </summary>
	public abstract UTAutomationPlanEntry NextEntry { get; }
	
	/// <summary>
	/// Gets the value that should be used for the $me variable.
	/// </summary>
	public abstract object Me{ get; }
}

