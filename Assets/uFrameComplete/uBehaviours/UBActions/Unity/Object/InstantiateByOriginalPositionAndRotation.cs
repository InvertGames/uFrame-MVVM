using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Clones the object original and returns the clone.")]
[UBCategory("Object")]
public class InstantiateByOriginalPositionAndRotation : UBAction {

	
	[UBRequired] public UBObject _Original = new UBObject();
	
	[UBRequired] public UBVector3 _Position = new UBVector3();

    [UBRequired]
    public UBVector3 _Rotation = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Object.Instantiate(_Original.GetValue(context), _Position.GetValue(context), Quaternion.Euler(_Rotation.GetValue(context))));
		}

	}

	public override string ToString(){

        return string.Format("Instantiate {0}", _Original.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Object.Instantiate(#_Original#, #_Position#, #_Rotation#)");
	}

}