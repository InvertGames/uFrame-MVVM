using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Projects a vector onto another vector.")]
[UBCategory("Vector3")]
public class Vector3Project : UBAction
{

	
	[UBRequired] public UBVector3 _Vector = new UBVector3();
	
	[UBRequired] public UBVector3 _OnNormal = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Project(_Vector.GetValue(context),_OnNormal.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Project {0} on {1}", _Vector.ToString(RootContainer), _OnNormal.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Project(#_Vector#, #_OnNormal#)");
	}

}