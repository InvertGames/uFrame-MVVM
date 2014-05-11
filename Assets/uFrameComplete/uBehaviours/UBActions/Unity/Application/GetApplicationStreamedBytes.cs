using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"How many bytes have we downloaded from the main unity web stream (Read Only).")]
[UBCategory("Application")]
public class GetApplicationStreamedBytes : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.streamedBytes);
		}

	}

	public override string ToString(){
	return string.Format("Get streamedBytes from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.streamedBytes");
	}

}