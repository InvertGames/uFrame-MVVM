public enum ViewModelRegistryType
{
    /// <summary>
    /// Will use the resolve method of the dependency container. If an instance isn't registered it will throw an exception.
    /// </summary>
    ResolveInstance = 1,

    /// <summary>
    /// On the views "Awake" method a Controller method will be invoked to assign the view model
    /// </summary>
    Controller = 2,

    /// <summary>
    /// Will use the resolve method creating a new instance of the dependency container but not throw an exception.
    /// </summary>
    ResolvePerObject = 3
}