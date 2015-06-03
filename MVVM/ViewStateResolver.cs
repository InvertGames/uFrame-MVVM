using System;
using System.Diagnostics;
using System.Linq;
using uFrame.MVVM;
using Debug = UnityEngine.Debug;
#if NETFX_CORE 
using System.Reflection;
#endif
[Obsolete]
public class ViewStateResolver : DefaultTypeResolver
{
    public SceneManager Context { get; set; }

    public ViewStateResolver(SceneManager context)
    {
        Context = context;
    }

    public override object CreateInstance(string name, string identifier)
    {
        var contextViewModel = Context.PersistantViews.FirstOrDefault(p=>p.Identifier == identifier);
        if (contextViewModel != null)
        {
            return contextViewModel;
        }
        return base.CreateInstance(name, identifier);
    }
}
[Obsolete]
public class SceneStateResolver : DefaultTypeResolver
{
    public SceneManager Context { get; set; }

    public SceneStateResolver(SceneManager context)
    {
        Context = context;
    }

    public override object CreateInstance(string name, string identifier)
    {
        var type = GetType(name);
       
#if NETFX_CORE 
        var isViewModel = type.GetTypeInfo().IsSubclassOf(typeof(ViewModel));
#else
        var isViewModel = typeof(ViewModel).IsAssignableFrom(type);
#endif
        if (isViewModel)
        {
            var contextViewModel = Context.PersistantViewModels.FirstOrDefault(p => p.Identifier == identifier);
            if (contextViewModel != null)
            {
                return contextViewModel;
            }
            return Activator.CreateInstance(type);
        }
        var view = Context.PersistantViews.FirstOrDefault(p => p.Identifier == identifier);
        if (view != null)
        {   
            return view;
        }
        return null;
    }
}