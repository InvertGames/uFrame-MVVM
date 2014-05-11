using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Opens the url in a browser.")]
[UBCategory("Application")]
public class OpenURL : UBAction {

	
	[UBRequired] public UBString _Url = new UBString();
	protected override void PerformExecute(IUBContext context){
		Application.OpenURL(_Url.GetValue(context));
	}

	public override string ToString(){
	return string.Format("OpenURL {0}", _Url.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.OpenURL(#_Url#)");
	}

}