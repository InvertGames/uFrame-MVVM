using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeDownLeftTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeDownLeftTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.DownLeft)
        {
            ExecuteSheet();
        }
    }
}