using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                ViewModels.Add(identifier, value);
                value.Identifier = identifier;
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

    public TViewModel CreateViewModel<TViewModel>(Controller controller, string identifier) where TViewModel : ViewModel, new()
    {

        var contextViewModel = this[identifier];
        if (contextViewModel == null)
        {
            contextViewModel = new TViewModel { Controller = controller, Identifier = identifier };
            this[identifier] = contextViewModel;

        }
        contextViewModel.Controller = controller;
        return (TViewModel)contextViewModel;
    }

    public void Save(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.SerializeArray("ViewModels", ViewModels.Values.Where(p=>p.References > 0));
        storage.Save(stream);
    }

    public void Load(ISerializerStorage storage,ISerializerStream stream)
    {
        storage.Load(stream);
        stream.TypeResolver = new StateLoaderResolver(this);
        var viewModels = stream.DeserializeObjectArray<ViewModel>("ViewModels").ToArray();

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

public class StateLoaderResolver : DefaultTypeResolver
{
    public SceneContext Context { get; set; }

    public StateLoaderResolver(SceneContext context)
    {
        Context = context;

    }

    public override object CreateInstance(string name, string identifier)
    {
        var contextViewModel = Context[identifier];
        if (contextViewModel != null)
        {
            return contextViewModel;
        }
        return base.CreateInstance(name, identifier);
    }
}