using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation of the transform relative to the parent transform's rotation.")]
[UBCategory("Transform")]
public class GetTransformLocalRotation : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequireVariable] [UBRequired] public UBQuaternion _Result = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).localRotation);
		}

	}

	public override string ToString(){
	return string.Format("Get localRotation from {0}", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Transform#.localRotation");
	}

}