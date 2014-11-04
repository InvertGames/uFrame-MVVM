[u]Frame Release Notes
Be sure to subscribe to our newsletter @ invertgamestudios.com for updates, notifications, and cool info.
1.5

Overview

uFrame 1.5 is out. Even thought is a minor version step, there are a lot of new exciting features,
 changes and bugfixes. This version addresses almost all of the issues in 1.4. Let's see what's new:

Editor and Project:

1.5 brings a completely new theme, with new cool UI features that make editor more responsive.
 Not only it looks more professional, but the usability is greatly improved, for example
 highlighted connections or handy Inputs and Outputs for the diagram items.

Thanks to the new project system, you can now seprate your project into as many graphs as you want, keeping it
 all under control. New workflow features, project level namespaces, and even code generation settings have arrived!

Framework:

  RX
    In 1.5 you will be able to unleash the power of Reactive Extensions!
     UniRX implementation of RX is now shipped with uFrame.
     Almost everything now is observable: viewmodels, properties, commands, collections and more!
     Reactive extension methods are integrated seamlessly into your code and definitely will save you
     incredible amount of time.
     Binding have been completely reworked to follow the RX model.

  Computed Properties
    1.5 introduces computed properties. They are just properties which depend on other properties.
     Simple and powerful. You can express dependency from the same element or from child elements.
     They can be dependent on regular properties, scene properties or other computed properties.
     They can serve as triggers for state machines.
     In your code you can easily modify all the dependencies by simply overriding a special method.

  Reactive State Machines:
    1.5 brings brand new Reactive State Machines directly into your diagrams.
     You can rapidly design states, transitions and triggers.
     Then you can generate view bindings for the states with a few mouse clicks. Finally
     you can easily setup any additional logic in the code.
     uFrame State Machines do not use any dirty checks! This can save a lot of performance
     to let other amazing things run inside your game.
     You can easily debug your states right in the diagram or from the View Inspector.

  Class Nodes:
    Now you can easily create regular classes right inside of your diagram.
     You can use those as property types or command argument types
     Generated class automagically implements INotifyPropertyChanged interface, which makes it
     compatible with other cool assets, libraries and frameworks

    Moreover, you can now use any type which does not inherit from Unity Object as a type for ViewModel property
     or command argument.

  Registered Instances:
    Subsystems now can export instances of ViewModels which will be shared around you application.
     No more problems with transferring small pieces of data around your scenes. It's all done automagically
     and only requires a few click in the diagram editor!

    It also plays nicely with the inheritance, allowing you to
     substitute certain instances with different implementations.

    Finally, you can register several instances under different names,
     and instantly get access to those in your controllers!

Major Changes:

  SIE

    No more single instance elements!
     Since you can now register any ViewModel instance in the subsystem,
     you have ultimate control over your shared instances. That's why Single Instance Elements are removed from uFrame.
     This also brings consistency into controller code, as now every command receives a sender.

  Scene Transitions:
    Scene transitions have been reworked to become as user-friendly as possible.
     You can still use inspector to define linear transition logic.
     But as soon as it comes to transitions based on command arguments / shared instances data,
     you can easily do it in the code, by just overriding a couple of methods.
     You can set scenes and scene manager settings with any condition you want.

  Scene Persistence:
    Save'n'Load functionality was greatly solidified. Now every user has an ability
     to implement saving and loading without worrying too much about the low-level functionality.
     ViewModel saving/loading is already there. But you also have an ability persist some of your view data
     like transforms, states and so on.
     You can access Read and Write methods in your View and operate over any data you want.

  2-Way-Bindings
    2-Way-Bindings were completely reworked and are now called Scene Properties.

    Those little handy things allow you to access specific View information, like positions and distances.
     Scene properties rely heavily on Observables. This means you get exceptional control over you performance.
     You define how and when those things are calculated. The rest is automagically done by uFrame.

  1.5 includes lots of other changes and bug fixes. And even more will come in the future versions. So stay tuned!

1.41 ---
Important Note: Element Inheritance,Property Types,Collections Types, Command Types and View links may be broken.  Double-check that they are correctly wired together.
Added: Computed properties on view-models and controllers. Right-Click and choose "Computed By->{PropertyName}" on a Element Property.


