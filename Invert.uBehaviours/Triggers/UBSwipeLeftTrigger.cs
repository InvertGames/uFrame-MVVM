using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeLeftTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeLeftTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.Left)
        {
            ExecuteSheet();
        }
    }
}