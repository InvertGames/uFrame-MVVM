using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Moves the transform in the direction and distance of translation.")]
[UBCategory("Transform")]
public class TranslateByTranslation : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBVector3 _Translation = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Transform.GetValue(context).Translate(_Translation.GetValue(context));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.Translate(#_Translation#)");
	}

}