using System;
using System.Collections.Generic;

/// <summary>
/// The error class that returns when an error exists on a trigger.
/// </summary>
public class BehaviourError : IBehaviourNotification
{
    public string Message { get; set; }
    public UnityEngine.Object SourceObject { get; set; }
    public object Source { get; set; }
    public Action Remove { get; set; }
 
}

public class BehaviourErrorComparer: IEqualityComparer<BehaviourError>
{
    public bool Equals(BehaviourError x, BehaviourError y)
    {
        return x.Source == y.Source && x.Message == y.Message;
    }

    public int GetHashCode(BehaviourError obj)
    {
        return obj.GetHashCode();
    }
}
/// <summary>
/// A breakpoint notification. Used for knowing when a breakpoint is active.
/// </summary>
public class BehaviourBreakpoint : IBehaviourNotification
{
    public string Message { get; set; }

    public object Source { get; set; }
    public Action Remove { get; set; }
    public UnityEngine.Object SourceObject { get; set; }
}