1.406 8/13/2014
Added: Scrollbar on Binding's window.
Fixed: Initialize{ElementName} not being called when using {ElementName}Controller.Create{ElementName}();
Fixed: Removed "YUPYUPYUP" debug log message when removing a binding :)
Fixed: When transitioning to and from another scene single instance view-models wern't resetting.
Fixed: Default Identifier for multiple single instance elements that where derived from a specific type was causing issues.
Fixed: Settings Colors where off when creating a new diagram
Added: When creating an new node it auto selects it for editing.
Added: When creating an item it auto selects it for editing.
Added: Keyboard shortcuts for Saving, Adding nodes, deleting nodes, and deleting items.


1.401 8/5/2014
Fixed: Scaling the diagram down caused element properties to exceed the width of the box.
Fixed: Unlinking of ViewComponents not working.
Fixed: Two-Way Properties on views not generating correctly.

1.4 8/5/2014
Important: When importing the new package ( you  may need to remove the old first. ) Be sure to open your diagram and click on 'Save & Compile'.
Important: see upgrading to 1.4 tutorial http://invertgamestudios.com/home/video-tutorials/upgrading-to-1-4
Note: uFrame Playmaker Plugin is obsolete.
   After making a very tough decision to deprecate the Playmaker plugin we felt with the new features it could be drastically optimized and more seemless. 
   If you are currently dependent on the old playmaker plugin and still want to upgrade you'll need to map View-Model properties directly to fsm's manually when properties change.  
   We feel that having a diagram node for FSM templates and using links to keep things synchronized will drastically improve the workflow when working with Playmaker.

Feature: View-Model property editing at run-time.
Feature: View Bindings in diagram.
  This feature drastically increases the workflow of uFrame. You now longer have to worry about checking bindings. Just add the binding in the diagram and it will insert the code directly into the view code.
Feature: Two-Way Properties in views.
Feature: Add properties from any component in the project directly onto a view in the designer. This will automatically wire the properties up and create the View-Model serialization code for you.
Feature: Scene State Saving and Loading with new generic serialization classes (Currently in beta).
Feature: View Properties for data persistance and Element References.
Feature: See all registered ViewModels in the SceneContext under the SceneManager inspector at run-time.
Feature: Visibility into the dependency container VIA GameManager Inspector
Feature: Code Generators now generate serialization code for ViewModels using uFrames new generic serializer implementation.
Feature: Generic Scene State Serialization (Beta)
Fixed: ArgumentException: Getting control 2's position in a group with only 2 controls when doing KeyDown error on rename.
Change: View-Models now contain a reference to a controller. This removes the responsibility from the controller and makes the framework work with View-Models and controllers in a more cohesive manner.
Change: Moved Controller WireCommands to ViewModels
Fix: When an element property is on a element with a short name the delete button can't be clicked.
Fix: UnityVS plugin now uses "Visual Studio Tools/Generate Project Files"


1.284 
Fixed: Controller's ExecuteCommand method sending a null sender.  So multi-instance element didn't received the correct reference.


1.283 7/13/2014
Fixed: Undo not reload correctly
Fixed: Enums Generating their own file.  (You may need to delete {EnumName}.designer.cs)

1.282 7/12/2014
Fixed: Duplicates on Undo
Fixed: View Inheritance issues
Feature: Property, Collection, & Command re-ordering.
Added: Theme Updates
Fixed: Links pointing an element now point to the top of the element rather than the middle.

1.28 7/6/2014
Feature: Playmaker Plugin
Feature: Save diagrams as json
Save the diagram as JSON format to support custom node plugins, better version control, and external export/import. User should be able to easily move from to the new format easily. Show a notification when diagram is loaded to convert. Will not destroy the current diagram.
Added: Delete items with the delete key
Fixed: When removing an Enum it doesn't remove the type from a property.
Added: Major performance enhancemenets.
Added: New Settings dialog for Diagrams
Added: Command buttons to the View's Insepctor
Fixed: Missing "Has Multiple Instances" on element right-click menu.
Added: Subsystem Folder structure option. via Settings button in the Designer toolbar.

1.26 6/20/2014
Fixed: Annoying error when clicking add on a node that is not selected adds to the wrong element.
Fixed: SceneManager Load method being invoked twice when switching scenes.
Updated: When registering named viewmodels via "Force Resolve" it registers for the type "ViewModel" and the actual instance type.
Added: Plugin Manager to turn plugins on and off via the Top Menu Tools->uFrame->Plugins.
Added: Default bindings for creating a custom loading screen.  Derive from LevelLoaderView and override StatusChanged, and ProgressChanged. (Tutorial coming soon.)
Updated: Other minor optimizations and performance enhancements.

