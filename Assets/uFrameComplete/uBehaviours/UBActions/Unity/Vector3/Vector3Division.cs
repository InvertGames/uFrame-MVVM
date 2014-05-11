using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Vector3")]
public class Vector3Division : UBAction {

	public UBVector3 _A = new UBVector3();
	public UBFloat _D = new UBFloat();
	public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			 _Result.SetValue( context, _A.GetValue(context) / _D.GetValue(context)			);
		}

	}

}