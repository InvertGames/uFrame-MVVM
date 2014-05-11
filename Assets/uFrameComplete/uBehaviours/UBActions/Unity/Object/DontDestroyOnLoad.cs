using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Makes the object target not be destroyed automatically when loading a new scene.")]
[UBCategory("Object")]
public class DontDestroyOnLoad : UBAction {

	
	[UBRequired] public UBObject _Target = new UBObject();
	protected override void PerformExecute(IUBContext context){
        UnityEngine.Object.DontDestroyOnLoad(_Target.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s DontDestroyOnLoad w/ {1}", "Object", _Target.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.DontDestroyOnLoad(#_Target#)");
	}

}