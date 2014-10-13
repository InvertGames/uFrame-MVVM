using System;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

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
        var isViewModel = typeof (ViewModel).IsAssignableFrom(type);

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