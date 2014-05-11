using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Unparents all children.")]
[UBCategory("Transform")]
public class DetachChildren : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		_Transform.GetValue(context).DetachChildren();
	}

	public override string ToString(){
        return string.Format("Detach {0}'s Children", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.DetachChildren()");
	}

}