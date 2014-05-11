using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Loads a level additively.")]
[UBCategory("Application")]
public class LoadLevelAdditiveByName : UBAction {

	
	[UBRequired] public UBString _Name = new UBString();
	protected override void PerformExecute(IUBContext context){
		Application.LoadLevelAdditive(_Name.GetValue(context));
	}

	public override string ToString(){
	return string.Format("LoadLevel {0} Additively", _Name.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.LoadLevelAdditive(#_Name#)");
	}

}