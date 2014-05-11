using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("GameObject")]
public class FindGameObjectWithTag : UBAction {

	
	[UBRequired] public UBString _Tag = new UBString();
	[UBRequireVariable] [UBRequired] public UBGameObject _Result = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GameObject.FindGameObjectWithTag(_Tag.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Find GameObject with {0} Tag", _Tag.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GameObject.FindGameObjectWithTag(#_Tag#)");
	}

}