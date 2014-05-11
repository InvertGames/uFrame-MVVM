using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The parent of the transform.")]
[UBCategory("Transform")]
public class GetTransformParent : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequireVariable] [UBRequired] public UBTransform _Result = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).parent);
		}

	}

	public override string ToString(){
	return string.Format("Get parent from {0}", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Transform#.parent");
	}

}