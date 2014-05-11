using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a vertiical scrollbar. Scrollbars are what you use to scroll through a document. Most likely, you want to use scrollViews instead.")]
[UBCategory("GUI")]
public class VerticalScrollbarByPositionValueSizeTopValueAndBottomValue : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBFloat _Value = new UBFloat();
	
	[UBRequired] public UBFloat _Size = new UBFloat();
	
	[UBRequired] public UBFloat _TopValue = new UBFloat();
	
	[UBRequired] public UBFloat _BottomValue = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.VerticalScrollbar(_Position.GetValue(context),_Value.GetValue(context),_Size.GetValue(context),_TopValue.GetValue(context),_BottomValue.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.VerticalScrollbar(#_Position#, #_Value#, #_Size#, #_TopValue#, #_BottomValue#)");
	}

}