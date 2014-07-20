using UnityEditor;
using UnityEngine;

public static class UBStyles
{
    private static GUIStyle _actionEnabledButtonStyle;
    private static GUIStyle _addButtonStyle;
    private static GUIStyle _arrowDownButtonStyle;
    private static Texture2D _arrowDownTexture;
    private static GUIStyle _arrowLeftButtonStyle;
    private static Texture2D _arrowLeftTexture;
    private static Texture2D _arrowRightTexture;
    private static GUIStyle _arrowUpButtonStyle;
    private static Texture2D _arrowUpTexture;
    private static GUIStyle _breakpointButtonStyle;
    private static GUIStyle _buttonStyle;
    private static GUIStyle _collapseArrowRight;
    private static Texture2D _collapseRightArrowTexture;
    private static GUIStyle _commandBarClosedStyle;
    private static GUIStyle _commandBarClosedStyleSmall;
    private static GUIStyle _commandBarOpenStyle;
    private static GUIStyle _commandBarOpenStyleSmall;
    private static GUIStyle _commandButtonStyle;
    private static GUIStyle _continueButtonStyle;
    private static GUIStyle _currentActionBackgroundStyle;
    private static GUIStyle _debugBackgroundStyle;
    private static GUIStyle _eventActiveButtonStyle;
    private static Texture2D _eventActiveTexture;
    private static GUIStyle _eventButtonLargeStyle;
    private static GUIStyle _eventButtonStyle;
    private static GUIStyle _eventForwardButtonStyle;
    private static GUIStyle _eventInActiveButtonStyle;
    private static Texture2D _eventInActiveTexture;
    private static GUIStyle _eventSmallButtonStyle;
    private static GUIStyle _foldoutButtonStyle;
    private static GUIStyle _forwardBackgroundStyle;
    private static GUIStyle _includeTriggerBackground;
    private static GUIStyle _instanceTriggerBackgroundStyle;
    private static GUIStyle _nextButtonStyle;
    private static GUIStyle _pasteButtonStyle;
    private static GUIStyle _removeButtonStyle;
    private static GUISkin _skin;

    private static GUIStyle _subHeaderStyle;
    private static GUIStyle _subLabelStyle;
    private static GUIStyle _toolbarStyle;

    private static GUIStyle _variablesButtonStyle;
    private static GUIStyle _seperatorStyle;
    private static GUIStyle _treeItemStyle;
    private static GUIStyle _treeItemStyleOdd;
    private static GUIStyle _treeviewButtonStyle;
    private static GUIStyle _listButtonStyle;

    public static string SkinName
    {
        get { return "images"; }
        //EditorGUIUtility.isProSkin ? "ProSkin" : "FreeSkin"; }
    }

    public static Texture2D GetSkinTexture(string name)
    {
        return Resources.Load<Texture2D>(string.Format("{0}/{1}", SkinName, name));
    }
    public static GUIStyle ActionEnabledButtonStyle
    {
        get
        {
            if (_actionEnabledButtonStyle == null)
                _actionEnabledButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("ActionEnabled"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _actionEnabledButtonStyle;
        }
    }
    public static GUIStyle SeperatorStyle
    {
        get
        {
            if (_seperatorStyle == null)
                _seperatorStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Seperator"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                   stretchHeight = true,
                    fixedWidth = 3f
                };
            return _seperatorStyle;
        }
    }
    public static GUIStyle AddButtonStyle
    {
        get
        {
            if (_addButtonStyle == null)
                _addButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Plus"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _addButtonStyle;
        }
    }

