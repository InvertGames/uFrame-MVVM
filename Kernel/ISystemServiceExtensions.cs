using System;

public static class ISystemServiceExtensions
{
    public static T CreateViewModel<T>(this ISystemService s) where T : ViewModel
    {
        return (T)s.CreateViewModel(typeof(T));
    }

    public static ViewModel CreateViewModel(this ISystemService s, Type type, string identifier = null)
    {
        var controller = uFrameMVVMKernel.Container.Resolve<Controller>(type.Name);
        return controller.Create(identifier);
    }
}