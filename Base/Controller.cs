using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

#if DLL
namespace Invert.Common.MVVM
{
#endif

public interface ISystemService
{
    IEventAggregator EventAggregator { get; set; }

    /// <summary>
    /// The setup method is called when the controller is first created and has been injected.  Use this
    /// to subscribe to any events on the EventAggregator
    /// </summary>
    void Setup();
    
}

public abstract class SystemService : ISystemService
{
    [Inject]
    public IEventAggregator EventAggregator { get; set; }

    public abstract void Setup();
    public virtual void Dispose()
    {
        
    }
}
public static class SystemControllerExtensions
{
    public static IObservable<TEvent> OnEvent<TEvent>(this ISystemService systemController)
    {
        return systemController.EventAggregator.GetEvent<TEvent>();
    }

    public static void Publish(this ISystemService systemController, object eventMessage)
    {
        systemController.EventAggregator.Publish(eventMessage);
    }
}

/// <summary>
/// A controller is a group of commands usually to provide an abstract level
/// </summary>
public abstract class Controller : SystemService
{
    private List<ViewModel> _viewModels = new List<ViewModel>();

    public List<ViewModel> ViewModels
    {
        get { return _viewModels; }
        set { _viewModels = value; }
    }


    /// <summary>
    /// The dependency container that this controller will use
    /// </summary>
    public IGameContainer Container { get; set; }


    protected Controller()
    {
        //throw new Exception("Default constructor is not allowed.  Please regenerate your diagram or create the controller with a SceneContext.");
    }

    /// <summary>
    /// Create a new ViewModel. This will generate a Unique Identifier for the VM.  If this is a specific instance use the overload and pass
    /// an identifier.
    /// </summary>
    /// <returns></returns>
    public virtual ViewModel Create()
    {
        var result =  Create(Guid.NewGuid().ToString());
        EventAggregator.Publish(new ViewModelCreatedEvent() {ViewModel = result });
        return result;
    }

    /// <summary>
    /// Creates a new ViewModel with a specific identifier.  If it already exists in the SceneContext it will return that instead
    /// </summary>
    /// <param name="identifier">The identifier that will be used to check the context to see if it already exists.</param>
    /// <returns></returns>
    public virtual ViewModel Create(string identifier)
    {

        var vm = CreateEmpty(identifier);
        vm.Identifier = identifier;
        return vm;
    }

    /// <summary>
    /// Create an empty view-model with the specified identifer. Note: This method does not wire up the view-model to this controller.
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns>A new View-Model or the view-model found in the context with the same identifier.</returns>
    public virtual ViewModel CreateEmpty(string identifier)
    {
        var vm = CreateEmpty();
        vm.Identifier = identifier;
        return vm;
    }

    /// <summary>
    /// Create an empty view-model . Note: This method does not wire up the view-model to this controller and only instantiates an associated view-model.
    /// </summary>
    /// <returns>A new View-Model or the view-model found in the context with the same identifier.</returns>
    public virtual ViewModel CreateEmpty()
    {
        throw new NotImplementedException("You propably need to resave you're diagram. Or you need to not call create on an abstract controller.");
    }

    [Obsolete("Game event is not longer used for transitions.  Regenerate your diagram.")]
    public void GameEvent(string name) { }

    /// <summary>
    /// The setup method is called when the controller is first created and has been injected.  Use this
    /// to subscribe to any events on the EventAggregator
    /// </summary>
    public override void Setup()
    {
        
    }

    public virtual void Initialize(ViewModel viewModel)
    {
        
    }

    [Obsolete("Use Publish")]
    public void ExecuteCommand(ICommand command, object argument)
    {
        //CommandDispatcher.ExecuteCommand(command, argument);
    }
    [Obsolete("Use Publish")]
    public virtual void ExecuteCommand(ICommand command)
    {
        //CommandDispatcher.ExecuteCommand(command, null);
    }
    [Obsolete("Use Publish")]
    public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument)
    {
        //CommandDispatcher.ExecuteCommand(command, argument);
    }

    public virtual void DisposingViewModel(ViewModel viewModel)
    {
        
    }
    

};

public interface IViewModelManager : IEnumerable<ViewModel>
{
    void Add(ViewModel viewModel);
    void Remove(ViewModel viewModel);
}

public interface IViewModelManager<T> : IViewModelManager
{
    
}

public class ViewModelManager<T> : IViewModelManager<T> where T : ViewModel
{
    private List<T> _viewModels = new List<T>();

    public List<T> ViewModels
    {
        get { return _viewModels; }
        set { _viewModels = value; }
    }

    public IEnumerator<ViewModel> GetEnumerator()
    {
        return ViewModels.Cast<ViewModel>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ViewModel viewModel)
    {
        ViewModels.Add((T)viewModel);
   
    }

    public void Remove(ViewModel viewModel)
    {
        ViewModels.Remove((T)viewModel);
 
    }

   
}

public class ViewModelCreatedEvent
{
    public ViewModelCreatedEvent()
    {
    }

    public ViewModel ViewModel { get; set; }
    
}
public class ViewModelDestroyedEvent
{

    public ViewModel ViewModel { get; set; }

}
#if DLL
}
#endif