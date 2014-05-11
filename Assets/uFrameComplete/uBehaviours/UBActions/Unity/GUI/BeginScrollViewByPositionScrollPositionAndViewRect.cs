using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Begin a scrolling view inside your GUI.")]
[UBCategory("GUI")]
public class BeginScrollViewByPositionScrollPositionAndViewRect : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBVector2 _ScrollPosition = new UBVector2();
	
	[UBRequired] public UBRect _ViewRect = new UBRect();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.BeginScrollView(_Position.GetValue(context),_ScrollPosition.GetValue(context),_ViewRect.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BeginScrollView(#_Position#, #_ScrollPosition#, #_ViewRect#)");
	}

}