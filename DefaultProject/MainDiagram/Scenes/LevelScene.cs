using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * This is a script which defines SCENE TYPE. In this case it is LevelScene.
 * You define scene type for the scene by attaching such script to the root of the scene.
 * You can add any propeties specific for the Scene instance to this class
 * You can modify LevelSceneLoader class to change how such scene loads.
 */
public class LevelScene : LevelSceneBase
{
    /*
     * An ID to be used to get meta information for this level 
     */
    public int Id;

    
    private LevelRootViewModel _levelRoot;

    /*
     * When requested, finds LevelRootView in the hierarchy, extracts the viewmodel and caches it
     */
    public LevelRootViewModel LevelRoot
    {
        get { return _levelRoot ?? (_levelRoot = GetComponentInChildren<LevelRootViewBase>().LevelRoot); }
        private set { _levelRoot = value; }
    }
}
