using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Transform")]
public class GetChild : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBInt _Index = new UBInt();
	[UBRequireVariable] [UBRequired] public UBTransform _Result = new UBTransform();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Transform.GetValue(context).GetChild(_Index.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("GetChild {0} in {1}", _Index.ToString(RootContainer), _Transform.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.GetChild(#_Index#)");
	}

}