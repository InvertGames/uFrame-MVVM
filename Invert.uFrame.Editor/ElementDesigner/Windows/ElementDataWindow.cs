using System;
using UnityEditor;
using UnityEngine;

public abstract class ElementDataWindow : EditorWindow
{
    public string _SearchText = "";
    public int _SelectedIndex;
    private Vector2 _scrollPosition;
    protected int _limit = 50;
    protected Func<ElementItemType, string> _labelSelector;

    protected string _upperSearchText;
    public void OnGUI()
    {
        _SearchText = GUILayout.TextField(_SearchText ?? "");
        GUILayout.Label("Search to find more...");
        _upperSearchText = _SearchText.ToUpper();
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        OnGUIScrollView();

        EditorGUILayout.EndScrollView();
    }

    public abstract void OnGUIScrollView();
}

public abstract class AddBindingWindow : EditorWindow
{
    
}