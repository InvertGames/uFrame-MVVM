using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the distance between a and b.")]
[UBCategory("Vector2")]
public class Vector2Distance : UBAction
{

	
	[UBRequired] public UBVector2 _A = new UBVector2();
	
	[UBRequired] public UBVector2 _B = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.Distance(_A.GetValue(context),_B.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Distance between {0} and {1}", _A.ToString(RootContainer), _B.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.Distance(#_A#, #_B#)");
	}

}