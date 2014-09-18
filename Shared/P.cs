using System;


#if DLL
using Invert.uFrame.Editor;
namespace Invert.MVVM
{
#endif
/// <summary>
/// A typed ViewModel Property Class
/// </summary>
/// <typeparam name="T"></typeparam>
//public class P<T> : ModelPropertyBase
//{
 

//    /// <summary>
//    /// Gets or sets the value.
//    /// </summary>
//    /// <value>The value.</value>
//    public T Value
//    {
//        get
//        {
//            if (ObjectValue == null)
//                return default(T);

//            try
//            {
//                return (T)ObjectValue;
//            }
//            catch
//            {
//                //UnityEngine.Debug.LogError(ObjectValue.GetType().FullName + " TO " + typeof(T).FullName + " : " + ex.Message);
//                return default(T);
//            }
//        }
//        set
//        {
//            if (CanSetValue(value))
//            {
//                ObjectValue = value;
//            }
//        }
//    }

//    public T LastValue
//    {
//        get
//        {
//            if (LastValueObject == null)
//                return default(T);

//            return (T) LastValueObject;
//        }
//    }
   
//    /// <summary>
//    /// Gets the type of the value.
//    /// </summary>
//    /// <value>The type of the value.</value>
//    public override Type ValueType
//    {
//        get
//        {
//            return typeof(T);
//        }
//    }

//    public P()
//    {
//        Value = default(T);
//    }

//    public P(ViewModel owner, string propertyName) : base(owner, propertyName)
//    {

//    }
//    public P(ViewModel owner, string propertyName,T value)
//        : base(owner, propertyName)
//    {
//        Value = value;
//    }
//    public P(T value)
//    {
//        Value = value;
//    }

//    /// <summary>
//    /// The binding class that allows chaining extra options.
//    /// </summary>
//    /// <param name="listener">Should set the value of the target.</param>
//    /// <returns>The binding class that allows chaining extra options.</returns>
//    public ModelPropertyBinding Subscribe(Action<T> listener)
//    {
//        var binding = new ModelPropertyBinding()
//        {

//            SetTargetValueDelegate = (o) => listener((T)o),
//            ModelPropertySelector = () => this,
//            TwoWay = false
//        };
//        Owner.AddBinding(binding);
//        return binding;
//    }


//    public virtual bool CanSetValue(T value)
//    {
//        return true;
//    }

//    /// <summary>
//    /// Deserialize the specified node into `Value`.
//    /// </summary>
//    /// <param name="node">Node.</param>
//    public override void Deserialize(JSONNode node)
//    {
//        this.ObjectValue = DeserializeObject(ValueType, node);
//    }

//    //public override int GetHashCode()
//    //{
//    //    return Value.GetHashCode();
//    //}

//    #region Operator Overloads

//    public override bool Equals(object obj)
//    {
//        if (obj is P<T>)
//        {
//            var tObj = (P<T>)obj;
//            return Value.Equals(tObj.Value);
//        }
//        return Value.Equals(obj);
//    }

//    public override int GetHashCode()
//    {
//        return Value.GetHashCode();
//    }

//    //public static bool operator ==(P<T> a, P<T> b)
//    //{
//    //    return a.Value.Equals(b.Value);
//    //}

//    //public static bool operator !=(P<T> a, P<T> b)
//    //{
//    //    if (a == null || b == null)
//    //    {
//    //        return !(a == null && b == null);
//    //    }
//    //    return !(a.Value.Equals(b.Value));
//    //}

//    //public static bool operator ==(P<T> a, T b)
//    //{
//    //    return a.Value.Equals(b);
//    //}

//    //public static bool operator !=(P<T> a, T b)
//    //{
//    //    return !(a.Value.Equals(b));
//    //}

//    //public static implicit operator P<T>(T d)
//    //{
//    //    return new P<T> { Value = d };
//    //}

//    ////  User-defined conversion from double to Digit
//    //public static implicit operator T(P<T> d)
//    //{
//    //    return d.Value;
//    //}

//    #endregion Operator Overloads

//    /// <summary>
//    /// Serializes this object
//    /// </summary>
//    public override JSONNode Serialize()
//    {
//        var value = ObjectValue;
//        return SerializeObject(ValueType, value);
//    }
//}

public class Computed<T> : P<T>
{
    private Func<ViewModel, T> _calculator;

    public Func<ViewModel, T> Calculator
    {
        get { return _calculator; }
        set
        {
            _calculator = value;
            if (_calculator != null)
            {
                DependantPropertyOnValueChanged(null);
            }
        }
    }

    public Computed(ViewModel owner, string propertyName,
       params IObservableProperty[] dependantProperties)
        : base(owner, propertyName)
    {

        foreach (var dependantProperty in dependantProperties)
        {
            dependantProperty.SubscribeInternal(DependantPropertyOnValueChanged);
        }
      
    }
    public Computed(ViewModel owner, string propertyName, Func<ViewModel,T> calculator,
        params IObservableProperty[] dependantProperties)
        : base(owner, propertyName)
    {
        Calculator = calculator;
        foreach (var dependantProperty in dependantProperties)
        {
            dependantProperty.SubscribeInternal(DependantPropertyOnValueChanged);
        }
    
    }

    public virtual bool CanSetValue(T value)
    {
        return true;
    }

    private void DependantPropertyOnValueChanged(object value)
    {
        if (Calculator != null)
        this.Value = Calculator(Owner);
    }

    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <param name="listener">Should set the value of the target.</param>
    /// <param name="immediate">Should the listener be invoked immediately (defaults to true).</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public ModelPropertyBinding Subscribe(Action<T> listener, bool immediate = true)
    {
        var binding = new ModelPropertyBinding()
        {
            SetTargetValueDelegate = (o) => listener((T)o),
            ModelPropertySelector = () => this,
            IsImmediate =  immediate,
            TwoWay = false
        };
        Owner.AddBinding(binding);
        return binding;
    }
    public override bool Equals(object obj)
    {
        if (obj is P<T>)
        {
            var tObj = (P<T>)obj;
            return Value.Equals(tObj.Value);
        }
        return Value.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }


}
#if DLL
}
#endif