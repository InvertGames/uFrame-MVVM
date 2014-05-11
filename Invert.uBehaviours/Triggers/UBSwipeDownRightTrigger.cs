using System.Collections.Generic;
using UnityEngine;

[UBCategory("Gestures")]
public class UBSwipeDownRightTrigger : UBSwipeTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBSwipeDownRightTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "Swipe", DefaultValue = Vector3.zero };
    }
    public override void Update()
    {
        DetectSwipe(this);
        if (SwipeDirection == Swipe.DownRight)
        {
            ExecuteSheet();
        }
    }
}