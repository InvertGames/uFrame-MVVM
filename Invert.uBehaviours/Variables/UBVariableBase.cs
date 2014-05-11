using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBehaviours.Actions;
using UnityEngine;

/// <summary>
/// All UB Variables derive from this.  Even UBVariableDeclare.
/// These variables can be accessed in context and also store information about how the variable resolves its value.
/// It keeps track of variable references
/// </summary>
[Serializable]
public abstract class UBVariableBase : IUBSerializable
{

    [SerializeField]
    public string _ValueFromVariableName = string.Empty;

    private bool _isResult = false;

    protected UBVariableBase _referenceVariable;

    [SerializeField]
    private int _valueFrom;

    [SerializeField]
    private string _varName;

    /// <summary>
    /// The unique identifier for this variable.  If you know the variable name will be Unique you can override
    /// to use just a Name. Usually that would be in the case of static variable declares manually created.
    /// </summary>
    public virtual string Guid { get; set; }

    public bool IsResult
    {
        get { return _isResult; }
        set { _isResult = value; }
    }

    /// <summary>
    /// The literal underlying object value of this variable.
    /// The ValueFrom must equal "0" for this value to be used in context.
    /// </summary>
    public abstract object LiteralObjectValue { get; set; }

    /// <summary>
    /// The name of this variable.
    /// </summary>
    public virtual string Name
    {
        get { return _varName; }
        set { _varName = value; }
    }

    /// <summary>
    /// How should the value be resolved?
    /// </summary>
    public int ValueFrom
    {
        get { return _valueFrom; }
        set { _valueFrom = value; }
    }

    /// <summary>
    /// The type of value that this UBVariable encapsulates
    /// </summary>
    public abstract Type ValueType
    {
        get;
    }

    /// <summary>
    /// The UB Variable Type that this variable encapsulates.
    /// </summary>
    public virtual UBVarType VarType { get; set; }

    protected UBVariableBase(bool isResult)
    {
        _isResult = isResult;
        if (_isResult)
        {
            _valueFrom = 1;
        }
    }

    public void ApplyValueFromInfo(ValueFromInfo info)
    {
        //Debug.Log("Applying" + info._ValueFrom + " : " + info._ValueFromVariableName);
        _ValueFromVariableName = info._ValueFromVariableName;
        ValueFrom = info._ValueFrom;
    }

    /// <summary>
    /// Check if there are any errors on this variable. Return null if none.
    /// </summary>
    /// <param name="container">The source Behaviour</param>
    /// <param name="action">The action that contains this variable</param>
    /// <param name="required"></param>
    /// <param name="isRequired">Is this variable a required variable via UBRequiredAttrbute.</param>
    /// <returns>Returns a string with the error message. Will be null if there are no errors.</returns>
    public virtual string CheckForErrors(IUBehaviours container, UBAction action, TriggerInfo required, bool isRequired)
    {
        if (ValueFrom == 0 && isRequired && LiteralObjectValue == null)
        {
            return "Value can not be empty";
        }

        if (ValueFrom == 1)// && isRequired)
        {
            if (isRequired && string.IsNullOrEmpty(_ValueFromVariableName))
                return "You must specify a valid variable.";

            if (container == null)
                return "Container is null";

            if (isRequired && container.FindDeclare(_ValueFromVariableName, action.ActionSheet.TriggerInfo) == null)
            {
                return "Couldn't find variable. Has it been removed?";
            }
        }
        return null;
    }

    /// <summary>
    /// Convert this variable to a declared variable.
    /// </summary>
    /// <returns>The declared variable</returns>
    public virtual UBVariableDeclare CreateAsDeclare()
    {
        var declare = new UBVariableDeclare
        {
            Name = "NewVariableName",
            _GUID = System.Guid.NewGuid().ToString(),
            VarType = VarType
        };
        //declare.GetType().GetField(string.Format("_{0}Value", VarType.ToString().ToLower())).SetValue(this,LiteralObjectValue);
        return declare;
    }

