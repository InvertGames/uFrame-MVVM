using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The global skin to use.")]
[UBCategory("GUI")]
public class SetGUISkin : UBAction {

	[UBRequired] public UBObject _Value = new UBObject(typeof(GUISkin));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.skin = _Value.GetValueAs<GUISkin>(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s skin to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.skin = #_Value# ");
	}

}