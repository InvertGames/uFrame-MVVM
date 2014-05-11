using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeUpTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeUpTriggerVariables()
    {
       yield return new UBStaticVariableDeclare(){Name="Swipe",DefaultValue = Vector3.zero,ValueType = typeof(Vector3)};
    }
    public override void Update()   
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.Up)
        {
           ExecuteSheet();
        }
    }
}