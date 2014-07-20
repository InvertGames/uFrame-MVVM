using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SceneContext : GameContainer
{
    private Dictionary<string, ViewModel> _viewModels;
    private IGameContainer _container;

    public Dictionary<string, ViewModel> ViewModels
    {
        get { return _viewModels ?? (_viewModels = new Dictionary<string, ViewModel>()); }
        set { _viewModels = value; }
    }

    public SceneContext()
    {
    }

    public SceneContext(IGameContainer gameContainer)
    {
        Container = gameContainer;
    }

    public SceneContext(IGameContainer container,ISerializerStorage storage, ISerializerStream stream)
    {
        Container = container;

    }

    public IGameContainer Container
    {
        get
        {
            return this;
            return _container;
        }
        set { _container = value; }
    }

    public ViewModel this[Type type]
    {
        get
        {
            if (ViewModels.ContainsKey(type.Name))
            {
                return ViewModels[type.Name];
            }
            return Container.Resolve(type) as ViewModel;
        }
        set { ViewModels[type.Name] = value; }
    }
    public ViewModel this[string identifier]
    {
        get
        {
            if (!ViewModels.ContainsKey(identifier))
            {
                return Container.Resolve<ViewModel>(identifier);
            };
            return ViewModels[identifier];
        }
        set
        {
            if (!ViewModels.ContainsKey(identifier))
            {
                Debug.Log("Added " + identifier + value.GetType().Name);
                ViewModels.Add(identifier, value);
                value.Identifier = identifier;
            }
            else
            {
                ViewModels[identifier] = value;
            }
        }
    }

    public override void RegisterInstance(Type baseType, object instance = null, string name = null, bool injectNow = true)
    {
        //if (typeof(ViewModel).IsAssignableFrom(baseType))
        //{
        //    if (name != null)
        //    {
        //        this[name] = instance as ViewModel;
        //    }
        //}
        base.RegisterInstance(baseType, instance, name, injectNow);
    }

    public void Save(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.SerializeArray("ViewModels", ViewModels.Values);
        storage.Save(stream);
    }

    public void Load(ISerializerStorage storage,ISerializerStream stream)
    {
        storage.Load(stream);
        var viewModels = stream.DeserializeObjectArray<ViewModel>("ViewModels");

        foreach (var viewModel in viewModels)
        {
            if (ViewModels.ContainsKey(viewModel.Identifier))
            {
                ViewModels[viewModel.Identifier] = viewModel;
            }
            else
            {
                ViewModels.Add(viewModel.Identifier, viewModel);
            }
            
        }
    }
}

public interface ISerializerStorage
{
    void Save(ISerializerStream stream);
    void Load(ISerializerStream stream);
}

public class FileSerializerStorage : ISerializerStorage
{
    public string Filename { get; set; }

    public FileSerializerStorage(string filename)
    {
        Filename = filename;
    }

    public void Save(ISerializerStream stream)
    {
        File.WriteAllBytes(Filename,stream.Save());
    }

    public void Load(ISerializerStream stream)
    {
        stream.Load(File.ReadAllBytes(Filename));
    }
}
public class StringSerializerStorage : ISerializerStorage
{
    public string Result { get; set; }

    public void Save(ISerializerStream stream)
    {
        Result = Encoding.UTF8.GetString(stream.Save());
    }

    public void Load(ISerializerStream stream)
    {
        stream.Load(Encoding.UTF8.GetBytes(Result));
        
    }

    public override string ToString()
    {
        return Result;
    }
}

