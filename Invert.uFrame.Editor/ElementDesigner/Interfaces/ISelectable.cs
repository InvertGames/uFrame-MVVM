using UnityEngine;

public interface ISelectable : IDrawable
{
    bool IsSelected { get; set; }
    void RemoveLink(IDiagramNode target);
    Vector2[] ConnectionPoints { get; set; }
}