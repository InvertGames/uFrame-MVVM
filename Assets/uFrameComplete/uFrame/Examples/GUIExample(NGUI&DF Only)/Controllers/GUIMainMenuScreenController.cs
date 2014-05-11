public class GUIMainMenuScreenController : GUIMainMenuScreenControllerBase {
    
    public override void InitializeGUIMainMenuScreen(GUIMainMenuScreenViewModel nGUIMainMenuScreen) {
    }

    public override void RandomName()
    {
        base.RandomName();
        var names = new string[]
        {"Brad", "John", "Mike", "Ryan", "Bob", "Sally", "Sue", "Robert", "Abigail", "Rachael", "Betty"};

        GUIMainMenuScreen.PlayerName = names[UnityEngine.Random.Range(0, names.Length - 1)];
    }
}
