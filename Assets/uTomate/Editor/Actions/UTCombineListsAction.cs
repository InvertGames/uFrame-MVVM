// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[UTActionInfo(actionCategory="General", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Combine Lists", description="Combines two lists of objects into one.")]
[UTDefaultAction]
public class UTCombineListsAction : UTAction
{
	
	[UTDoc(description="The type of list operation to be performed.")]
	[UTInspectorHint(required=true, order=0)]
	public UTCombineListOperationType listOperationType;
	[UTDoc(description="The name of the property containing the first list.")]
	[UTInspectorHint(required=true, order=1)]
	public UTString firstListProperty;
	[UTDoc(description="The name of the property containing the second list.")]
	[UTInspectorHint(required=true, order=2)]
	public UTString secondListProperty;
	[UTDoc(description="The name of the property into which the final list should be written.")]
	[UTInspectorHint(required=true, order=3)]
	public UTString outputProperty;
	
	public override System.Collections.IEnumerator Execute (UTContext context)
	{
		var theFirstListProperty = firstListProperty.EvaluateIn (context);
		if (string.IsNullOrEmpty (theFirstListProperty)) {
			throw new UTFailBuildException ("You must specify the property holding the first list.", this);
		}
		
		var theSecondListProperty = secondListProperty.EvaluateIn (context);
		if (string.IsNullOrEmpty (theFirstListProperty)) {
			throw new UTFailBuildException ("You must specify the property holding the second list.", this);
		}

		var theOutputProperty = outputProperty.EvaluateIn (context);
		if (string.IsNullOrEmpty (theFirstListProperty)) {
			throw new UTFailBuildException ("You must specify the output property.", this);
		}
		
		var firstEnumerable = context [theFirstListProperty];
		if (!(firstEnumerable is IEnumerable)) { 
			if (firstEnumerable == null) {
				throw new UTFailBuildException ("Property '" + theFirstListProperty + "' has a null value. Cannot combine this.", this);
			}
			throw new UTFailBuildException ("Property '" + theFirstListProperty + "' is of type '" + firstEnumerable.GetType () + "'. Cannot combine this.", this);
		}
		
		var secondEnumerable = context [theSecondListProperty];
		if (!(secondEnumerable is IEnumerable)) { 
			if (secondEnumerable == null) {
				throw new UTFailBuildException ("Property '" + theSecondListProperty + "' has a null value. Cannot combine this.", this);
			}
			throw new UTFailBuildException ("Property '" + theSecondListProperty + "' is of type '" + secondEnumerable.GetType () + "'. Cannot combine this.", this);
		}
		
		var firstListAsSet = new HashSet<object> ();
		foreach (var obj in (IEnumerable)firstEnumerable) {
			firstListAsSet.Add (obj);
		}
		
		var secondListAsSet = new HashSet<object> ();
		foreach (var obj in (IEnumerable)secondEnumerable) {
			secondListAsSet.Add (obj);
		}
		
		var theListOperationType = listOperationType.EvaluateIn (context);
		switch (theListOperationType) {
		case UTCombineListOperation.Union:
			firstListAsSet.UnionWith (secondListAsSet);
			break;
		case UTCombineListOperation.Intersect:
			firstListAsSet.IntersectWith (secondListAsSet);
			break;
		case UTCombineListOperation.Subtract:
			firstListAsSet.ExceptWith (secondListAsSet);
			break;
		case UTCombineListOperation.ExclusiveOr:
			firstListAsSet.SymmetricExceptWith (secondListAsSet);
			break;
		}
		
		context [theOutputProperty] = firstListAsSet;
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/General/Combine Lists", false, 390)]
	public static void AddAction ()
	{
		Create<UTCombineListsAction> ();
	}

}

