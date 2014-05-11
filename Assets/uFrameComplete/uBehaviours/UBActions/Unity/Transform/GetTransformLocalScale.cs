using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The scale of the transform relative to the parent.")]
[UBCategory("Transform")]
public class GetTransformLocalScale : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).localScale);
		}

	}

	public override string ToString(){
	return string.Format("Get localScale from {0}", _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Transform#.localScale");
	}

}