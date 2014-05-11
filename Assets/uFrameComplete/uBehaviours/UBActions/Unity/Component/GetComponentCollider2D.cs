using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Component")]
public class GetComponentCollider2D : UBAction {

	[UBRequired] public UBObject _Component = new UBObject(typeof(Component));
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Collider2D));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Component.GetValueAs<Component>(context).collider2D);
		}

	}

	public override string ToString(){
	return string.Format("Get collider2D from {0}", _Component.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Component#.collider2D");
	}

}