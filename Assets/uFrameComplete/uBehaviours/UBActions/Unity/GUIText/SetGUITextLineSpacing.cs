using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The line spacing multiplier.")]
[UBCategory("GUIText")]
public class SetGUITextLineSpacing : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GUIText.GetValueAs<GUIText>(context).lineSpacing = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s lineSpacing to {1}", _GUIText.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GUIText#.lineSpacing = #_Value# ");
	}

}