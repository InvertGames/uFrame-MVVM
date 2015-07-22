using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class HowItWorksTogether : uFrameMVVMPage<MVVMPage>
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
     

        _.Paragraph("So if you've made games in Unity before, you may have noticed how easy " +
                    "it is to end up with a mess of components with heavy dependencies.  Unit " +
                    "testing is impossible.  Adding/removing properties or changing the game " +
                    "logic of one component may break components on one, two, or half a dozen " +
                    "other Gameobjects and/or UI elements.  It can easily become a nightmare.");
        _.Break();
        _.Paragraph("In uFrame, an emphasis is placed on separating out the pieces of your game " +
                    "into certain categories (often referred to as \"layers\"), based on this hybrid" +
                    " MVCVM pattern.  The reasoning behind this is to help enforce separation of concerns, " +
                    "and allow you to quickly split things into these categories and think about them " +
                    "up-front.  These parts are defined as:");
        _.ImageByUrl("http://i.imgur.com/oVunJef.png");
        _.Paragraph(" - ViewModels: The objects, including properties and available commands");
        _.Paragraph(" - Controllers: The rules, where you implement logic of commands");
        _.Paragraph(" - Views: The visual/auditory representation of your objects in a game engine environment");
        _.Break();
        _.Paragraph("It gets a little more complicated with the actual implementation, but the chart above " +
                    "is the core concept.  Ideas are always theoretical.  The idea of your game and everything" +
                    " that defines it should technically be able to exist in any environment, whether it's a " +
                    "game engine, a console app, or a physical board game.  A player takes damage and health " +
                    "is decremented; this concept can be represented any number of ways, as a UI health gauge, " +
                    "a damage printout message in a console, or loss of health tokens from a board game player.");
        _.Break();
        _.Break();
        _.Paragraph(
            "In the previous example the Player would be a ViewModel, an object in your game, with a " +
            "Health property.  There would most likely be a TakeDamage command defined in the " +
            "PlayerController, which would handle the rule of decrementing the playerViewModel's Health." +
            "  When the value changes on the Health property, you may have it trigger a view binding on " +
            "the PlayerHUDView which updates a health gauge according to this new value.  The fun part is " +
            "that all it takes to trigger this chain of events is something like:");

        _.CodeSnippet("ExecuteCommand(playerVM.TakeDamage, 10); // player takes 10 damage");
        _.Paragraph("This command can be executed from any controller, or any view that has access to that " +
                    "particular PlayerViewModel instance, usually through some kind of interaction, such as a " +
                    "collision with spikes or an enemy's weapon.  Furthermore, it is important to make a " +
                    "distinction of game logic (which goes on the Controller layer of the design) and " +
                    "visual/auditory/engine-specific logic (which belongs on the View layer).");
        _.Break();
        _.Paragraph("Instead of an EnemyView detecting that it has hit the PlayerView, taking its PlayerViewModel " +
                    "instance, and executing the TakeDamage command on it directly from that EnemyView, it's " +
                    "important to make the distinction that this is game logic and belongs in the Controller " +
                    "layer.  The correct approach that most follows the MVCVM pattern would be to implement " +
                    "some kind of AttackPlayer command on the Enemy element, and pass the playerViewModel of " +
                    "the PlayerView that the EnemyView has hit.");
        _.ShowGist("2db6ead6cc89deb81a5e", "EnemyView, Hit was detected");
        _.Paragraph("Now that you're handling the game logic properly on the Controller layer, the actual logic " +
                    "is no longer dependent on that specific view, and is available to anything that tells the" +
                    " EnemyViewModel to AttackPlayer.");

        _.ShowGist("f79b3de4126c75d5bb03", "Enemy and Player Controllers Command Logic");
        _.Break();
        _.Paragraph("As you can see, there's a method to the madness.  Separating the core logic and state " +
                    "from the \"Monobehaviour\" side of things allows any number of Views to use this data. " +
                    " Under the hood, uFrame manages the state and helps centralize logic.  Everything else " +
                    "is just an expression of that state, a Player getting attacked or taking damage and " +
                    "losing health.");
    }
}

public class uFrameOfMind : uFrameMVVMPage<MVVMPage>
{
    public override decimal Order
    {
        get { return -1; }
    }

    public override string Name
    {
        get { return "uFrame Of Mind"; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
     
        base.GetContent(_);
        _.Paragraph("uFrame, specifically created for the Unity game engine, uses a pattern-based framework called MVVM (Model View ViewModel). It is designed to provide developers with the visual editing tools, code structure and knowledge to develop games faster and more efficiently. This \"frame of mind\" for creating games is different than what most Unity developers are used to, but once understood the possibilities of where an idea can go are limitless. So, let's jump right into the \"uFrame of mind\"! ");
        _.Title2("Making things simple");
        _.Paragraph("Far too often game development begins with throwing a bunch of random game objects and components together and then retrofitting code in hopes of creating a masterpiece. Teams fail to focus on the basic concepts first and are left with trying to assemble pieces of a project that don't quite fit together. Doesn't make much sense does it?");
        _.Paragraph("uFrame simplifies things by separating out the essential and peripheral parts of the Unity's component model. It eliminates the visual noise (user-interface, HUD, transformers, positions, rotations, working with geometry, shaders, pathfinders, mouse events etc.) that can overwhelm and discourage many developers, and allows them to initially focus on the core logic.");
        _.Paragraph("uFrame does this with the use of Elements . Each element in a game has different parts, which consists of the ViewModel, Controller, View and possibly the View Component. A ViewModel serves as interface between the visual entities and logic of the game. These entities include properties (Score, Ammo, etc.), collections (Players, Environments etc.), and commands (PlayerHit, Upgrade etc.). A Controller works with a ViewModel to initialize and modify data when a command is invoked and/or interaction occurs between an event and the user. ");
        _.Break();
        _.Title2("Always Consider Portability.");
        _.Paragraph("In uFrame, both the Controller and the ViewModel are \"portable\" parts of a game. This means taking them outside of Unity and putting them into a separate environment, such as a terminal application or web server, will never be an issue. In fact, this is essential to providing proper support for the extended features of uFrame and is enforced by the Element Designer (which limit the types that are available). This should always be taken into consideration when developing or extending Controllers or ViewModels. ");
        _.Note("Even if a game doesn't need portability it is still good to think in these terms. uFrame is built around this concept, which allows each piece to fit together more naturally.");
        _.Break();
        _.Title2("Making The Noise Sing.");
        _.Paragraph("The other two parts of the element, the View and the View Component, connect the game logic with the visual noise. They are the key components that connect Unity to the Controllers and ViewModels, bringing the game to life. A View Component can read and modify the real-time data of a ViewModel, but isn't necessary for every element. It is only meant to provide an extra level of extendibility when needed. A View (which is technically a View Component, but with a larger role) binds to an element's data so that it's notified when something is changed. When these changes occur, it directs the View Component to execute the appropriate action. Views also execute the commands of events and interactions that occur in the scene (e.g. Collision, MouseClick, Hover). ");
        _.Break();
        _.Title2("Naming Things.");
        _.Paragraph("When creating elements, particularly element commands, it's important to think about how things are being named. For instance, if a player can pick up an item when a game object collides with it and when the player clicks on it, then a developer would want to create a label such as \"PickUpItem\", that describes both commands. This would more broadly explain its function and allow for items to be collected in a different way, if the developer so chooses.  ");
        _.Break();
    }
}