    public static GUIStyle ArrowDownButtonStyle
    {
        get
        {
            if (_arrowDownButtonStyle == null)
                _arrowDownButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("ArrowDown"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _arrowDownButtonStyle;
        }
    }

    public static Texture2D ArrowDownTexture
    {
        get
        {
            return _arrowDownTexture ?? (_arrowDownTexture = GetSkinTexture("ArrowDown"));
        }
    }

    public static GUIStyle ArrowLeftButtonStyle
    {

        get
        {
            if (_arrowLeftButtonStyle == null)
                _arrowLeftButtonStyle = new GUIStyle
                {
                    normal = { background = ArrowLeftTexture, textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };

            return _arrowLeftButtonStyle;
        }
    }

    public static Texture2D ArrowLeftTexture
    {
        get
        {
            return _arrowLeftTexture ?? (_arrowLeftTexture = GetSkinTexture("ArrowLeft"));
        }
    }

    public static Texture2D ArrowRightTexture
    {
        get
        {
            return _arrowRightTexture ?? (_arrowRightTexture = GetSkinTexture("ArrowRight"));
        }
    }

    public static GUIStyle ArrowUpButtonStyle
    {
        get
        {
            if (_arrowUpButtonStyle == null)
                _arrowUpButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("ArrowUp"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _arrowUpButtonStyle;
        }
    }

    public static Texture2D ArrowUpTexture
    {
        get
        {
            return _arrowUpTexture ?? (_arrowUpTexture = GetSkinTexture("ArrowUp"));
        }
    }

    public static GUIStyle BreakpointButtonStyle
    {
        get
        {
            if (_breakpointButtonStyle == null)
                _breakpointButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Breakpoint"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };

            return _breakpointButtonStyle;
        }
    }

    public static GUIStyle ButtonStyle
    {
        get
        {
            if (_buttonStyle == null)
            {
                var textColor = new Color(0.8f, 0.8f, 0.8f);
                _buttonStyle = new GUIStyle
                {
                    normal =
                    {
                        background = GetSkinTexture("CommandBar"),
                        textColor = textColor
                    },
                    focused =
                    {
                        background = GetSkinTexture("CommandBar"),
                        textColor = textColor
                    },
                    active = { background = GetSkinTexture("EventButton"), textColor = textColor },
                    hover = { background = GetSkinTexture("CommandBar"), textColor = textColor },
                    onHover =
                    {
                        background = GetSkinTexture("CommandBar"),
                        textColor = textColor
                    },
                    onFocused = { background = GetSkinTexture("EventButton"), textColor = textColor },
                    onNormal = { background = GetSkinTexture("EventButton"), textColor = textColor },
                    onActive = { background = GetSkinTexture("EventButton"), textColor = textColor },
                    fixedHeight = 25,
                    alignment = TextAnchor.MiddleCenter,
                };
            }
            return _buttonStyle;
        }
    }

    public static GUIStyle CollapseArrowRightStyle
    {
        get
        {
            if (_collapseArrowRight == null)
                _collapseArrowRight = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CollapseRightArrow"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _collapseArrowRight;
        }
    }

    public static Texture2D CollapseRightArrowTexture
    {
        get
        {
            return _collapseRightArrowTexture ?? (_collapseRightArrowTexture = GetSkinTexture("CollapseRightArrow"));
        }
    }
    public static GUIStyle TreeItemStyle
    {
        get
        {
            if (_treeItemStyle == null)
                _treeItemStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Toolbar"), textColor = Color.white },
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(1,1,1,1),
                    
                    fixedHeight = 20f
                };
            return _treeItemStyle;
        }
    }
    public static GUIStyle TreeItemStyleOdd
    {
        get
        {
            if (_treeItemStyleOdd == null)
                _treeItemStyleOdd = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandBar"), textColor = Color.white },
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(1,1,1,1),
                   
                    fixedHeight = 18f
                };
            return _treeItemStyleOdd;
        }
    }
    public static GUIStyle CommandBarButtonStyle
    {
        get
        {
            if (_commandButtonStyle == null)
                _commandButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandExpanded"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.grey },
                    fontStyle = FontStyle.Bold,
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 30f
                };
            return _commandButtonStyle;
        }
    }

    public static GUIStyle CommandBarClosedStyle
    {
        get
        {
            if (_commandBarClosedStyle == null)
                _commandBarClosedStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandBar"), textColor = Color.white },
                    padding = new RectOffset(7, 2, 4, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 22f,

                    stretchHeight = true,
                };
            return _commandBarClosedStyle;
        }
    }

