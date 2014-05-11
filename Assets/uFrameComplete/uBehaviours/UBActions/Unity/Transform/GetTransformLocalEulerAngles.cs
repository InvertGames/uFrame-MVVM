using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation as Euler angles in degrees relative to the parent transform's rotation.")]
[UBCategory("Transform")]
public class GetTransformLocalEulerAngles : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).localEulerAngles);
		}

	}

	public override string ToString(){
	return string.Format("Get localEulerAngles from {0}", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Transform#.localEulerAngles");
	}

}