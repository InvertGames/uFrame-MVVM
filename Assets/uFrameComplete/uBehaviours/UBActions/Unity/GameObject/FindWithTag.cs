using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns one active GameObject tagged tag. Returns null if no GameObject was found.")]
[UBCategory("GameObject")]
public class FindWithTag : UBAction {

	
	[UBRequired] public UBString _Tag = new UBString();
	[UBRequireVariable] [UBRequired] public UBGameObject _Result = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GameObject.FindWithTag(_Tag.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Find Object With {0} Tag", _Tag.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GameObject.FindWithTag(#_Tag#)");
	}

}