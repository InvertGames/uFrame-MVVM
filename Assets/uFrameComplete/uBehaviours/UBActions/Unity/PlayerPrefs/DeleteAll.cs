using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("PlayerPrefs")]
public class DeleteAllPlayerPrefs : UBAction
{

	protected override void PerformExecute(IUBContext context){
		PlayerPrefs.DeleteAll();
	}

}