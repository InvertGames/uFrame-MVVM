using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using uFrame.Kernel;
using uFrame.IOC;
using UniRx;
using UnityEngine;
using uFrame.MVVM;


public class MainMenuService : MainMenuServiceBase
{

    //Inject MainMenuRoot view model with id "MainMenuRoot"
    [Inject("MainMenuRoot")] public MainMenuRootViewModel MainMenuRoot;
    [Inject("LocalUser")] public UserViewModel LocalUser;

    //Invoked when kernel is loading to prepare the service
    public override void Setup()
    {
        base.Setup();
        
        //Every time CurrentScreenType changes, invoke ChangeMainMenuScreen and pass it a new value
        MainMenuRoot.CurrentScreenTypeProperty.Subscribe(ChangeMainMenuScreen);

        //Every time new Screen is added to the Screens collection, invoke ScreenAdded and pass the new screen
        MainMenuRoot.Screens
            .Where(_ => _.Action == NotifyCollectionChangedAction.Add)
            .Select(_ => _.NewItems[0] as SubScreenViewModel)
            .Subscribe(ScreenAdded);

        //Every time user athorization state changes activate corresponding screen
        LocalUser.AuthorizationStateProperty
            .StartWith(LocalUser.AuthorizationState) //Force subscribtion to be triggered immediately with the current value
            .Subscribe(OnAuthorizationStateChanged);

    }

    private void OnAuthorizationStateChanged(AuthorizationState state)
    {
        //Just activate right screen based on authorization state`
        switch (state)
        {
            case AuthorizationState.Authorized:
                //Show menu if user is authorized
                MainMenuRoot.CurrentScreenType = typeof (MenuScreenViewModel);
                break;
            case AuthorizationState.Unauthorized:
                //Show login screen if user is unauthorized
                MainMenuRoot.CurrentScreenType = typeof (LoginScreenViewModel);
                break;
            default:
                throw new ArgumentOutOfRangeException("state", state, null);
        }
    }

    private void ScreenAdded(SubScreenViewModel screen)
    {
        //if screen is of current type, activate it; else deactivate it
        screen.IsActive = MainMenuRoot.CurrentScreenType == screen.GetType();
    }

    private void ChangeMainMenuScreen(Type screenType)
    {

        Debug.Log(string.Format("Screen type changed to {0}", screenType == null ? "null" : screenType.Name));

        //Cast to IEnumerable to avoid ambiguosity between UniRX and Collections namespaces
        var screens = MainMenuRoot.Screens as IEnumerable<SubScreenViewModel>; 

        //Find screen we want to activate
        var screen = screens.FirstOrDefault(s => s.GetType() == screenType); 
        
        //Deactivate all the screens
        screens.ToList().ForEach( s => s.IsActive = false );
        
        //If screen of matching type is found - activate is
        if (screen != null) screen.IsActive = true;
    }

    public override void RequestMainMenuScreenCommandHandler(RequestMainMenuScreenCommand data)
    {
        base.RequestMainMenuScreenCommandHandler(data);
        //Change screen to what was requested
        MainMenuRoot.CurrentScreenType = data.ScreenType;
    }
}
