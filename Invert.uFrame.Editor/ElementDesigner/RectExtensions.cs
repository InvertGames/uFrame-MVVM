using UnityEngine;

public static class RectExtensions
{

    public static Rect Scale(this Rect r, float scale)
    {
        return new Rect(r.x * scale, r.y * scale, r.width * scale, r.height * scale);
    }
    public static Rect Normalize(this Rect r, float scale)
    {
        return new Rect(r.x / scale, r.y / scale, r.width / scale, r.height / scale);
    }
    public static RectOffset Scale(this RectOffset r, float scale)
    {
        return new RectOffset(Mathf.RoundToInt(r.left * scale), Mathf.RoundToInt(r.right * scale), Mathf.RoundToInt(r.top * scale), Mathf.RoundToInt(r.bottom * scale));
    }
    public static GUIStyle Scale(this GUIStyle style, float scale)
    {
        var s = new GUIStyle(style);
        s.fontSize = Mathf.RoundToInt(style.fontSize * scale);
        s.fixedHeight = Mathf.RoundToInt(style.fixedHeight * scale);
        s.fixedWidth = Mathf.RoundToInt(style.fixedWidth * scale);
        s.padding = s.padding.Scale(scale);
        s.margin = s.margin.Scale(scale);
        return s;
    }
}