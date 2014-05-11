using System;

using UnityEngine;

public class UBTriggerContent
{
    private GUIStyle _backgroundStyle;
    private GUIStyle _indicatorStyle;
    private string _label;
    private GUIStyle _optionsStyle;
    private bool _showArrow;
    private TextAnchor _textAnchor;

    public GUIStyle BackgroundStyle
    {
        get
        {
            //if (UBEditor.MouseDown)
            //{
            //    return UBStyles.ToolbarStyle;
            //}
            return _backgroundStyle;
        }
        set { _backgroundStyle = value; }
    }
    public Action OnDoubleClicked { get; set; }
    public GUIStyle IndicatorStyle
    {
        get { return _indicatorStyle; }
        set { _indicatorStyle = value; }
    }

    public UBActionSheetInfo Info { get; set; }

    public string Label
    {
        get { return _label; }
        set { _label = value; }
    }

    public Action OnClicked { get; set; }

    public Action OnDragOver { get; set; }

    public Action OnShowOptions { get; set; }

    public GUIStyle OptionsStyle
    {
        get { return _optionsStyle; }
    }

    public UBActionSheet Sheet { get; set; }

    public bool ShowArrow
    {
        get { return _showArrow; }
    }

    public string SubLabel { get; set; }

    public TextAnchor TextAnchor
    {
        get { return _textAnchor; }
    }

    public UBTriggerContent(string label, UBActionSheet sheet)
    {
        _label = label;
        _backgroundStyle = UBStyles.CommandBarClosedStyle;
        Sheet = sheet;
        if (sheet != null)
        {
            if (sheet.IsForward)
            {
                _backgroundStyle = UBStyles.ForwardBackgroundStyle;
                //SubLabel = "Forward To: " + sheet.ForwardTo.DisplayName;
                _label += "->" + sheet.ForwardTo.DisplayName;
            }
        }
        _showArrow = true;
        _textAnchor = TextAnchor.MiddleRight;
        _indicatorStyle = GetTriggerIndicatorStyle(sheet);
        _optionsStyle = GetTriggerOptionsStyle(sheet);
    }

    public UBTriggerContent(string label, GUIStyle backgroundStyle, GUIStyle indicatorStyle = null, GUIStyle optionsStyle = null, Action onShowOptions = null, bool showArrow = true, TextAnchor textAnchor = TextAnchor.MiddleRight)
    {
        _label = label;
        _backgroundStyle = backgroundStyle;
        _indicatorStyle = indicatorStyle;
        _optionsStyle = optionsStyle;
        OnShowOptions = onShowOptions;
        _showArrow = showArrow;
        _textAnchor = textAnchor;
    }

    public static GUIStyle GetTriggerIndicatorStyle(UBActionSheet sheet)
    {
        return null;
    }

    public static GUIStyle GetTriggerOptionsStyle(UBActionSheet info)
    {
        if (info == null)
            return UBStyles.TriggerInActiveButtonStyle;
        if (info.IsForward)
            return UBStyles.TriggerForwardButtonStyle;
        if (info.IsActive)
            return UBStyles.TriggerActiveButtonStyle;

        return UBStyles.TriggerInActiveButtonStyle;
    }
}