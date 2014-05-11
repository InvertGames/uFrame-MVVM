using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Makes this vector have a  of 1.")]
[UBCategory("Vector3")]
public class Vector3NormalizeByValue : UBAction
{

	
	[UBRequired] public UBVector3 _Value = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Normalize(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Normalize {0}", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Normalize(#_Value#)");
	}

}