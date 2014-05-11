using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"How far has the download progressed? [0...1]")]
[UBCategory("Application")]
public class GetStreamProgressForLevelByLevelIndex : UBAction {

	
	[UBRequired] public UBInt _LevelIndex = new UBInt();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.GetStreamProgressForLevel(_LevelIndex.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s GetStreamProgressForLevel w/ {1}", "Application", _LevelIndex.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.GetStreamProgressForLevel(#_LevelIndex#)");
	}

}