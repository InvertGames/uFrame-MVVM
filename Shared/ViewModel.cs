using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
#if DLL
using Invert.MVVM;
using Invert.uFrame.Editor;
namespace Invert.MVVM
{
#endif
/// <summary>
///  A data structure that contains information/data needed for a 'View'
/// </summary>
[Serializable]
public abstract class ViewModel 
#if !DLL
    : IJsonSerializable, IUFSerializable, IViewModelObserver, INotifyPropertyChanged
#else
 : INotifyPropertyChanged
#endif
{
    public event PropertyChangedEventHandler PropertyChanged;

    private Dictionary<int, List<IBinding>> _bindings;
    private Dictionary<string, ICommand> _commands;
    private Controller _controller;
    private Dictionary<string, ModelPropertyBase> _modelProperties;
    private string _identifier;
    //private List<IBinding> _bindings;
    
    /// <summary>
    ///Access a model property via string.  This is optimized using a compiled delegate to
    ///access derived classes properties so use as needed
    /// </summary>
    /// <param name="bindingPropertyName">The name of the property/field to access</param>
    /// <returns>ModelPropertyBase The Model Property class.  Use value to get the value of the property</returns>
    public ModelPropertyBase this[string bindingPropertyName]
    {
        get
        {
            try
            {
                CacheReflectedModelProperties();
                return _modelProperties[bindingPropertyName];
            }
            catch (Exception ex)
            {
                throw new Exception(bindingPropertyName + " was not found on " + this.ToString() + ": " + ex.Message);
            }
        }
    }

    public Dictionary<int, List<IBinding>> Bindings
    {
        get { return _bindings ?? (_bindings = new Dictionary<int, List<IBinding>>()); }
        set { _bindings = value; }
    }

    public Dictionary<string, ICommand> Commands
    {
        get
        {
            if (_commands == null)
            {
                var dictionary = new Dictionary<string, ICommand>();
                foreach (KeyValuePair<string, PropertyInfo> command in GetReflectedCommands(this.GetType()))
                    dictionary.Add(command.Key, (ICommand)command.Value.GetValue(this, null));
                _commands = dictionary;
            }
            return _commands;
        }
    }

    public Controller Controller
    {
        get { return _controller; }
        set
        {
            if (_controller == value)
                return;
            if (value != null)
            {
                WireCommands(value);
                //value.Initialize(this);
            }
            _controller = value;
        }
    }

    public bool Dirty { get; set; }

    public virtual string Identifier
    {
        get { return _identifier ?? (_identifier = Guid.NewGuid().ToString()); }
        set { _identifier = value; }
    }

    public Dictionary<string, ModelPropertyBase> Properties
    {
        get
        {
            CacheReflectedModelProperties();
            return _modelProperties;
        }
    }

    public int References { get; set; }
#if !DLL
    public ICommandHandler CommandHandler { get; set; }


#endif
    /// <summary>
    /// Grabs all the commands available for a viewmodel type
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    public static Dictionary<string, PropertyInfo> GetReflectedCommands(Type modelType)
    {
        var modelProperties = new Dictionary<string, PropertyInfo>();
        var fields = modelType.GetProperties();
        foreach (var field in fields)
        {
            if (typeof(ICommand).IsAssignableFrom(field.PropertyType))
            {
                modelProperties.Add(field.Name, field);
            }
        }
        return modelProperties;
    }

    /// <summary>
    /// Grab the bindable properties for the view-model
    /// </summary>
    /// <param name="modelType"></param>
    /// <returns></returns>
    public static Dictionary<string, FieldInfo> GetReflectedModelProperties(Type modelType)
    {
        var modelProperties = new Dictionary<string, FieldInfo>();
        var fields = modelType.GetFields();
        foreach (var field in fields.Where(p => p.IsPublic && p.IsInitOnly))
        {
            modelProperties.Add(field.Name, field);
        }
        return modelProperties;
    }

    public void AddBinding(IBinding binding)
    {
        if (!Bindings.ContainsKey(-1))
        {
            Bindings[-1] = new List<IBinding>();
        }
        Bindings[-1].Add(binding);
        binding.Bind();
    }
    public void AddBinding(Action bind, Action unbind)
    {
        AddBinding(new GenericBinding(bind,unbind));
    }
    public virtual void Deserialize(JSONNode node)
    {
        CacheReflectedModelProperties();
        if (node["Identifier"] != null)
        {
            Identifier = node["Identifier"].Value;
        }
        foreach (var property in _modelProperties)
        {
            if (property.Value == null) continue;
            var val = node[property.Key];
            if (val == null) continue;
            property.Value.Deserialize(val);
        }
    }

    /// <summary>
    /// Override this method to skip using reflection.  This can drastically improve performance especially IOS
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<ModelPropertyBase> GetProperties()
    {
        CacheReflectedModelProperties();
        return _modelProperties.Values.ToArray();
    }

    /// <summary>
    /// Reflection-less get of all view-model commands generated by the designer tool.
    /// </summary>
    /// <returns></returns>
    public List<ViewModelCommandInfo> GetViewModelCommands()
    {
        var list = new List<ViewModelCommandInfo>();
        FillCommands(list);
        return list;
    }
    /// <summary>
    /// Reflection-less get of all view-model commands generated by the designer tool.
    /// </summary>
    /// <returns></returns>
    public List<ViewModelPropertyInfo> GetViewModelProperties()
    {
        var list = new List<ViewModelPropertyInfo>();
        FillProperties(list);
        return list;
    }

    /// <summary>
    /// Implementation of Microsoft's INotifyPropertyChanged
    /// </summary>
    /// <param name="propertyName"></param>
    public virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }


#if !DLL
    public virtual void Read(ISerializerStream stream)
    {
        Identifier = stream.DeserializeString("Identifier");
        var controllerName = stream.DeserializeString("Controller");
        if (controllerName != null)
        {
            var controller = stream.DependencyContainer.Resolve(stream.TypeResolver.GetType(controllerName)) as Controller;
            Controller = controller;
        }
        this.Dirty = true;
    }
    public virtual void Write(ISerializerStream stream)
    {
        stream.SerializeString("Identifier", Identifier);
        if (Controller != null)
            stream.SerializeString("Controller", Controller.GetType().AssemblyQualifiedName);
    }

