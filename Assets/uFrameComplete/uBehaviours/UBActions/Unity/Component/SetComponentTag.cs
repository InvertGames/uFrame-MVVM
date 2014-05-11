using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The tag of this game object.")]
[UBCategory("Component")]
public class SetComponentTag : UBAction {

	[UBRequired] public UBObject _Component = new UBObject(typeof(Component));
	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Component.GetValueAs<Component>(context).tag = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s tag to {1}", _Component.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Component#.tag = #_Value# ");
	}

}