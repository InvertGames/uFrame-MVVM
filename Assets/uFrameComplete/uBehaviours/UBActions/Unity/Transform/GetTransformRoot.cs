using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the topmost transform in the hierarchy.")]
[UBCategory("Transform")]
public class GetTransformRoot : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequireVariable] [UBRequired] public UBTransform _Result = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).root);
		}

	}

	public override string ToString(){
	return string.Format("Get root from {0}", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Transform#.root");
	}

}