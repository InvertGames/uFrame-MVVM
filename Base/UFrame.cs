using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The uFrame static factory class for overriding/customizing core uFrame functionality if needed
/// </summary>
public static class UFrame
{
    public static ViewResolver ViewManager { get; set; }

    static UFrame()
    {
        ViewManager = new ViewResolver();
    }
}