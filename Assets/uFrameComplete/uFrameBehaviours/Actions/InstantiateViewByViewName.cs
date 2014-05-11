using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("ViewContainer")]
public class InstantiateViewByViewName : UBAction {

	public UBObject _ViewContainer = new UBObject(typeof(ViewContainer));
	public UBString _ViewName = new UBString();
	[UBRequireVariable] public UBObject _Result = new UBObject(typeof(ViewBase));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _ViewContainer.GetValueAs<ViewContainer>(context).InstantiateView(_ViewName.GetValue(context)));
		}

	}

}