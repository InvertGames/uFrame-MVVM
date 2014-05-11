using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation of the transform relative to the parent transform's rotation.")]
[UBCategory("Transform")]
public class SetTransformLocalRotation : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequired] public UBQuaternion _Value = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Transform.GetValue(context).localRotation = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s localRotation to {1}", _Transform.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.localRotation = #_Value# ");
	}

}