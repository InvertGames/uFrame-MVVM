using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;

/// <summary>
///  A data structure that contains information/data needed for a 'View'
/// </summary>
[Serializable]
public class ViewModel : IJsonSerializable, IUFSerializable
{

    public virtual string Identifier { get; set; }

    private Dictionary<string, ICommand> _commands;
    private Dictionary<string, ModelPropertyBase> _modelProperties;

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

    public Dictionary<string, ModelPropertyBase> Properties
    {
        get
        {
            CacheReflectedModelProperties();
            return _modelProperties;
        }
    }

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

    public virtual JSONNode Serialize()
    {
        CacheReflectedModelProperties();

        var node = new JSONClass();
        node.Add("TypeName", GetType().FullName);
        node.Add("Identifier",Identifier);
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

    private void CacheReflectedModelProperties()
    {
        if (_modelProperties != null) return;
        var dictionary = new Dictionary<string, ModelPropertyBase>();
        foreach (KeyValuePair<string, FieldInfo> property in GetReflectedModelProperties(this.GetType()))
            dictionary.Add(property.Key, (ModelPropertyBase)property.Value.GetValue(this));

        _modelProperties = dictionary;
    }

    public virtual void Write(ISerializerStream stream)
    {
        stream.SerializeString("Identifier",Identifier);
    }

    public virtual void Read(ISerializerStream stream)
    {
        Identifier = stream.DeserializeString("Identifier");
    }
}