1.25 6/15/2014
Fixed: View Classes not working as expected
Fixed: A small bug in ViewBase class.
Updated: Inspector titlebar for inspectors.
Change: You now have to set the start scenemanager in the game manager.
Added: A new NGUI demo project showing of Templates, and Setting a View Base class.
Fixed: Other small bugs

1.2 6/12/2014
Added/Fixed: Unity 4.5 Support
Added: Diagram plugin support. Extend just about anything.  Look at (uFrameComplete/uFrame/Editor/DiagramPlugins) for details.
Added: UnitVS plugin to auto sync project files on save.
Added: "Template Elements" which makes them abstract. Right click an element and choose "Is Template";
Added: View base class selection.  Just right-click a view and choose "View Base/{Name Of Base View}".
Added: Command object model for plugins and easy feature additions.
Added: Dependency injection class relationships.
Added: Command hooks.  Now any command (Toolbar,ContextMenu) can be hooked to execute additional actions.
Modified: Scene transitions to work better and play happy with Navigation system.
Modified: Theme for a more polished look and feel.
Modified: Dependency injection RegisterInstance has some reordered parameters to illeminate confusion and odd overloads.
Modified: Dependency injection mappings. 
Modified: File repositories to support future file formats such as json.
Modified: View Inspector's play mode to show better results for element properties.  Still needs some polishing.
Modified: Refactored code generation pieces to easily create plug-ins for third-party assets.
Fixed: Major Performance enhancements.
Fixed: Link colors being random colors. (Note diagram colors will reset)
Fixed: Mac path problems causing annoying error messages.
Fixed: Collapsing not refreshing the link positions.
Fixed: A small bug in the controller where sometimes a Resolved ViewModel would return a new instance.
Fixed: Recompiling makes the current diagram the current Unity selection.
Fixed: Some small sizing issues.
Fixed: Panning to the edge of the diagram caused selection to occur.

1.06 5/10/2014
Fixed: Heirarchy Icons going missing
Fixed: When creating views that are not connected they won't be generated as an empty file.
Fixed: You can't delete items that contain other items.  You must delete the "inner" items first.
Fixed: When removing things it correctly removes the associations so that cascading errors don't occur. This includes Views still being generated
Fixed: Add to scene on scene managers not working correctly
Fixed: Some errors that occur from other non uFrame corrupted assets when polling for diagrams.
Fixed: Other minor issues and fixes
Modified: Now you can use ForceResolve directly from a view without manually adding the named injection.
Modified: Controllers now have a GetByName and GetByType which can be overriden and are used by views.
Modified: SceneManagers no longer initialize single instance types.  So a at least one view should check InitializeViewModel in the inspector

1.04 5/4/2014
IMPORTANT!!!: Since we re-released the source code you will need to reconnect all GameManagers in you're scene.
IMPORTANT!!!: Make sure you delete the uFrameComplete folder then re-import.
Note: Refactoring doesn't apply to changing type parameters on commands or enum items yet.
Note: Now source code for uFrame is available again
Added: New refactoring library added for renaming anything
Added: New skin for a better look and feel
Added: Panning via thie middle mouse button
Added: Zooming (This will be improved in later builds)
Modified: SceneManager Settings files are now partial so they can be extended.
Modified: You no longer have to use shift for multi-selection.
Fixed: A lot of ViewComponent link issues
Fixed: Unity Free skin where things were hard to see
Fixed: Where deleting a view wouldn't delete the view file
Fixed: Collapsed states not being persisted correctly between sub diagrams
Fixed: Switches to LoadLevelAdditive if you don't have a pro license.
Fixed: iOS Mobile issues
Fixed: Diagram Loading issues causing unity to crash or not able to load when Unity starts
Fixed: Collapsing link issue

1.0Beta 4/27/2014
Added an all new element designer that generates SceneManagers(GameTypes), Controllers, ViewModels, Views, & View Components.
Add a whole new action behaviour system called uBehaviours for quick one-off components.
Integration of uBehaviours and the new diagram system.
Rename GameType to SceneManager.
Watch ViewModels from the inspector
Initialize ViewModels from the inspector (Element Designer generates this)
Automatic Bindings generation.
Controllers have been removed from the scene the Element designer will automatically Dependency inject them.
Simplified bindings via the Element Designer
Updated Checkers Demo (To Demonstrate Diagrams and uBehaviours)
Update FPS Demo
Loading screen changed to just a view in a scene and nothing more.
Dependency containers are no longer cleared when moving from scene to scene.
Tons of other fixes and improvements.

