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