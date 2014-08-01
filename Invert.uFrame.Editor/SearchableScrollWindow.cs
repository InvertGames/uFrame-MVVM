using System;
using UnityEditor;
using UnityEngine;

public abstract class SearchableScrollWindow : EditorWindow
{
    public string _SearchText = "";
    public int _SelectedIndex;
    private Vector2 _scrollPosition;
    protected int _limit = 50;
    protected Func<ElementItemType, string> _labelSelector;
    
    protected string _upperSearchText;
    public virtual void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        _SearchText = GUILayout.TextField(_SearchText ?? "");
        if (EditorGUI.EndChangeCheck())
        {
            ApplySearch();
        }
        GUILayout.Label("Search to find more...");
        _upperSearchText = _SearchText.ToUpper();
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        OnGUIScrollView();

        EditorGUILayout.EndScrollView();
    }

    protected abstract void ApplySearch();
    public abstract void OnGUIScrollView();
}