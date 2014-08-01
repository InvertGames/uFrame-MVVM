
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Invert.Common.UI
{
    public class GUIHelpers
    {
        public static Rect GetRect(GUIStyle style, bool fullWidth = true, params GUILayoutOption[] options)
        {
            var rect = GUILayoutUtility.GetRect(GUIContent.none, style, options);
            if (!fullWidth) return rect;
            //var indentAmount = (Indent * 25);
            rect.x -= 13;
            //rect.x += +(indentAmount);
            rect.width += 17;
            //rect.width -= indentAmount;
            rect.y += 3;
            return rect;
        }
        public static bool DoToolbar(string label, bool open, Action add = null, Action leftButton = null, Action paste = null, GUIStyle addButtonStyle = null, GUIStyle pasteButtonStyle = null,bool fullWidth = true)
        {
            var rect = GetRect(UBStyles.ToolbarStyle,fullWidth);
            GUI.Box(rect, "", UBStyles.ToolbarStyle);
            var labelStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                fontSize = 10
            };
            var labelRect = new Rect(rect.x + 2, rect.y + (rect.height / 2) - 8, rect.width - 50, 16);
            var result = open;
            if (leftButton == null)
            {
                result = GUI.Button(labelRect,
                    new GUIContent(label,
                        open ? UBStyles.ArrowDownTexture : UBStyles.CollapseRightArrowTexture),
                    labelStyle);
            }
            else
            {
                if (GUI.Button(labelRect, new GUIContent(label, UBStyles.ArrowLeftTexture), labelStyle))
                {
                    leftButton();
                }
            }

            if (paste != null)
            {
                var addButtonRect = new Rect(rect.x + rect.width - 42, rect.y + (rect.height / 2) - 8, 16, 16);
                if (GUI.Button(addButtonRect, "", pasteButtonStyle ?? UBStyles.PasteButtonStyle))
                {
                    paste();
                }
            }

            if (add != null)
            {
                var addButtonRect = new Rect(rect.x + rect.width - 21, rect.y + (rect.height / 2) - 8, 16, 16);
                if (GUI.Button(addButtonRect, "", addButtonStyle ?? UBStyles.AddButtonStyle))
                {
                    add();
                }
            }
            return result;
        }

        public static bool DoToolbar(string label, Action add = null, Action leftButton = null, Action paste = null)
        {
            return DoToolbar(label, true, add, leftButton, paste);
        }
        public static bool DoTriggerButton(UFStyle ubTriggerContent)
        {
            var hasSubLabel = !String.IsNullOrEmpty(ubTriggerContent.SubLabel);

            var rect = !hasSubLabel
                ? GetRect(ubTriggerContent.BackgroundStyle,ubTriggerContent.FullWidth && !ubTriggerContent.IsWindow)
                : GetRect(ubTriggerContent.BackgroundStyle, ubTriggerContent.FullWidth && !ubTriggerContent.IsWindow, GUILayout.Height(35));

            var style = ubTriggerContent.BackgroundStyle;

            if (UFStyle.MouseDownStyle != null && ubTriggerContent.IsMouseDown(rect))
                style = UFStyle.MouseDownStyle;

            if (!ubTriggerContent.Enabled)
            {
                style = GUIStyle.none;
            }

            GUI.Box(rect, "", style);

            if (ubTriggerContent.MarkerStyle != null)
            {
                var rectIndicator = new Rect(rect);
                rectIndicator.width = 2;
                rectIndicator.y -= 2;
                rectIndicator.x = rect.width - 2;
                rectIndicator.height -= 3;
                GUI.Box(rectIndicator, "", ubTriggerContent.MarkerStyle);
            }
            if (ubTriggerContent.IconStyle != null )
            {
                var eventOptionsButtonRect = new Rect(rect.x + 5, rect.y + ((rect.height / 2) - 8), 16, 16);
                if (GUI.Button(eventOptionsButtonRect, "", ubTriggerContent.IconStyle))
                {
                    if (ubTriggerContent.OnShowOptions != null)
                    ubTriggerContent.OnShowOptions();
                }
                var seperatorRect = new Rect(rect) {width = 3};
                seperatorRect.y += 2;
                seperatorRect.height -= 5;
                seperatorRect.x = eventOptionsButtonRect.x + 17;
                GUI.Box(seperatorRect, String.Empty, UBStyles.SeperatorStyle);
            }

            var labelStyle =  new GUIStyle(EditorStyles.label) { alignment = ubTriggerContent.TextAnchor, fontSize = 10 };
            if (!ubTriggerContent.Enabled)
            {
                labelStyle.normal.textColor = new Color(0.4f,0.4f,0.4f);
                
            }
            var labelRect = new Rect(rect.x, rect.y - (hasSubLabel ? 6 : 0), rect.width - 30, rect.height);
            var lbl = ubTriggerContent.Label;
            var result = GUI.Button(labelRect, lbl, labelStyle);

            if (hasSubLabel)
            {
                var subLabelRect = new Rect(labelRect);
                subLabelRect.y += 12;

                GUI.Label(subLabelRect, ubTriggerContent.SubLabel, UFStyle.SubLabelStyle);
            }
            if (ubTriggerContent.ShowArrow)
                GUI.DrawTexture(new Rect(rect.x + rect.width - 18f, rect.y + ((rect.height / 2) - 8), 16, 16), UBStyles.ArrowRightTexture);
            if (ubTriggerContent.Enabled)
            {
                return result;
            }
            return result;
        }
    }

    public class UFStyle
    {
        private bool _fullWidth = true;
        private TextAnchor _textAnchor = TextAnchor.MiddleCenter;
        private GUIStyle _iconStyle;
        private GUIStyle _backgroundStyle;
        private static GUIStyle _subLabelStyle;

        public UFStyle()
        {
            Enabled = true;
        }

        public UFStyle(string label, GUIStyle backgroundStyle, GUIStyle indicatorStyle = null, GUIStyle optionsStyle = null, Action onShowOptions = null, bool showArrow = true, TextAnchor textAnchor = TextAnchor.MiddleRight)
        {
            Label = label;
            _backgroundStyle = backgroundStyle;
            MarkerStyle = indicatorStyle;
            IconStyle = optionsStyle;
            OnShowOptions = onShowOptions;
            ShowArrow = showArrow;
            _textAnchor = textAnchor;
            Enabled = true;
        }

        public GUIStyle BackgroundStyle
        {
            get { return _backgroundStyle ?? UBStyles.EventButtonStyle; }
            set { _backgroundStyle = value; }
        }

        public string SubLabel { get; set; }
        public static GUIStyle MouseDownStyle { get; set; }
        public GUIStyle MarkerStyle { get; set; }

        public GUIStyle IconStyle
        {
            get { return _iconStyle ?? UBStyles.TriggerActiveButtonStyle; }
            set { _iconStyle = value; }
        }

        public Action OnShowOptions { get; set; }
        public string Label { get; set; }

        public bool FullWidth
        {
            get { return _fullWidth; }
            set { _fullWidth = value; }
        }

        public bool IsWindow { get; set; }
        public TextAnchor TextAnchor
        {
            get { return _textAnchor; }
            set { _textAnchor = value; }
        }

        public static GUIStyle SubLabelStyle
        {
            get { return _subLabelStyle ?? UBStyles.SubLabelStyle; }
            set { _subLabelStyle = value; }
        }

        public bool ShowArrow { get; set; }
        public bool Enabled { get; set; }
        public string Group { get; set; }
        public object Tag { get; set; }

        public bool IsMouseDown(Rect rect)
        {
            return false;
        }
    }

}
