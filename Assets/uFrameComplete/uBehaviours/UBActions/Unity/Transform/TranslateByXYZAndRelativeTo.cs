using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Moves the transform in the direction and distance of translation.")]
[UBCategory("Transform")]
public class TranslateByXYZAndRelativeTo : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBFloat _X = new UBFloat();
	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _Z = new UBFloat();
	
	[UBRequired] public UBTransform _RelativeTo = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		_Transform.GetValue(context).Translate(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context),_RelativeTo.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.Translate(#_X#, #_Y#, #_Z#, #_RelativeTo#)");
	}

}