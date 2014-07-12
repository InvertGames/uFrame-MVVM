//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

[UTDoc(title="Set Property", description="This action sets a property in the context which can be read later by other actions.")]
[UTActionInfo(actionCategory="General")]
[UTDefaultAction]
public class UTSetPropertyAction : UTAction
{
	[UTDoc(title = "Name", description="The name of the property to be set.")]
	[UTInspectorHint(required=true, order=0)]
	public UTString propertyName;
	[UTDoc(title = "Value", description="The value of the property.")]
	[UTInspectorHint(order=1)]
	public UTString propertyValue;
	[UTDoc(description="When ticked, the property value will only be set if the property is not already set to a value.")]
	public UTBool onlyIfUnset;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theName = propertyName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theName)) {
			throw new UTFailBuildException ("The name of the property must not be empty.",this);
		}		
		
		object theRealValue = propertyValue.Value;
		if (propertyValue.UseExpression) {
			theRealValue = context.Evaluate (propertyValue.Expression);	
		}
		
		var doSetOnlyIfUnset = onlyIfUnset.EvaluateIn(context);
		
		if (!doSetOnlyIfUnset || !context.ContainsProperty(theName)) {
			if (UTPreferences.DebugMode) {
				Type valueType = theRealValue != null ? theRealValue.GetType() : null;
				Debug.Log ("Setting property '" + theName + "' to " + 
					(valueType != null ? "[" + valueType.Name + "] " : "") + theRealValue, this);
			}
			context [theName] = theRealValue;
		}
		else {
			if (doSetOnlyIfUnset) {
				if (UTPreferences.DebugMode) {
					Debug.Log("Not setting property '" + theName + "' because it is already set.");
				}
			}
		}
		yield return "";
	}
	
	
	[MenuItem("Assets/Create/uTomate/General/Set Property",  false, 360)]
	public static void AddAction ()
	{
		Create<UTSetPropertyAction> ();
	}

	
	
}

