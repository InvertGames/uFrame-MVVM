using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation as Euler angles in degrees relative to the parent transform's rotation.")]
[UBCategory("Transform")]
public class SetTransformLocalEulerAngles : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequired] public UBVector3 _Value = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Transform.GetValue(context).localEulerAngles = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s localEulerAngles to {1}", _Transform.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.localEulerAngles = #_Value# ");
	}

}