using System;
using System.Collections.Generic;
using System.Linq;

public class SceneContext : GameContainer
{
    private IGameContainer _container;
    private Dictionary<string, ViewModel> _viewModels;

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

    public IGameContainer Container
    {
        get
        {
            return this;
            return _container;
        }
        set { _container = value; }
    }

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

    public SceneContext(IGameContainer container, ISerializerStorage storage, ISerializerStream stream)
    {
        Container = container;
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

    public void Load(ISerializerStorage storage, ISerializerStream stream)
    {
        storage.Load(stream);
        stream.TypeResolver = new StateLoaderResolver(this);
        var viewModels = stream.DeserializeObjectArray<ViewModel>("ViewModels").ToArray();
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
}