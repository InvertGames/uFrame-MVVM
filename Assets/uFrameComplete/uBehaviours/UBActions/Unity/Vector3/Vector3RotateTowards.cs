using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Rotates a vector/from/ towards to.")]
[UBCategory("Vector3")]
public class Vector3RotateTowards : UBAction
{

	[UBRequired] public UBVector3 _Current = new UBVector3();
	[UBRequired] public UBVector3 _Target = new UBVector3();
	
	[UBRequired] public UBFloat _MaxRadiansDelta = new UBFloat();
	
	[UBRequired] public UBFloat _MaxMagnitudeDelta = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.RotateTowards(_Current.GetValue(context),_Target.GetValue(context),_MaxRadiansDelta.GetValue(context),_MaxMagnitudeDelta.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Rotate {0} Towards {1}",  _Current.ToString(RootContainer), _Target.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.RotateTowards(#_Current#, #_Target#, #_MaxRadiansDelta#, #_MaxMagnitudeDelta#)");
	}

}