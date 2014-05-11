using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeUpRightTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeUpRightTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.UpRight)
        {
            ExecuteSheet();
        }
    }
}