using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class GetPlayerPrefsInt : UBAction
{

	public UBString _Key = new UBString();
	public UBInt _DefaultValue = new UBInt();
	public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
						_Result.SetValue( context, PlayerPrefs.GetInt(_Key.GetValue(context),_DefaultValue.GetValue(context))			);
		}

	}

}