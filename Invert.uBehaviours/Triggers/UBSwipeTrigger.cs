using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



[UBSetting("MinSwipeLength", typeof(float), 0.5f)]
[UBSetting("MouseSimulation", typeof(bool), true)]
public class UBSwipeTrigger : UBTrigger
{
    public enum Swipe { Up, Down, Left, Right, None, UpLeft, UpRight, DownLeft, DownRight };
    public float _MinSwipeLength = 200f;
    public bool _MouseSimulation = true;

    static Vector2 _firstPressPos;
    static Vector2 _secondPressPos;
    private static Vector2 _currentSwipe;
    private static float _lastDetect;
    private const float _TweakFactor = 0.5f;

    public static Swipe SwipeDirection { get; set; }

    public static Vector2 CurrentSwipe
    {
        get { return _currentSwipe; }
        set { _currentSwipe = value; }
    }

    public override void ExecuteSheet()
    {
        Instance.SetVariable("Swipe","CurrentSwipe");
        base.ExecuteSheet();
        //ExecuteSheetWithVars(new UBVector3(CurrentSwipe) { Name = "Swipe" });
    }

    public override void Initialize(TriggerInfo trigger, Dictionary<string, object> settings)
    {
        base.Initialize(trigger, settings);
        _MinSwipeLength = (float)settings["MinSwipeLength"];
        _MouseSimulation = (bool)settings["MouseSimulation"];
    }

    public virtual void Update()
    {
        DetectSwipe(this);
    }

    public static void DetectSwipe(UBSwipeTrigger instance)
    {
        if (_lastDetect == Time.time) return;
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {

                _firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                _secondPressPos = new Vector2(t.position.x, t.position.y);
                if (DetermineSwipe(instance)) return;
            }
        }
        else if (instance._MouseSimulation && Input.GetMouseButtonDown(0))
        {
            _firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (instance._MouseSimulation && Input.GetMouseButtonUp(0))
        {
            _secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (DetermineSwipe(instance)) return;
        }
        else
        {
            SwipeDirection = Swipe.None;
            //debugInfo.text = "No swipe"; // if you display this, you will lose the debug text when you stop swiping
        }
        _lastDetect = Time.time;
    }

    private static bool DetermineSwipe(UBSwipeTrigger instance)
    {
        CurrentSwipe = new Vector3(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);

        // Make sure it was a legit swipe, not a tap
        if (CurrentSwipe.magnitude < instance._MinSwipeLength)
        {
            SwipeDirection = Swipe.None;
            return true;
        }

        CurrentSwipe.Normalize();

        // Swipe up
        if (CurrentSwipe.y > 0 && CurrentSwipe.x > 0 - _TweakFactor && CurrentSwipe.x < _TweakFactor)
        {
            SwipeDirection = Swipe.Up;
        }
        else if (CurrentSwipe.y < 0 && CurrentSwipe.x > 0 - _TweakFactor && CurrentSwipe.x < _TweakFactor)
        {
            SwipeDirection = Swipe.Down;
        }
        else if (CurrentSwipe.x < 0 && CurrentSwipe.y > 0 - _TweakFactor && CurrentSwipe.y < _TweakFactor)
        {
            SwipeDirection = Swipe.Left;
        }
        else if (CurrentSwipe.x > 0 && CurrentSwipe.y > 0 - _TweakFactor && CurrentSwipe.y < _TweakFactor)
        {
            SwipeDirection = Swipe.Right;
        }
        else if (CurrentSwipe.y > 0 && CurrentSwipe.x < 0)
        {
            SwipeDirection = Swipe.UpLeft;
        }
        else if (CurrentSwipe.y > 0 && CurrentSwipe.x > 0)
        {
            SwipeDirection = Swipe.UpRight;
        }
        else if (CurrentSwipe.y < 0 && CurrentSwipe.x < 0)
        {
            SwipeDirection = Swipe.DownLeft;
        }
        else if (CurrentSwipe.y < 0 && CurrentSwipe.x > 0)
        {
            SwipeDirection = Swipe.DownRight;
        }
        return false;
    }
}