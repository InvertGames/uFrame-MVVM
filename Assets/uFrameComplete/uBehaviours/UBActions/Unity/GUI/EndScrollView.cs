using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Ends a scrollview started with a call to BeginScrollView.")]
[UBCategory("GUI")]
public class EndScrollView : UBAction {

	protected override void PerformExecute(IUBContext context){
		GUI.EndScrollView();
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.EndScrollView()");
	}

}