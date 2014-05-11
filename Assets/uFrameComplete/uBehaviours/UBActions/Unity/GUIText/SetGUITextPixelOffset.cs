using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The pixel offset of the text.")]
[UBCategory("GUIText")]
public class SetGUITextPixelOffset : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequired] public UBVector2 _Value = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GUIText.GetValueAs<GUIText>(context).pixelOffset = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s pixelOffset to {1}", _GUIText.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GUIText#.pixelOffset = #_Value# ");
	}

}