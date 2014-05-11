using System;
using System.Collections.Generic;
using UnityEngine;

public class DiagramItemHeader : IDrawable
{

    public string Label { get; set; }

    public delegate void AddItemClickedEventHandler();

    public event AddItemClickedEventHandler OnAddItem;

    protected virtual void OnOnAddItem()
    {
        AddItemClickedEventHandler handler = OnAddItem;
        if (handler != null) handler();
    }



    public void Draw(float scale,GUIStyle textColorStyle)
    {
        var style = UFStyles.HeaderStyle;//.Scale(scale);
        style.normal.textColor = textColorStyle.normal.textColor;
        style.fontStyle = FontStyle.Bold;

        GUI.Box(Position.Scale(scale), Label, style);
        var btnRect = new Rect();
        btnRect.y = Position.y + ((Position.height / 2) - 8);
        btnRect.x = Position.x + Position.width - 18;
        btnRect.width = 16;
        btnRect.height = 16;
        if (OnAddItem != null)
        {
            if (GUI.Button(btnRect.Scale(scale), string.Empty, UFStyles.AddButtonStyle))
            {
                OnOnAddItem();
            }    
        }
        
    }

    public Rect Position { get; set; }
    public Type HeaderType { get; set; }

    public void CreateLink(IDiagramItem container, IDrawable target)
    {
        throw new NotImplementedException();
    }

    public bool CanCreateLink(IDrawable target)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IDiagramLink> GetLinks(IDiagramItem[] elementDesignerData)
    {
        yield break;
    }
}