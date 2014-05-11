using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Captures a screenshot at path filename as a PNG file.")]
[UBCategory("Application")]
public class CaptureScreenshotByFilename : UBAction {

	
	[UBRequired] public UBString _Filename = new UBString();
	protected override void PerformExecute(IUBContext context){
		Application.CaptureScreenshot(_Filename.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s CaptureScreenshot w/ {1}", "Application", _Filename.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.CaptureScreenshot(#_Filename#)");
	}

}