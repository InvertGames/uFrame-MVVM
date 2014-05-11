using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Camera")]
public class SetupCurrent : UBAction {

	
	[UBRequired] public UBObject _Current = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		Camera.SetupCurrent(_Current.GetValueAs<Camera>(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s SetupCurrent w/ {1}", "Camera", _Current.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Camera.SetupCurrent(#_Cur#)");
	}

}