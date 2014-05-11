using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Transforms position from world space to local space. The opposite of .")]
[UBCategory("Transform")]
public class InverseTransformPointByPosition : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).InverseTransformPoint(_Position.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s InverseTransformPoint w/ {1}", _Transform.ToString(RootContainer), _Position.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.InverseTransformPoint(#_Position#)");
	}

}