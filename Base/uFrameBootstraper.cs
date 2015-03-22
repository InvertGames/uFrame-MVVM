/// <summary>
/// The uFrameBootstraper class that will configure all the required dependencies of uFrame, 
/// this can easily be overriden in your scene managers, or by simply editing the class.
/// </summary>
public static class uFrameBootstraper
{
    /// <summary>
    /// This is method is invoked with GameManagers container property is first invoked to setup the bare essential uFrame necessities.
    /// </summary>
    /// <param name="manager">The game manager instance.</param>
    /// <param name="container"></param>
    public static void Configure(GameManager manager, IGameContainer container)
    {
        // The view resolver is the class that will find a view-prefab from a view-model
        container.RegisterInstance<IViewResolver>(new ViewResolver());

    

    }
}