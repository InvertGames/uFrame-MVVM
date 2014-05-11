using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The game object this component is attached to. A component is always attached to a game object.")]
[UBCategory("Component")]
public class GetComponentGameObject : UBAction {

	[UBRequired] public UBObject _Component = new UBObject(typeof(Component));
	[UBRequireVariable] [UBRequired] public UBGameObject _Result = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Component.GetValueAs<Component>(context).gameObject);
		}

	}

	public override string ToString(){
	return string.Format("Get gameObject from {0}", _Component.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Component#.gameObject");
	}

}