    [Obsolete]
    protected ICommand Command(Action command)
    {
        return new Command(command);
    }

    [Obsolete]
    protected ICommand Command(Func<IEnumerator> command)
    {
        return new YieldCommand(command);
    }

#endif
    public void RemoveBinding(IBinding binding)
    {
        Bindings[-1].Remove(binding);
    }

    public virtual JSONNode Serialize()
    {
        CacheReflectedModelProperties();

        var node = new JSONClass();
        node.Add("TypeName", GetType().FullName);
        node.Add("Identifier", Identifier);
        foreach (var property in _modelProperties)
        {
            if (property.Value == null) continue;
            node.Add(property.Key, property.Value.Serialize());
        }
        return node;
    }

    public override string ToString()
    {
        return Serialize().ToString();
    }

    public virtual void Unbind()
    {
        foreach (var binding in Bindings)
        {
            foreach (var binding1 in binding.Value)
            {
                binding1.Unbind();
            }
            binding.Value.Clear();
        }
        Bindings.Clear();
    }


    protected virtual void FillCommands(List<ViewModelCommandInfo> list)
    {
    }

    protected virtual void FillProperties(List<ViewModelPropertyInfo> list)
    {
    }

    protected virtual void WireCommands(Controller controller)
    {
    }

    private void CacheReflectedModelProperties()
    {
        if (_modelProperties != null) return;
        var dictionary = new Dictionary<string, ModelPropertyBase>();
        foreach (KeyValuePair<string, FieldInfo> property in GetReflectedModelProperties(this.GetType()))
            dictionary.Add(property.Key, (ModelPropertyBase)property.Value.GetValue(this));

        _modelProperties = dictionary;
    }
}

public class ViewModelCommandInfo
{
    public ICommand Command { get; set; }

    public string Name { get; set; }

    public Type ParameterType { get; set; }

    public ViewModelCommandInfo(string name, ICommand command)
    {
        Name = name;
        Command = command;
    }

    public ViewModelCommandInfo(Type parameterType, string name, ICommand command)
    {
        ParameterType = parameterType;
        Name = name;
        Command = command;
    }
}

public class ViewModelPropertyInfo
{
    public bool IsComputed { get; set; }
    public bool IsCollectionProperty { get; set; }

    public bool IsElementProperty { get; set; }

    public bool IsEnum { get; set; }

    public ModelPropertyBase Property { get; set; }

    public ViewModelPropertyInfo(ModelPropertyBase property, bool isElementProperty, bool isCollectionProperty, bool isEnum,bool isComputed = false)
    {
        Property = property;
        IsElementProperty = isElementProperty;
        IsCollectionProperty = isCollectionProperty;
        IsEnum = isEnum;
        IsComputed = isComputed;
    }
}

#if DLL
}
#endif

public static class ViewModelExtensions
{
    public static void AsComputed<T>(this P<T> targetProperty, Func<T> calculate, params ModelPropertyBase[] dependantProperties)
    {
        ModelPropertyBase.PropertyChangedHandler handler = delegate(object value) {  targetProperty.Value = calculate(); };
        targetProperty.Owner.AddBinding(() =>
        {
            foreach (var property in dependantProperties)
            {
                property.ValueChanged += handler;
            }
        }, () =>
        {
            foreach (var property in dependantProperties)
            {
                property.ValueChanged -= handler;
            }
        });
        
    }
}