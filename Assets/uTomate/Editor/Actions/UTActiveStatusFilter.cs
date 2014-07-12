using System;
using UnityEngine;

public class UTActiveStatusFilter : UTFilter
{
	
	private bool activeStatus;
	
	public UTActiveStatusFilter (bool activeStatus)
	{
		this.activeStatus = activeStatus;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
#if UNITY_3_5
		if (go.active == activeStatus) {
			return true;
		}
#else
		if (go.activeSelf == activeStatus) {
			return true;
		}
#endif		
		return false;
	}
}

