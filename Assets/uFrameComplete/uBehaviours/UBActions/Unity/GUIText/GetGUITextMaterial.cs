using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The  to use for rendering.")]
[UBCategory("GUIText")]
public class GetGUITextMaterial : UBAction {

	[UBRequired] public UBObject _GUIText = new UBObject(typeof(GUIText));
	[UBRequireVariable] [UBRequired] public UBMaterial _Result = new UBMaterial();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GUIText.GetValueAs<GUIText>(context).material);
		}

	}

	public override string ToString(){
	return string.Format("Get material from {0}", _GUIText.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_GUIText#.material");
	}

}