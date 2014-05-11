using System;

public interface IBehaviourNotification
{
    string Message { get; set; }

    object Source { get; set; }

    Action Remove { get; set; }
    UnityEngine.Object SourceObject { get; set; }
}