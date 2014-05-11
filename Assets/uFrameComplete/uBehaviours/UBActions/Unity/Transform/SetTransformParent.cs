using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The parent of the transform.")]
[UBCategory("Transform")]
public class SetTransformParent : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequired] public UBTransform _Value = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Transform.GetValue(context).parent = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s parent to {1}", _Transform.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.parent = #_Value# ");
	}

}