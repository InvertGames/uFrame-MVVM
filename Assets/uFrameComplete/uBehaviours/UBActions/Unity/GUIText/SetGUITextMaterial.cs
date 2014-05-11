using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The  to use for rendering.")]
[UBCategory("GUIText")]
public class SetGUITextMaterial : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequired] public UBMaterial _Value = new UBMaterial();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GUIText.GetValueAs<GUIText>(context).material = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s material to {1}", _GUIText.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GUIText#.material = #_Value# ");
	}

}