using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("GameObject")]
public class DestroyGameObject : UBAction {

	public UBGameObject _Obj = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		UnityEngine.Object.Destroy(_Obj.GetValue(context).gameObject);
	}

	public override void WriteCode(IUBCSharpGenerator sb){
	sb.AppendExpression("Object.Destroy(#_Obj#)");
	}

}