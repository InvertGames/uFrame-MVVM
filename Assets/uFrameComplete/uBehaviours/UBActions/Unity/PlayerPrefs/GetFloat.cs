using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class GetPlayerPrefsFloat : UBAction
{

	public UBString _Key = new UBString();
	public UBFloat _DefaultValue = new UBFloat();
	public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
						_Result.SetValue( context, PlayerPrefs.GetFloat(_Key.GetValue(context),_DefaultValue.GetValue(context))			);
		}
	}

}