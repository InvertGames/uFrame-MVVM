using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Multiplies two vectors component-wise.")]
[UBCategory("Vector2")]
public class Vector2ScaleByAAndB : UBAction
{

	
	[UBRequired] public UBVector2 _A = new UBVector2();
	
	[UBRequired] public UBVector2 _B = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.Scale(_A.GetValue(context),_B.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.Scale(#_A#, #_B#)");
	}

}