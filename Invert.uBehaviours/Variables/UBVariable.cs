using System;
using System.Collections.Generic;
using System.Diagnostics;

[Serializable]
public abstract class UBVariable<T> : UBVariableBase
{
    /// <summary>
    /// The Value of the variable typed.
    /// </summary>
    public T LiteralValue
    {
        get
        {
            return (T)LiteralObjectValue;
        }
        set
        {
            LiteralObjectValue = value;
        }
    }
    /// <summary>
    /// The type of value of this variable
    /// </summary>
    public override Type ValueType
    {
        get { return typeof(T); }
    }

    protected UBVariable(bool isResult)
        : base(isResult)
    {
        //LiteralObjectValue = default(T);
    }
    /// <summary>
    /// Gets the value of the variable in the context.
    /// </summary>
    /// <param name="context">The context at which to find the variable if referenced.</param>
    /// <returns>The value of this variable</returns>
    public T GetValue(IUBContext context)
    {
        return (T)GetObjectValue(context);
    }

    public override string GetVariableReferenceString()
    {
        
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the value of this variable.
    /// </summary>
    /// <param name="context">The context at which to set the value.  This is only needed if the variable is referenced.</param>
    /// <param name="value">The value to set on this variable or its referenced variable.</param>
    public void SetValue(IUBContext context, T value)
    {
        SetObjectValue(context, value);
    }
}