using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeUpLeftTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeUpLeftTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.UpLeft)
        {
            ExecuteSheet();
        }
    }
}