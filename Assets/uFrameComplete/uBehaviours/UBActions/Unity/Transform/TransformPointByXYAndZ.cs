using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Transforms position from local space to world space.")]
[UBCategory("Transform")]
public class TransformPointByXYAndZ : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBFloat _X = new UBFloat();
	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _Z = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).TransformPoint(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.TransformPoint(#_X#, #_Y#, #_Z#)");
	}

}