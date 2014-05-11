using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2Lerp : UBAction
{

	[UBRequired] public UBVector2 _From = new UBVector2();
	[UBRequired] public UBVector2 _To = new UBVector2();
	[UBRequired] public UBFloat _T = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.Lerp(_From.GetValue(context),_To.GetValue(context),_T.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Lerp {0}",  _From.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.Lerp(#_From#, #_To#, #_T#)");
	}

}