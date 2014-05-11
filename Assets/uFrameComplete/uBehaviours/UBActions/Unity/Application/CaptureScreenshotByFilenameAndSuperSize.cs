using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class CaptureScreenshotByFilenameAndSuperSize : UBAction {

	[UBRequired] public UBString _Filename = new UBString();
	[UBRequired] public UBInt _SuperSize = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Application.CaptureScreenshot(_Filename.GetValue(context),_SuperSize.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s CaptureScreenshot w/ {1} and {2}", "Application", _Filename.ToString(RootContainer), _SuperSize.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.CaptureScreenshot(#_Filename#, #_SuperSize#)");
	}

}