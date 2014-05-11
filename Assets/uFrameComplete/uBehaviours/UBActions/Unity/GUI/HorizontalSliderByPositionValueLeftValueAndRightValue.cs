using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"A horizontal slider the user can drag to change a value between a min and a max.")]
[UBCategory("GUI")]
public class HorizontalSliderByPositionValueLeftValueAndRightValue : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBFloat _Value = new UBFloat();
	
	[UBRequired] public UBFloat _LeftValue = new UBFloat();
	
	[UBRequired] public UBFloat _RightValue = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.HorizontalSlider(_Position.GetValue(context),_Value.GetValue(context),_LeftValue.GetValue(context),_RightValue.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.HorizontalSlider(#_Position#, #_Value#, #_LeftValue#, #_RightValue#)");
	}

}