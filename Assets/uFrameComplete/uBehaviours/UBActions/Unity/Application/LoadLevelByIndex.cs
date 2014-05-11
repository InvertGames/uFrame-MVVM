using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Loads the level.")]
[UBCategory("Application")]
public class LoadLevelByIndex : UBAction {

	
	[UBRequired] public UBInt _Index = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Application.LoadLevel(_Index.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Load level at index {0}", _Index.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.LoadLevel(#_Index#)");
	}

}