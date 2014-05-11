using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Destroys the object obj immediately. It is strongly recommended to use Destroy instead.")]
[UBCategory("Object")]
public class DestroyImmediateByObj : UBAction {

	
	[UBRequired] public UBObject _Obj = new UBObject();
	protected override void PerformExecute(IUBContext context){
		UnityEngine.Object.DestroyImmediate(_Obj.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Destroy {0} Immediately", _Obj.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.DestroyImmediate(#_Obj#)");
	}

}