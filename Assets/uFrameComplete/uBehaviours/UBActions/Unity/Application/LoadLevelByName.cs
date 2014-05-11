using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Loads the level.")]
[UBCategory("Application")]
public class LoadLevelByName : UBAction {

	
	[UBRequired] public UBString _Name = new UBString();
	protected override void PerformExecute(IUBContext context){
		Application.LoadLevel(_Name.GetValue(context));
	}

	public override string ToString(){
	    return string.Format("LoadLevel w/ {0}", _Name.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.LoadLevel(#_Name#)");
	}

}