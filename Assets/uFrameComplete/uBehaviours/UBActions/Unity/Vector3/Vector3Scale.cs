using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Vector3")]
public class Vector3Scale : UBAction
{

	public UBVector3 _A = new UBVector3();
	public UBVector3 _B = new UBVector3();
	public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue( context, Vector3.Scale(_A.GetValue(context),_B.GetValue(context)) );
		}

	}

}