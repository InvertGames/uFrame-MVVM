using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.MVVM.Services;
using uFrame.MVVM.Bindings;
using uFrame.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.UI;



/*
 * This view serves to manage the list of available levels.
 * This view also contains some bindings in the base class (LevelSelectScreenViewBase)
 * You can enable/disable/configure those in the inspector
 */
public class LevelSelectScreenView : LevelSelectScreenViewBase
{

    /*
     * A prefab which represents level information (LevelListItem)
     */
    public GameObject LevelListItemPrefab;

    /*
     * Container which stores LevelListItems
     */
    public RectTransform LevelListContainer;

    /*
     * This is invoked every time we add new level descriptor to LevelSelectScreen.AvailableLevels collection
     */
    public override void AvailableLevelsOnAdd(LevelDescriptor levelDescriptor)
    {
        /*
         * We use naming convention: we name LevelListItems using LevelDescriptor.LevelScene property,
         * as it is unique for every level.
         * 
         * Before actually creating a new LevelListItem, we ensure that it is not already in the list
         */
        var item = LevelListContainer.FindChild(levelDescriptor.LevelScene);
        if (item != null) return;

        /* Instantiate new LevelListItem */
        var go = Instantiate(LevelListItemPrefab) as GameObject;
        item = go.transform;

        /* Parent created LevelListItem to the container */
        item.SetParent(LevelListContainer);

        /* 
         * Each LevelListItem has similar hierarchy. We can use it and setup different objects and their
         * values, based on the LevelDescriptor 
         */
        item.FindChild("LevelTitle").GetComponent<Text>().text = levelDescriptor.Title;
        item.FindChild("LevelDescription").GetComponent<Text>().text = levelDescriptor.Description;

        /* Setup the name based on LevelDescriptor.LevelScene */
        item.gameObject.name = levelDescriptor.LevelScene;

        /* Make button interactable, if level is unlocked */
        item.GetComponent<Button>().interactable = !levelDescriptor.IsLocked;

        /* 
         * MOST IMPORTANT: attach unique handler to this button, 
         * which executes command with a specific LevelDescriptor 
         */
        this.BindButtonToHandler(item.GetComponent<Button>(), () =>
        {
            ExecuteSelectLevel(levelDescriptor);
        });

    }

    /*
    * This is invoked every time we remove level descriptor from LevelSelectScreen.AvailableLevels collection
    */
    public override void AvailableLevelsOnRemove(LevelDescriptor levelDescriptor)
    {
        /* Simply remove visual representation of LevelDescriptor, if it ever exited */
        var item = LevelListContainer.FindChild(levelDescriptor.LevelScene);
        if(item!=null) Destroy(item.gameObject);
    }


    protected override void InitializeViewModel(uFrame.MVVM.ViewModel model)
    {
        base.InitializeViewModel(model);
    }

    public override void Bind()
    {
        base.Bind();
    }

}
