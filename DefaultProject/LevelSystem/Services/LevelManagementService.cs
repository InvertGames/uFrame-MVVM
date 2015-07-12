using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using UniRx;
using uFrame.Kernel;
using uFrame.MVVM;
using UnityEngine;

/*
 * This service introduces example of interesting database.
 * Check the kernel how LevelDescriptors live on a certain GameObject and this service reads them.
 */
public class LevelManagementService : LevelManagementServiceBase
{
    private List<LevelDescriptor> _levels;

    //Game object holding LevelDescriptor components 
    public GameObject LevelsContainer;    
    
    // This list will hold all the registered levels
    // You can add level descriptors dynamically by adding new LevelDescriptor
    // component on the service object and calling UpdateLevels
    public List<LevelDescriptor> Levels
    {
        get { return _levels ?? (_levels = new List<LevelDescriptor>()); }
        set { _levels = value; }
    }

    public override void Setup()
    {
        base.Setup();
        //On setup register levels initially
        UpdateLevels();
    }

    private void UpdateLevels()
    {
        var levelDescriptorComponents = LevelsContainer.GetComponents<LevelDescriptor>().Except(Levels); //Get all non registered level descriptors
        Levels.AddRange(levelDescriptorComponents); //Add those to the list of registered levels
    }
}