0.99r1 3/8/2014
Added IBindingProvider interface so that other components can directly connect to a View Components Bind & Unbind methods.
Removed the class constraints on Container.Resolve & Container.RegisterInstance.
Add a User Submitted Cheat Sheet diagram.
Other Minor Fixes/Improvements.

0.99 2/4/2014
 Added A single ViewModel to View Binding via 
	// View Class
	public SingleView ChildView {get;set;}
	// In Bind method
	this.BindToView(() => Model._SingleViewModelProperty, v => ChildView = v, () => ChildView);
 Bug Fix: Now in objects where the Inject attribute is used and there isn't a registered instance it will keep the value at null.
 Important Note: Now when commands are Executed by default it passes the viewmodel along with it. 

Used to look like this.-------------
    public virtual TWeaponViewModel Create<TWeaponViewModel>() where TWeaponViewModel : FPSWeaponViewModel, new()
    {
        var fpsWeapon = new TWeaponViewModel();
        fpsWeapon.NextZoomCommand = new CommandWith<FPSWeaponViewModel>(NextZoom);
        fpsWeapon.FireCommand = new YieldCommandWith<FPSWE>(() =>
        {
            fpsWeapon.Spread += _SpreadMultiplier;

            if (fpsWeapon.IsFiring)
                return null;

            return Fire(fpsWeapon);
        });
        fpsWeapon.RecoilAmount = _RecoilSpeed;
        fpsWeapon.ReloadCommand = new YieldCommand(() => { return Reload(fpsWeapon); });
        fpsWeapon.Spread = _Spread;
        fpsWeapon.Ammo = _RoundSize * 3;
        fpsWeapon.State = FPSWeaponState.Active;
        return fpsWeapon;
    }

You can now do this--------------------------------------------------------------
    public virtual TWeaponViewModel Create<TWeaponViewModel>() where TWeaponViewModel : FPSWeaponViewModel, new()
    {
        return new TWeaponViewModel
        {
            NextZoomCommand = new CommandWith<FPSWeaponViewModel>(NextZoom),
            FireCommand = new YieldCommandWith<FPSWeaponViewModel>(FireWeapon),
            RecoilAmount = _RecoilSpeed,
            ReloadCommand = new YieldCommandWith<FPSWeaponViewModel>(Reload),
            Spread = _Spread,
            Ammo = _RoundSize*3,
            State = FPSWeaponState.Active
        };
    }

0.98r1 2/2/2014
Bug Fix: Child Views will sometimes have a Model that is null causing an exception in Bind.
Examples: Updated some example code on the FPS Game.

0.98 2/1/2014
Updated View Editor.
Fixed Templates for SceneManager.
Seperated templates for Generating a single file or generating multiple files via "New Element Structure" or "View Tools"
Generating a View Prefab now automatically sets the "ViewModel From: to Controller and method that is generated.
Fixed Window Hanging on Code Generation.
Modified the Level Loading Algorithm to be more direct of the value passed in UpgradeProgressDelegate. (Thanks killamaaki)
Fixed Controller Method Issue on Views where two Views in the scene caused a controller not to register.

0.97 1/30/2014
Minor Editor Bug Fixes

0.96 1/29/2014
InitializeModel method was renamed to InitializeViewModel ( BREAKING CHANGE APPLY RENAME WHERE USED! )
Game class renamed to SceneManager ( Marked obsolete to not breaking anything )
vmp snippet fixed and creates properties named _NameProperty now instead of _Name
Added the ability to specify a controller & method that will be used to create a ViewModel when View's already exist in a scene.
Fixed a GUI Styles Warning ( Thanks Shawn! )

0.95 1/24/2014
Added FPS Demo
You can now use ExecuteCommand to execute commands with a parameter
Added the ability to set parameters on command bindings.
Added the ability for collision event bindings to be subscribed to with a GameObject as the parameter of the collider.
Fixed Automatic controller injection where a controller that has multiple other controller properties being injected.

0.92 1/19/2014
	- Fixed a Property Binding Component where the Model Property wouldn't be set until the target component was set.

0.9 1/12/2014
	- Initial Release

For Support Questions Contact:

Micah Osborne
invertgamestudios@gmail.com