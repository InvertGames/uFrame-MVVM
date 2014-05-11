using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class SavePlayerPrefs : UBAction
{

	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.Save()		;
	}

}