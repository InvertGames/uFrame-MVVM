using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationStreamingAssetsPath : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.streamingAssetsPath);
		}

	}

	public override string ToString(){
	return string.Format("Get streamingAssetsPath from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.streamingAssetsPath");
	}

}