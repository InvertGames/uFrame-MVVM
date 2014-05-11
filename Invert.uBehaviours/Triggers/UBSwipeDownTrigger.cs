using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeDownTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeDownTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.Down)
        {
            ExecuteSheet();
        }
    }
}