    public static GUIStyle CommandBarClosedStyleSmall
    {
        get
        {
            if (_commandBarClosedStyleSmall == null)
                _commandBarClosedStyleSmall = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandBar"), textColor = Color.white },
                    stretchHeight = true,
                    //  padding = new RectOffset(2, 2, 2, 2),
                    // margin = new RectOffset(0, 0, 0, 0),

                    //fixedHeight = 25f,
                };
            return _commandBarClosedStyleSmall;
        }
    }

    public static GUIStyle CommandBarOpenStyle
    {
        get
        {
            if (_commandBarOpenStyle == null)
                _commandBarOpenStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandExpanded"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.grey },
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 30f,
                    alignment = TextAnchor.MiddleCenter
                };
            return _commandBarOpenStyle;
        }
    }

    public static GUIStyle CommandBarOpenStyleSmall
    {
        get
        {
            if (_commandBarOpenStyleSmall == null)
                _commandBarOpenStyleSmall = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CommandExpanded"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.grey },
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 25f,
                };
            return _commandBarOpenStyleSmall;
        }
    }

    public static GUIStyle ContinueButtonStyle
    {
        get
        {
            if (_continueButtonStyle == null)
                _continueButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("ContinueButton"), textColor = Color.white },
                    fixedHeight = 25,
                    fixedWidth = 32,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _continueButtonStyle;
        }
    }

    public static GUIStyle CurrentActionBackgroundStyle
    {
        get
        {
            if (_currentActionBackgroundStyle == null)
                _currentActionBackgroundStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("CurrentActionBackground"), textColor = Color.white },

                    stretchHeight = true,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _currentActionBackgroundStyle;
        }
    }

    public static GUIStyle DebugBackgroundStyle
    {
        get
        {
            if (_debugBackgroundStyle == null)
                _debugBackgroundStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("DebugBackground"), textColor = new Color(0.95f, 0.95f, 0.95f) },
                    alignment = TextAnchor.MiddleCenter
                };

            return _debugBackgroundStyle;
        }
    }

    public static Texture2D EventActiveTexture
    {
        get
        {
            return _eventActiveTexture ?? (_eventActiveTexture = GetSkinTexture("EventActive"));
        }
    }

    public static GUIStyle EventButtonLargeStyle
    {
        get
        {
            var textColor = Color.white;
            if (_eventButtonLargeStyle == null)
                _eventButtonLargeStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("EventButton"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.grey },
                    fixedHeight = 33,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _eventButtonLargeStyle;
        }
    }

    public static GUIStyle EventButtonStyle
    {
        get
        {
            if (_eventButtonStyle == null)
                _eventButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("EventButton"), textColor = EditorGUIUtility.isProSkin ? Color.white : Color.grey },
                    active = { background = CommandBarClosedStyle.normal.background },
                    fixedHeight = 22,
                    stretchHeight = true,

                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _eventButtonStyle;
        }
    }

    public static Texture2D EventInActiveTexture
    {
        get
        {
            return _eventInActiveTexture ?? (_eventInActiveTexture = GetSkinTexture("EventInActive"));
        }
    }

    public static GUIStyle EventSmallButtonStyle
    {
        get
        {
            if (_eventSmallButtonStyle == null)
                _eventSmallButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("EventButton"), textColor = Color.white },
                    active = { background = CommandBarClosedStyle.normal.background },
                    fixedHeight = 20,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _eventSmallButtonStyle;
        }
    }

    public static GUIStyle FoldoutCloseButtonStyle
    {
        get
        {
            if (_foldoutButtonStyle == null)
                _foldoutButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    focused = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    active = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    hover = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    onHover = { background = GetSkinTexture("FoldoutClose"), textColor = Color.white },
                    onFocused = { background = GetSkinTexture("FoldoutClose"), textColor = Color.white },
                    onNormal = { background = GetSkinTexture("FoldoutClose"), textColor = Color.white },
                    onActive = { background = GetSkinTexture("FoldoutClose"), textColor = Color.white },
                    fixedHeight = 16,
                    fixedWidth = 16,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _foldoutButtonStyle;
        }
    }

    public static GUIStyle FoldoutOpenButtonStyle
    {
        get
        {
            if (_foldoutButtonStyle == null)
                _foldoutButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    focused = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    active = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    hover = { background = GetSkinTexture("FoldoutOpen"), textColor = Color.white },
                    fixedHeight = 16,
                    fixedWidth = 16,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _foldoutButtonStyle;
        }
    }

    public static GUIStyle ForwardBackgroundStyle
    {
        get
        {
            if (_forwardBackgroundStyle == null)
                _forwardBackgroundStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("ForwardBackground"), textColor = Color.white },
                    fixedHeight = 22,
                    stretchHeight = true,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _forwardBackgroundStyle;
        }
    }

    public static GUIStyle IncludeTriggerBackgroundStyle
    {
        get
        {
            if (_includeTriggerBackground == null)
                _includeTriggerBackground = new GUIStyle
                {
                    normal = { background = GetSkinTexture("IncludeTriggerBackground"), textColor = new Color(0.7f, 0.7f, 0.7f) },
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 22f,
                    fontSize = 10,
                    stretchHeight = true,
                };

            return _includeTriggerBackground;
        }
    }

    public static GUIStyle InstanceTriggerBackgroundStyle
    {
        get
        {
            if (_instanceTriggerBackgroundStyle == null)
                _instanceTriggerBackgroundStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("InstanceTriggerBackground"), textColor = Color.white },
                    //fixedHeight = 30,
                    stretchHeight = true,
                    margin = new RectOffset(0, 0, 10, 5)
                };

            return _instanceTriggerBackgroundStyle;
        }
    }

    public static GUIStyle NextButtonStyle
    {
        get
        {
            if (_nextButtonStyle == null)
                _nextButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("NextButton"), textColor = Color.white },
                    fixedHeight = 25,
                    fixedWidth = 32,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _nextButtonStyle;
        }
    }

    public static GUIStyle PasteButtonStyle
    {
        get
        {
            if (_pasteButtonStyle == null)
                _pasteButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("edit-paste"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _pasteButtonStyle;
        }
    }
    public static GUIStyle TreeviewButtonStyle
    {
        get
        {
            if (_treeviewButtonStyle == null)
                _treeviewButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("treeview"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _treeviewButtonStyle;
        }
    }
    public static GUIStyle ListButtonStyle
    {
        get
        {
            if (_listButtonStyle == null)
                _listButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("list"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _listButtonStyle;
        }
    }

    public static GUIStyle RemoveButtonStyle
    {
        get
        {
            if (_removeButtonStyle == null)
                _removeButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Remove"), textColor = Color.white },

                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _removeButtonStyle;
        }
    }

    public static GUISkin Skin
    {
        get
        {
            if (ReferenceEquals(_skin, null))
            {
                _skin = Resources.Load<GUISkin>("ActionSheetSkin");
                _skin.hideFlags = HideFlags.HideAndDontSave;
            }

            return _skin;
        }
        set { _skin = value; }
    }
  
    public static GUIStyle SubHeaderStyle
    {
        get
        {
            if (_subHeaderStyle == null)
                _subHeaderStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("EventButton"), textColor = EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.2f,0.2f,0.2f) },
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(0, 0, 0, 0),
                    fixedHeight = 18f,
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 9,
                    //fontStyle = FontStyle.Bold
                };
            return _subHeaderStyle;
        }
    }

    public static GUIStyle SubLabelStyle
    {
        get
        {
            if (_subLabelStyle == null)
                _subLabelStyle = new GUIStyle
                {
                    normal = new GUIStyleState() { textColor = new Color(0.9f, 0.9f, 0.9f) },
                    fontStyle = FontStyle.Bold,
                    fontSize = 10,
                    alignment = TextAnchor.MiddleCenter
                };

            return _subLabelStyle;
        }
    }

    public static GUIStyle ToolbarStyle
    {
        get
        {
            if (_toolbarStyle == null)
                _toolbarStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("Toolbar"), textColor = Color.white },
                    fixedHeight = 25,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _toolbarStyle;
        }
    }

    public static GUIStyle TriggerActiveButtonStyle
    {
        get
        {
            if (_eventActiveButtonStyle == null)
                _eventActiveButtonStyle = new GUIStyle
                {
                    normal = { background = EventActiveTexture, textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };

            return _eventActiveButtonStyle;
        }
    }

    public static GUIStyle TriggerForwardButtonStyle
    {
        get
        {
            if (_eventForwardButtonStyle == null)
                _eventForwardButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("EventForward"), textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };

            return _eventForwardButtonStyle;
        }
    }

    public static GUIStyle TriggerInActiveButtonStyle
    {
        get
        {
            if (_eventInActiveButtonStyle == null)
                _eventInActiveButtonStyle = new GUIStyle
                {
                    normal = { background = EventInActiveTexture, textColor = Color.white },
                    padding = new RectOffset(10, 10, 10, 10),
                    fixedHeight = 16f,
                    fixedWidth = 16f
                };
            return _eventInActiveButtonStyle;
        }
    }

    public static GUIStyle VariablesButtonActiveStyle
    {
        get
        {
            if (_variablesButtonStyle == null)
                _variablesButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("VariablesButtonActive"), textColor = Color.white },
                    fixedHeight = 28,
                    fixedWidth = 47,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _variablesButtonStyle;
        }
    }

    public static GUIStyle VariablesButtonStyle
    {
        get
        {
            if (_variablesButtonStyle == null)
                _variablesButtonStyle = new GUIStyle
                {
                    normal = { background = GetSkinTexture("VariablesButton"), textColor = Color.white },
                    focused = { background = GetSkinTexture("VariablesButton"), textColor = Color.white },
                    active = { background = GetSkinTexture("VariablesButton"), textColor = Color.white },
                    hover = { background = GetSkinTexture("VariablesButton"), textColor = Color.white },
                    onHover = { background = GetSkinTexture("VariablesButtonActive"), textColor = Color.white },
                    onFocused = { background = GetSkinTexture("VariablesButtonActive"), textColor = Color.white },
                    onNormal = { background = GetSkinTexture("VariablesButtonActive"), textColor = Color.white },
                    onActive = { background = GetSkinTexture("VariablesButtonActive"), textColor = Color.white },
                    fixedHeight = 28,
                    fixedWidth = 47,
                    padding = new RectOffset(0, 0, 5, 5)
                };

            return _variablesButtonStyle;
        }
    }
}