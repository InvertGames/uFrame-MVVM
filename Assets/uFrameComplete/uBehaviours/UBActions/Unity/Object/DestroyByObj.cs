using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Removes a gameobject, component or asset.")]
[UBCategory("Object")]
public class DestroyByObj : UBAction {

	
	[UBRequired] public UBObject _Obj = new UBObject();
	protected override void PerformExecute(IUBContext context){
		UnityEngine.Object.Destroy(_Obj.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Destroy {0}", _Obj.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.Destroy(#_Obj#)");
	}

}