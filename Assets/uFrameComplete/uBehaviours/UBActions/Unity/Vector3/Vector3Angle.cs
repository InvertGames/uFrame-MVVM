using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the angle in degrees between from and to.")]
[UBCategory("Vector3")]
public class Vector3Angle : UBAction {

	
	[UBRequired] public UBVector3 _From = new UBVector3();
	
	[UBRequired] public UBVector3 _To = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Angle(_From.GetValue(context),_To.GetValue(context)));
		}
	}

	public override string ToString(){
	    return string.Format("Angle between {0} & {1}",  _From.ToString(RootContainer), _To.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Angle(#_From#, #_To#)");
	}

}