    /// <summary>
    /// Deserialize this variable from a byte array using the serializer.
    /// </summary>
    /// <param name="referenceHolder">The referenceHolder to restore references to UnityEngine.Object's</param>
    /// <param name="serializer">the binary serializer/deserializer</param>
    public virtual void Deserialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        this._valueFrom = serializer.DeserializeInt();
        this._ValueFromVariableName = serializer.DeserializeString();
        if (_valueFrom == 0)
        {
            DeserializeValue(referenceHolder, serializer);
        }
    }

    /// <summary>
    /// Override this method to actually deserialize the LiteralObjectValue. Only invoked when ValueFrom is equal to zero.
    /// </summary>
    /// <param name="referenceHolder">The referenceHolder to restore references to UnityEngine.Object's</param>
    /// <param name="serializer">The binary serializer/deserializer</param>
    public abstract void DeserializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer);

    /// <summary>
    /// Convert this variable into a friendly string.  If its a variable return the name.  If its a literal return that
    /// </summary>
    /// <returns>A pretty title for this variable.</returns>
    public virtual string ToString(IUBehaviours behaviour)
    {
       
        if (behaviour != null && (_valueFrom == 1 || _valueFrom > 2))
        {
            var declare = behaviour.FindDeclare(_ValueFromVariableName);
            if (declare != null)
                return declare.Name;
        }
        //return "";
        return ToString();
    }

    /// <summary>
    /// Get the actual runtime value of this variable based off of GetObjectValue
    /// </summary>
    /// <param name="context">The context at which to get reference variables or access other context sensitive data.</param>
    /// <returns>The value</returns>
    public virtual object GetObjectValue(IUBContext context)
    {
        if (_valueFrom == 0)
        {
            return LiteralObjectValue;
        }
        //
        _referenceVariable = context.GetVariableById(_ValueFromVariableName);
        if (_referenceVariable == null)
        {
            var sb = new StringBuilder();
            foreach (var v in context.Variables)
            {
                sb.AppendFormat("{0}:{1}", v.Name, v.Guid).AppendLine();
            }
            throw new Exception(string.Format("\"{0}\" variable was not found.", _ValueFromVariableName));
        }
        return _referenceVariable.LiteralObjectValue;
    }

    /// <summary>
    /// Grabs the reference variable.
    /// </summary>
    /// <param name="action">The action this variable belongs to.</param>
    /// <param name="instance">The behaviour the action belongs to.</param>
    /// <returns>The declare that this variable references.</returns>
    public IUBVariableDeclare GetReferenceDeclare(UBAction action, IUBehaviours instance)
    {
        return action.ActionSheet.RootContainer.FindDeclare(_ValueFromVariableName);
    }
    /// <summary>
    /// If this variable is reference return the reference.  Otherwise return this;
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public UBVariableBase GetReferenceIfUsed(UBAction action)
    {
        return action.RootContainer.FindDeclare(_ValueFromVariableName) as UBVariableBase ?? this;
    }
    /// <summary>
    /// Wrapper for FillValueFromOptions to return a dictionary.
    /// </summary>
    /// <returns>Aictionary of the available "ValueFrom" possibilities.</returns>
    public IEnumerable<ValueFromInfo> GetValueFromOptions(IUBehaviours behaviour, UBAction action, IUBVariableDeclare[] contextVariables)
    {
        return FillValueFromOptions(contextVariables);
    }

    public abstract string GetVariableReferenceString();


    public virtual void Serialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        serializer.Serialize(_valueFrom);
        serializer.Serialize(_ValueFromVariableName ?? string.Empty);
        if (_valueFrom == 0)
            SerializeValue(referenceHolder, serializer);
    }

    public abstract void SerializeValue(IReferenceHolder referenceHolder, UBBinarySerializer serializer);

    /// <summary>
    /// Set the object value in the context.
    /// </summary>
    /// <param name="context">The context of the value.</param>
    /// <param name="value">The value to set</param>
    public virtual void SetObjectValue(IUBContext context, object value)
    {
        if (_valueFrom == 0)
        {
            LiteralObjectValue = value;
            return;
        }
        context.SetVariableById(_ValueFromVariableName, value);
    }

    public override string ToString()
    {
        if (_valueFrom == 0)
        {
            if (this.LiteralObjectValue == null) return "";
            return this.LiteralObjectValue.ToString();
        }
        if (_valueFrom == 1)
        {
            return _varName ?? string.Empty;
        }
        if (_valueFrom == 2)
        {
            return "Instance";
        }

        return _varName ?? string.Empty;
    }

    /// <summary>
    /// Override this to extend various options from ValueFrom. But there must be options for 0 and 1
    /// 0=Literal/Specified
    /// 1=Reference To Another Variable
    /// </summary>
    /// <param name="contextVariables"></param>
    protected abstract IEnumerable<ValueFromInfo> FillValueFromOptions(IUBVariableDeclare[] contextVariables);
}