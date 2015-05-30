using System;

public static class MVVMKernelExtensions
{
    public static void RegisterViewModel<TViewModel>(this IGameContainer container, TViewModel viewModel, string identifier) where TViewModel : ViewModel
    {
        container.Register<TViewModel, TViewModel>();
        container.RegisterInstance<ViewModel>(viewModel, identifier);
        container.RegisterInstance(typeof(TViewModel), viewModel, identifier);
    }

    public static void RegisterController<TController>(this IGameContainer container, TController controller) where TController : Controller
    {
        container.RegisterInstance<Controller>(controller, controller.GetType().Name, false);
        container.RegisterInstance<ISystemService>(controller, controller.GetType().Name, false);
        container.RegisterInstance<TController>(controller, false);
        // Todo Convention hack make it prettier :)
        container.RegisterInstance<Controller>(controller, typeof(TController).Name.Replace("Controller", "ViewModel"));
    }

    public static void RegisterViewModelManager<TViewModel>(this IGameContainer container,
        IViewModelManager<TViewModel> manager)
    {
        container.RegisterInstance<IViewModelManager>(manager, typeof(TViewModel).Name.Replace("ViewModel", ""));
        container.RegisterInstance<IViewModelManager>(manager, typeof(TViewModel).Name);
        container.RegisterInstance<IViewModelManager<TViewModel>>(manager, typeof(TViewModel).Name.Replace("ViewModel", ""));
        container.RegisterInstance<IViewModelManager<TViewModel>>(manager);
    }

    public static void RegisterViewModelController<TController, TViewModel>(this IGameContainer container, TController controller) where TController : Controller
    {

    }

    public static T CreateViewModel<T>(this ISystemService s, string identifier = null) where T : ViewModel
    {
        return (T)s.CreateViewModel(typeof(T), identifier);
    }

    public static ViewModel CreateViewModel(this ISystemService s, Type type, string identifier = null)
    {
        var controller = uFrameMVVMKernel.Container.Resolve<Controller>(type.Name);
        if (controller == null)
        {
            throw new Exception("Controller could not be found.  Make sure your subsystem loader has been attached to the kernel.");

        }
        return controller.Create(identifier ?? Guid.NewGuid().ToString());
    }

    public static T CreateViewModel<T>() where T : ViewModel
    {
        return (T)CreateViewModel(typeof(T));
    }

    public static ViewModel CreateViewModel( Type type, string identifier = null)
    {
        var controller = uFrameMVVMKernel.Container.Resolve<Controller>(type.Name);
        if (controller == null)
        {
            throw new Exception("Controller could not be found.  Make sure your subsystem loader has been attached to the kernel.");
        }
        return controller.Create(identifier ?? Guid.NewGuid().ToString());
    }

    public static TViewModel CreateInstanceViewModel<TViewModel>(this ISystemLoader loader, string identifier) where TViewModel : ViewModel
    {
        var controller = uFrameMVVMKernel.Container.Resolve<Controller>(typeof(TViewModel).Name);
        return (TViewModel)controller.Create(identifier);
    }
}