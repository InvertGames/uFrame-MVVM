using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Resources")]
public class UnloadAsset : UBAction {

	[UBRequired] public UBObject _AssetToUnload = new UBObject();
	protected override void PerformExecute(IUBContext context){
		Resources.UnloadAsset(_AssetToUnload.GetValue(context));
	}

	public override string ToString(){
        return string.Format("Unload {0} Asset ", _AssetToUnload.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Resources.UnloadAsset(#_AssetToUnload#)");
	}

}