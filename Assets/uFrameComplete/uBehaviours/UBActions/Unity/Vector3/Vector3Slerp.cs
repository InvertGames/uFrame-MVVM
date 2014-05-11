using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Spherically interpolates between two vectors.")]
[UBCategory("Vector3")]
public class Vector3Slerp : UBAction
{

	
	[UBRequired] public UBVector3 _From = new UBVector3();
	
	[UBRequired] public UBVector3 _To = new UBVector3();
	
	[UBRequired] public UBFloat _T = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Slerp(_From.GetValue(context),_To.GetValue(context),_T.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Slerp from {0} to {1}", _From.ToString(RootContainer), _To.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Slerp(#_From#, #_To#, #_T#)");
	}

}