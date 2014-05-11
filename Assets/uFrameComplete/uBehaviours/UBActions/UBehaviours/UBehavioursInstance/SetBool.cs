using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[UBCategory("Variables")]
public class SetUBAnimation : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBAnimation _Target = new UBAnimation();

    public UBAnimation _Value = new UBAnimation();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBBool : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBBool _Target = new UBBool();

    public UBBool _Value = new UBBool();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBColor : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBColor _Target = new UBColor();

    public UBColor _Value = new UBColor();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBFloat : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBFloat _Target = new UBFloat();

    public UBFloat _Value = new UBFloat();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBGameObject : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBGameObject _Target = new UBGameObject();

    public UBGameObject _Value = new UBGameObject();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBInt : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBInt _Target = new UBInt();

    public UBInt _Value = new UBInt();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBMaterial : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBMaterial _Target = new UBMaterial();

    public UBMaterial _Value = new UBMaterial();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBObject : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBObject _Target = new UBObject();

    public UBObject _Value = new UBObject();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBQuaternion : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBQuaternion _Target = new UBQuaternion();

    public UBQuaternion _Value = new UBQuaternion();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBRect : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBRect _Target = new UBRect();

    public UBRect _Value = new UBRect();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBString : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBString _Target = new UBString();

    public UBString _Value = new UBString();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBTexture : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBTexture _Target = new UBTexture();

    public UBTexture _Value = new UBTexture();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBTransform : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBTransform _Target = new UBTransform();

    public UBTransform _Value = new UBTransform();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBVector2 : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBVector2 _Target = new UBVector2();

    public UBVector2 _Value = new UBVector2();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}

[UBCategory("Variables")]
public class SetUBVector3 : UBAction
{
    [UBRequireVariable, UBRequired]
    public UBVector3 _Target = new UBVector3();

    public UBVector3 _Value = new UBVector3();

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("#_Target# = #_Value#");
    }

    protected override void PerformExecute(IUBContext context)
    {
        _Target.SetValue(context, _Value.GetValue(context));
    }
}