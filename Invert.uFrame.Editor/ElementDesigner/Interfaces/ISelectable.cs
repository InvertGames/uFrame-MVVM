using UnityEngine;

public interface ISelectable : IDrawable
{
    bool IsSelected { get; set; }
    void RemoveLink(IDiagramItem target);
    Vector2[] ConnectionPoints { get; set; }
}