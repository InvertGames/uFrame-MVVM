using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GUIText")]
public class GetGUITextFontStyle : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(FontStyle));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GUIText.GetValueAs<GUIText>(context).fontStyle);
		}

	}

	public override string ToString(){
	return string.Format("Get fontStyle from {0}", _GUIText.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_GUIText#.fontStyle");
	}

}