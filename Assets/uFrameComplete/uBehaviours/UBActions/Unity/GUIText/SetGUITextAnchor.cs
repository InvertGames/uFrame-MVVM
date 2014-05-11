using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The anchor of the text.")]
[UBCategory("GUIText")]
public class SetGUITextAnchor : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequired] public UBEnum _Value = new UBEnum(typeof(TextAnchor));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GUIText.GetValueAs<GUIText>(context).anchor = ((TextAnchor)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s anchor to {1}", _GUIText.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GUIText#.anchor = #_Value# ");
	}

}