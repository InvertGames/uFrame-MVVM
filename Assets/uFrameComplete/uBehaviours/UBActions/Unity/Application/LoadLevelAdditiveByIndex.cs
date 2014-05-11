using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Loads a level additively.")]
[UBCategory("Application")]
public class LoadLevelAdditiveByIndex : UBAction {

	
	[UBRequired] public UBInt _Index = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Application.LoadLevelAdditive(_Index.GetValue(context));
	}

	public override string ToString(){
	return string.Format("LoadLevel {1} Additively", _Index.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.LoadLevelAdditive(#_Index#)");
	}

}