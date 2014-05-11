using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"A vertical slider the user can drag to change a value between a min and a max.")]
[UBCategory("GUI")]
public class VerticalSliderByPositionValueTopValueAndBottomValue : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBFloat _Value = new UBFloat();
	
	[UBRequired] public UBFloat _TopValue = new UBFloat();
	
	[UBRequired] public UBFloat _BottomValue = new UBFloat();

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.VerticalSlider(_Position.GetValue(context),_Value.GetValue(context),_TopValue.GetValue(context),_BottomValue.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.VerticalSlider(#_Position#, #_Value#, #_TopValue#, #_BottomValue#)");
	}

}