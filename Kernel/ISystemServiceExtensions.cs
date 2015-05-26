using System;

public static class ISystemServiceExtensions
{
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
}