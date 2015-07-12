using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.IOC;
using UniRx;
using UnityEngine;
using uFrame.MVVM;


public class AssetLoadingService : AssetLoadingServiceBase {


    /*
     * This method handles StartAssetLoadingCommand. 
     * Even though StartAssetLoadingCommand is just another event, it has a bit different name.
     * This is because we use this event to invoke asset loading procedure. Thus, Command is 
     * an appropriate name for such an event.
     */
    public override void StartAssetLoadingCommandHandler(StartAssetLoadingCommand data)
    {
        base.StartAssetLoadingCommandHandler(data);
        StartCoroutine(LoadAssets());
    }

    /*
     * This coroutine simulates asset loading. You can substitute this method to actually load
     * something from disk/cloud/external service. Please notice, that we publish progress event.
     * So, any part of the application intrested in the progress can subscribe to this event 
     * and perform additional logic.
     */
    private IEnumerator LoadAssets()
    {
       
        for (int i = 0; i < 100; i++)
        {
            Publish(new AssetLoadingProgressEvent()
            {
                Message = string.Format("Loaded {0}% of game assets...", i),
                Progress = i/100f
            });
            yield return new WaitForSeconds(0.03f);
        }

        /*
         * Ensure, that we publish "1f progress" event with a different message, after we finish.
         */
        Publish(new AssetLoadingProgressEvent()
        {
            Message = "Loaded 100% of game assets!",
            Progress = 1f
        });

    }

}
