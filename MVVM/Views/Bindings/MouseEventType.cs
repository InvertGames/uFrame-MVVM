namespace uFrame.MVVM.Bindings
{
    /// <summary>
    /// A Unity mouse event. The comments are from the unity documentation.
    /// </summary>
    public enum MouseEventType
    {
        OnBecameInvisible, //OnBecameInvisible is called when the renderer is no longer visible by any camera.
        OnBecameVisible, //OnBecameVisible is called when the renderer became visible by any camera.
        OnMouseDown,
        //OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
        OnMouseDrag,
        //OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
        OnMouseEnter, //OnMouseEnter is called when the mouse entered the GUIElement or Collider.
        OnMouseExit, //OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider.
        OnMouseOver, //OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.
        OnMouseUp, //OnMouseUp is called when the user has released the mouse button.
        OnMouseUpAsButton,
        //OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.
    }
}