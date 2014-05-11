using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Reflects the vector along the normal.")]
[UBCategory("Vector3")]
public class Vector3Reflect : UBAction
{

	
	[UBRequired] public UBVector3 _InDirection = new UBVector3();
	
	[UBRequired] public UBVector3 _InNormal = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Reflect(_InDirection.GetValue(context),_InNormal.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Reflect(#_InDirection#, #_InNormal#)");
	}

}