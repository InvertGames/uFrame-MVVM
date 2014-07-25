using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class UBActionContent
{
    private UBTriggerContent[] _subContents;
    private Dictionary<string, UBActionSheet> _subSheets;
    private TextAnchor _anchor = TextAnchor.MiddleLeft;

    public UBAction Action { get; set; }

    public TextAnchor Anchor
    {
        get { return _anchor; }
        set { _anchor = value; }
    }

    public GUIStyle BackgroundStyle { get; set; }

    public IUBehaviours Behaviour { get; set; }

    public string DisplayText { get; set; }

    public GUIStyle IndicatorStyle { get; set; }

    public Action OnMoveDown { get; set; }

    public Action<UBActionSheet> OnMoveForward { get; set; }

    public Action OnMoveUp { get; set; }

    public Action OnRemove { get; set; }

    public Action<UBActionContent> OnShowOptions { get; set; }

    public Action<UBAction> OnSubContents { get; set; }

    public GUIStyle OptionsStyle { get; set; }

    public UBActionSheet Sheet { get; set; }

    public Object UndoTarget
    {
        get { return Behaviour as UnityEngine.Object; }
    }

    public bool AllowDrag { get; set; }

    //public Dictionary<string, UBActionSheet> SubSheets
    //{
    //    get { return _subSheets ?? (_subSheets = Action.GetAvailableActionSheets(Behaviour)); }
    //    set { _subSheets = value; }
    //}
    public UBActionContent()
    {
    }

    public UBActionContent(UBActionSheet sheet, UBAction action)
    {
        Sheet = sheet;
        Action = action;
        Anchor = TextAnchor.MiddleLeft;
        BackgroundStyle = action.Expanded ? UBStyles.CommandBarOpenStyle : UBStyles.CommandBarClosedStyle;
        DisplayText = action.ToString();
        AllowDrag = true;
        if (action.Breakpoint)
        {
            IndicatorStyle = UBStyles.DebugBackgroundStyle;
            //OptionsStyle = UBStyles.BreakpointButtonStyle;
        }

        if (!action.Enabled)
        {
            IndicatorStyle = null;
            BackgroundStyle = UBStyles.ForwardBackgroundStyle;
        }

        if (action.IsCurrentlyActive)
        {
            IndicatorStyle = UBStyles.CurrentActionBackgroundStyle;
        }

        OptionsStyle = Action.Expanded ? UBStyles.FoldoutOpenButtonStyle : UBStyles.FoldoutCloseButtonStyle;
        //if (action != sheet.Actions.LastOrDefault())
        //{
        //    OnMoveDown = () =>
        //    {
        //        var index = sheet.Actions.IndexOf(action);
        //        sheet.Actions.RemoveAt(index);
        //        sheet.Actions.Insert(index + 1, action);
        //        Save();
        //    };
        //}
        //if (action != sheet.Actions.FirstOrDefault())
        //{
        //    OnMoveUp = () =>
        //    {
        //        var index = sheet.Actions.IndexOf(action);
        //        sheet.Actions.RemoveAt(index);
        //        sheet.Actions.Insert(index - 1, action);
        //        Save();
        //    };
        //}
        OnRemove = () => UBUtils.RemoveAction(Behaviour, sheet, Action);
    }

    public void Save()
    {
        Sheet.Save(Behaviour);
        Sheet.Load(Behaviour);
        EditorUtility.SetDirty(UndoTarget);
    }
}