//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using UnityEngine;

[Serializable]
[UTDoc(title="For Each", description="Executes a set of actions for each item in the list of items.")]
public class UTAutomationPlanForEachEntry : UTAutomationPlanEntryBase
{
	[UTDoc(description="The sub-tree that should be executed.")]
	[HideInInspector]
	public UTAutomationPlanEntry startOfSubtree;
	
	[UTDoc(description="Name of the property which holds the list of items to iterate over.")]
	[UTInspectorHint(order=0, required=true)]
	public UTString itemsPropertyName;
	
	[UTDoc(description="Name of the property which holds the current item.")]
	[UTInspectorHint(order=1, required=true)]
	public UTString itemPropertyName;
	
	[UTDoc(description="Name of the property which holds the index of the current item in the list. Index is zero-based. Leave empty if you don't need this.")]
	[UTInspectorHint(order=2)]
	public UTString indexPropertyName;
	
	public override string Label {
		get {
			return "For Each";
		}
	}
	
	public override IEnumerator Execute (UTContext context)
	{
		if (startOfSubtree == null) {
			Debug.LogWarning("No subtree specified.");
			yield break;
		}
		
		var theItemsPropertyName = itemsPropertyName.EvaluateIn(context);
		if (string.IsNullOrEmpty(theItemsPropertyName)) {
			throw new UTFailBuildException("You need to specify the name of the property which holds the list of items.", this);
		}
		
		var items = context[theItemsPropertyName];
		
		var theItemPropertyName = itemPropertyName.EvaluateIn(context);
		if (string.IsNullOrEmpty(theItemsPropertyName)) {
			throw new UTFailBuildException("You need to specify the name of the property which holds the current item.", this);
		}

		var theIndexPropertyName = indexPropertyName.EvaluateIn(context);
		var indexPropertySet = !string.IsNullOrEmpty(theIndexPropertyName);
		
		var index = 0;
		if ( items is IEnumerable ) {
			foreach(var item in (IEnumerable)items) {
				context[theItemPropertyName] = item;
				if (indexPropertySet) {
					context[theIndexPropertyName] = index;
				}
				var enumerator = UTAutomationPlan.ExecutePath(startOfSubtree, context);
				do {
					yield return "";
				}
				while (enumerator.MoveNext());
				index++;
			}
		}
	}
	
}

