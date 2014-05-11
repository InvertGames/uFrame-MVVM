using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The color with which the screen will be cleared.")]
[UBCategory("Camera")]
public class SetCameraBackgroundColor : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBColor _Value = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).backgroundColor = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s backgroundColor to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.backgroundColor = #_Value# ");
	}

}