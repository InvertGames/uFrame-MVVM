// (C) 2013 Ancient Light Studios. All rights reserved.
//
// Copyright (c) 2013 Ancient Light Studios 
// All Rights Reserved 
//  
// http://www.ancientlightstudios.com
//
using System;
using UnityEngine;

public class UTAutomationPlanListItemRenderer :  CUItemRenderer<UTAutomationPlan>
{
 
	public float defaultHeight = 20f;
	
	public override float MeasureHeight (UTAutomationPlan item)
	{
		return defaultHeight;
	}
	
	public override void Arrange (UTAutomationPlan item, int itemIndex, bool selected, bool focused, Rect itemRect)
	{
		GUIStyle backgroundStyle = itemIndex % 2 == 1 ? ListStyle.oddBackground : ListStyle.evenBackground;
		backgroundStyle.Draw (itemRect, false, false, selected, focused);					
		ListStyle.item.Draw (itemRect, item.name, true, false, selected, false);
	}
	

}


