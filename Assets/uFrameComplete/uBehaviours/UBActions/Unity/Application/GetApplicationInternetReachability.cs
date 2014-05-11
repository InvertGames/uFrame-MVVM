using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationInternetReachability : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(NetworkReachability));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.internetReachability);
		}

	}

	public override string ToString(){
	return string.Format("Get internetReachability from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.internetReachability");
	}

}