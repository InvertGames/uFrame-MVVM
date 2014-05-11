using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a horizontal scrollbar. Scrollbars are what you use to scroll through a document. Most likely, you want to use scrollViews instead.")]
[UBCategory("GUI")]
public class HorizontalScrollbarByPositionValueSizeLeftValueAndRightValue : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBFloat _Value = new UBFloat();
	
	[UBRequired] public UBFloat _Size = new UBFloat();
	
	[UBRequired] public UBFloat _LeftValue = new UBFloat();
	
	[UBRequired] public UBFloat _RightValue = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.HorizontalScrollbar(_Position.GetValue(context),_Value.GetValue(context),_Size.GetValue(context),_LeftValue.GetValue(context),_RightValue.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.HorizontalScrollbar(#_Position#, #_Value#, #_Size#, #_LeftValue#, #_RightValue#)");
	}

}