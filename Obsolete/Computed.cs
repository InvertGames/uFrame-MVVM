using System;
using uFrame.MVVM;
using uFrame.MVVM.Bindings;


#if DLL
using Invert.uFrame.Editor;
namespace Invert.MVVM
{
#endif
[Obsolete]
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
    public Computed(
       params IObservableProperty[] dependantProperties)
        : base()
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