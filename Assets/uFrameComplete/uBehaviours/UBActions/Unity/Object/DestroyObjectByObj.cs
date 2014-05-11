using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Object")]
public class DestroyObjectByObj : UBAction {

	
	[UBRequired] public UBObject _Obj = new UBObject();
	protected override void PerformExecute(IUBContext context){
		UnityEngine.Object.DestroyObject(_Obj.GetValue(context));
	}

	public override string ToString(){
        return string.Format("Destroy {0}", _Obj.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.DestroyObject(#_Obj#)");
	}

}