using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Transforms a direction from world space to local space. The opposite of .")]
[UBCategory("Transform")]
public class InverseTransformDirectionByDirection : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBVector3 _Direction = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).InverseTransformDirection(_Direction.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s InverseTransformDirection w/ {1}", _Transform.ToString(RootContainer), _Direction.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.InverseTransformDirection(#_Direction#)");
	}

}