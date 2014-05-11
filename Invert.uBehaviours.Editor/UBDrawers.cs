using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[UBCustomDrawer(typeof(Array))]
public class ArrayDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        var elementType = info.FieldType.GetElementType();

        var drawer = UBDrawers.GetDrawerFor(elementType);

        var arr = (Array)value;
        var newSize = EditorGUILayout.TextField("Size", arr.Length.ToString());
        GUILayout.Label(label);
        if (newSize.Length > 0 && newSize.All(p => char.IsNumber(p)) && Convert.ToInt32(newSize) != arr.Length)
        {
            return ResizeArray(arr, Convert.ToInt32(newSize));
        }
        EditorGUI.indentLevel++;
        for (var i = 0; i < arr.Length; i++)
        {
            var currentValue = arr.GetValue(i);
            var newValue = drawer.DrawProperty(target, info, currentValue, new GUIContent(label.text + " " + i));
            if (currentValue != newValue)
            {
                arr.SetValue(newValue, i);
            }
        }
        EditorGUI.indentLevel--;
        return value;
    }

    public System.Array ResizeArray(System.Array oldArray, int newSize)
    {
        int oldSize = oldArray.Length;
        System.Type elementType = oldArray.GetType().GetElementType();
        System.Array newArray = System.Array.CreateInstance(elementType, newSize);

        int preserveLength = System.Math.Min(oldSize, newSize);

        if (preserveLength > 0)
            System.Array.Copy(oldArray, newArray, preserveLength);

        return newArray;
    }
}

[UBCustomDrawer(typeof(bool))]
public class BoolDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.Toggle(label, (bool)value);
    }
}

[UBCustomDrawer(typeof(Color))]
public class ColorDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.ColorField(label.text, (Color)value);
    }
}

[UBCustomDrawer(typeof(Enum))]
public class EnumDrawer : UBPropertyDrawer
{
    
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        var values = (int[])Enum.GetValues(info.FieldType);
        var names = Enum.GetNames(info.FieldType);
        var selectedIndex = Array.IndexOf(values, value);
        var v = EditorGUILayout.Popup(label.text, selectedIndex, names);
        if (v > -1 && v < values.Length)
        {
            return values[v];
        }
        return 0;
    }
}

[UBCustomDrawer(typeof(float))]
public class FloatDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.FloatField(label, (float)value);
    }
}
[UBCustomDrawer(typeof(Quaternion))]
public class QuaternionDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        Debug.Log("Drawing Quaternion");
        return Quaternion.Euler(EditorGUILayout.Vector3Field(label, ((Quaternion)value).eulerAngles));
    }
}
[UBCustomDrawer(typeof(int))]
public class IntDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.IntField(label, (int)value);
    }
}

[UBCustomDrawer(typeof(LayerMask))]
public class LayerMaskDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.LayerField(label.text, (int)value);
    }
}

[UBCustomDrawer(typeof(UnityEngine.Object))]
public class ObjectDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.ObjectField(label.text, value as UnityEngine.Object, info.FieldType.IsArray ? info.FieldType.GetElementType() : info.FieldType, true);
    }
}

[UBCustomDrawer(typeof(Rect))]
public class RectDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.RectField(label, (Rect)value);
    }
}

[UBCustomDrawer(typeof(string))]
public class StringDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.TextField(label, (string)value);
    }
}

public class UBCustomDrawerAttribute : Attribute
{
    public Type DrawerFor
    {
        get;
        set; //
    }

    public UBCustomDrawerAttribute(Type drawerFor)
    {
        DrawerFor = drawerFor;
    }
}

public class UBDrawers
{
    private static Dictionary<Type, UBPropertyDrawer> _drawers;

    public static Dictionary<Type, UBPropertyDrawer> Drawers
    {
        get
        {
            if (_drawers == null)
            {
                LoadDrawers();
            }
            return _drawers;
        }
        set { _drawers = value; }
    }

    public static UBPropertyDrawer GetDrawerFor(IUBFieldInfo field)
    {
        var customAttribute = field.GetCustomAttributes(typeof(UBPropertyAttribute), true).FirstOrDefault();
        if (customAttribute != null)
        {
            return GetDrawerFor(customAttribute.GetType());
        }
        return GetDrawerFor(field.FieldType);
    }

    public static UBPropertyDrawer GetDrawerFor(Type type)
    {
        if (Drawers.ContainsKey(type))
        {
            return Drawers[type];
        }
        var key = Drawers.Keys.FirstOrDefault(p => p.IsAssignableFrom(type));
        if (key != null)
        {
            return Drawers[key];
        }
        return null;
    }

    /// <summary>
    /// Load the Property Drawers
    /// </summary>
    private static void LoadDrawers()
    {
        //      Debug.Log("Loading Drawers");
        _drawers = new Dictionary<Type, UBPropertyDrawer>();
        var drawers = ActionSheetHelpers.GetDerivedTypes<UBPropertyDrawer>(false, false);
        foreach (var drawer in drawers)
        {
            var drawerFor = drawer.GetCustomAttributes(typeof(UBCustomDrawerAttribute), false).FirstOrDefault() as UBCustomDrawerAttribute;
            if (drawerFor == null) continue;
            //            Debug.Log(drawer.Name);
            if (_drawers.ContainsKey(drawerFor.DrawerFor))
            {
                _drawers.Remove(drawerFor.DrawerFor);
            }
            _drawers.Add(drawerFor.DrawerFor, Activator.CreateInstance(drawer) as UBPropertyDrawer);
        }
    }
}

public abstract class UBPropertyDrawer : Attribute
{
    private IUBFieldInfo _fieldInfo;

    public object[] Attributes
    {
        get;
        set;
    }

    public IUBehaviours Behaviour
    {
        get { return UndoTarget as IUBehaviours; }
    }

    public IUBFieldInfo FieldInfo
    {
        get { return _fieldInfo; }
        set
        {
            _fieldInfo = value;
            Attributes = _fieldInfo.GetCustomAttributes(false).ToArray();
        }
    }

    public bool ForceSave
    {
        get;
        set;
    }

    public string HelpText
    {
        get
        {
            var help = Attributes.OfType<UBHelpAttribute>().FirstOrDefault();
            if (help == null) return null;
            return help.Text;
        }
    }

    public bool IsRequired
    {
        get
        {
            return Attributes.OfType<UBRequiredAttribute>().FirstOrDefault() != null;
        }
    }

    public Object UndoTarget
    {
        get;
        set;
    }

    public bool Draw(UnityEngine.Object undoTarget, object target, IUBFieldInfo info, object value, GUIContent label)
    {
        UndoTarget = undoTarget;
        FieldInfo = info;
        label.tooltip = HelpText ?? string.Empty;
        EditorGUI.BeginChangeCheck();
        var newValue = DrawProperty(target, info, value, label);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(undoTarget, string.Format("Set {0} on action.", UBEditor.PrettyLabel(info.Name)));
            info.SetValue(target, newValue);
            return true;
        }
        var forceSave = ForceSave;
        ForceSave = false;
        return forceSave;
    }

    public abstract object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label);
}

[UBCustomDrawer(typeof(Vector2))]
public class Vector2Drawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.Vector2Field(label, (Vector2)value);
    }
}

[UBCustomDrawer(typeof(Vector3))]
public class Vector3Drawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.Vector3Field(label, (Vector3)value);
    }
}

[UBCustomDrawer(typeof(Vector4))]
public class Vector4Drawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        return EditorGUILayout.Vector4Field(label.text, (Vector4)value);
    }
}