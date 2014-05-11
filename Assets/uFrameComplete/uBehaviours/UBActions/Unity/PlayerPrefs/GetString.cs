using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class GetPlayerPrefsString : UBAction
{

	public UBString _Key = new UBString();
	public UBString _DefaultValue = new UBString();
	public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
						_Result.SetValue( context, PlayerPrefs.GetString(_Key.GetValue(context),_DefaultValue.GetValue(context))			);
		}

	}

}