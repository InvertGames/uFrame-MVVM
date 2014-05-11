using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Destroy the object associated with this view ID across the network.")]
[UBCategory("Network")]
public class DestroyByGameObject : UBAction {

	
	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		Network.Destroy(_GameObject.GetValue(context));
	}

	public override string ToString(){
	return string.Format("NetworkDestroy {0}", _GameObject.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.Destroy(#_GameObject#)");
	}

}