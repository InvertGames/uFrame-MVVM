using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The scale of the transform relative to the parent.")]
[UBCategory("Transform")]
public class SetTransformLocalScale : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequired] public UBVector3 _Value = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Transform.GetValue(context).localScale = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s localScale to {1}", _Transform.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.localScale = #_Value# ");
	}

}