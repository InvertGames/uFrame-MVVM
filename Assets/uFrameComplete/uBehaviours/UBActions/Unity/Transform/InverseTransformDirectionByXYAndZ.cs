using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Transforms a direction from world space to local space. The opposite of .")]
[UBCategory("Transform")]
public class InverseTransformDirectionByXYAndZ : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBFloat _X = new UBFloat();
	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _Z = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).InverseTransformDirection(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s InverseTransformDirection w/ {1}, {2} and {3}", _Transform.ToString(RootContainer), _X.ToString(RootContainer), _Y.ToString(RootContainer), _Z.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.InverseTransformDirection(#_X#, #_Y#, #_Z#)");
	}

}