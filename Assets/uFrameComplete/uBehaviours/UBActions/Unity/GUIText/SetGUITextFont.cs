using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The font used for the text.")]
[UBCategory("GUIText")]
public class SetGUITextFont : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequired] public UBObject _Value = new UBObject(typeof(Font));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GUIText.GetValueAs<GUIText>(context).font = _Value.GetValueAs<Font>(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s font to {1}", _GUIText.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GUIText#.font = #_Value# ");
	}

}