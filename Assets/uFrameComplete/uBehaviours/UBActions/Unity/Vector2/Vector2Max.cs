using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2Max : UBAction
{

	[UBRequired] public UBVector2 _Lhs = new UBVector2();
	[UBRequired] public UBVector2 _Rhs = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.Max(_Lhs.GetValue(context),_Rhs.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Max between {0} & {1}", _Lhs.ToString(RootContainer), _Rhs.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.Max(#_Lhs#, #_Rhs#)");
	}

}