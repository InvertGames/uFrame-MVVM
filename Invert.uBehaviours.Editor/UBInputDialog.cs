using System;
using UnityEditor;
using UnityEngine;

public class UBInputDialog : EditorWindow
{
    public string _InputText = "";

    public Action<string> _OkAction;

    public Func<string, string> _Validator;

    public static void Init(string title, Action<string> okAction, string current = null, Func<string,string> validator = null)
    {
        var window = (UBInputDialog)GetWindow(typeof(UBInputDialog));
        window.title = title;
        window._OkAction = okAction;
        window._InputText = current ?? string.Empty;
        window._Validator = validator;
        window.ShowPopup();
    }

    public static void InitDrowDown(Rect buttonRect, string title, Action<string> okAction)
    {
        var window = (UBInputDialog)GetWindow(typeof(UBInputDialog));
        window.title = title;
        window._OkAction = okAction;
        window.ShowUtility();
        //window.ShowAsDropDown(buttonRect, new Vector2(200, 100));
    }

    public void OnGUI()
    {
        _InputText = GUILayout.TextField(_InputText ?? "");
        if (_Validator != null)
        {
            var result = _Validator(_InputText);
            if (result != null)
            {
                EditorGUILayout.HelpBox(result,MessageType.Error);
                return;
            }
        }
        if (_OkAction != null)
        {
            if (GUILayout.Button("OK"))
            {
                _OkAction(_InputText);
                this.Close();
            }
        }
    }
}