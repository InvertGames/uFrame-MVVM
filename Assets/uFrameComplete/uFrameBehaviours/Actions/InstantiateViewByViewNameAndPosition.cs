using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("ViewContainer")]
public class InstantiateViewByViewNameAndPosition : UBAction {

	public UBObject _ViewContainer = new UBObject(typeof(ViewContainer));
	public UBString _ViewName = new UBString();
	public UBVector3 _Position = new UBVector3();
	[UBRequireVariable] public UBObject _Result = new UBObject(typeof(ViewBase));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _ViewContainer.GetValueAs<ViewContainer>(context).InstantiateView(_ViewName.GetValue(context),_Position.GetValue(context)));
		}

	}

}