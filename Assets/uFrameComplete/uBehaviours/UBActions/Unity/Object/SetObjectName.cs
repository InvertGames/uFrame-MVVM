using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The name of the object.")]
[UBCategory("Object")]
public class SetObjectName : UBAction {

	[UBRequired] public UBObject _Object = new UBObject();
	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Object.GetValue(context).name = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s name to {1}", _Object.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Object#.name = #_Value# ");
	}

}