using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using uFrame.MVVM;

/// <summary>
/// The scene context keeps track of view-models based on their identifiers when a view has checked "Save & Load"
/// </summary>
[Obsolete]
public class SceneContext
{
    private IUFrameContainer _container;
    private Dictionary<string, ViewModel> _viewModels;
    private Dictionary<string, ViewModel> _persitantViewModels;

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
                return null;//Container.Resolve<ViewModel>(identifier);
            }
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

    public IUFrameContainer Container
    {
        get
        {
            //return this;
            return _container;
        }
        set { _container = value; }
    }

    /// <summary>
    /// The dictionary of ViewModels currently loaded in the scene that have been marked as persistant.
    /// </summary>
    public Dictionary<string, ViewModel> ViewModels
    {
        get { return _viewModels ?? (_viewModels = new Dictionary<string, ViewModel>()); }
        set { _viewModels = value; }
    }


    public Dictionary<string, ViewModel> PersitantViewModels
    {
        get { return _persitantViewModels ?? (_persitantViewModels = new Dictionary<string, ViewModel>()); }
        set { _persitantViewModels = value; }
    }

    public SceneContext()
    {
    }

    public SceneContext(IUFrameContainer gameContainer)
    {
        Container = gameContainer;
    }

    public TViewModel CreateViewModel<TViewModel>(Controller controller, string identifier) where TViewModel : ViewModel, new()
    {
        return null;
    }

    /// <summary>
    /// Load's a set of view-models from a storage medium based on a stream.
    /// </summary>
    /// <param name="storage">This is for loading the stream from a persistant medium. e.g. File, String..etc</param>
    /// <param name="stream">The type of stream to serialize as. eg. Json,Xml,Binary</param>
    public void Load(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.DependencyContainer = Container;
        storage.Load(stream);
        //stream.TypeResolver = new ViewStateResolver(this);
        // ReSharper disable once UnusedVariable
        var vms = stream.DeserializeObjectArray<ViewModel>("ViewModels").ToArray();
        foreach (var vm in vms)
        {
            this[vm.Identifier] = vm;
        }
    }

    /// <summary>
    /// Saves 
    /// </summary>
    /// <param name="storage"></param>
    /// <param name="stream"></param>
    /// <param name="viewModels"></param>
    public void Save(ISerializerStorage storage, ISerializerStream stream, IEnumerable<ViewModel> viewModels = null)
    {
        stream.SerializeArray("ViewModels", viewModels ?? PersitantViewModels.Values);
        storage.Save(stream);
    }

    public void Clear()
    {
        ViewModels.Clear();
        PersitantViewModels.Clear();
    }
}