using uFrame.Kernel;
using UniRx;
using UnityEngine.UI;


/*
 * This is another example of using uFrameComponent.
 * In this case we listen to a specific event and show the progress.
 */
public class AssetLoadingScreen : uFrameComponent
{

    public Slider ProgressSlider;
    public Text ProgressMessage;
   
    /*
     * We use this method to subscribe to different events. Since
     * this method is invoked when kernel is loaded, we are sure
     * that EventAggregator is instantiated and ready to be used.
     * That is why we can Publish and Subscribe.
     */
    public override void KernelLoaded()
    {
        base.KernelLoaded();
     
        /*
         * Here we subscribe using inline handler represented as lambda expression
         * We set appropriate values on the slider and the text object.
         */
        this.OnEvent<AssetLoadingProgressEvent>().Subscribe(evt =>
        {
            ProgressSlider.value = evt.Progress;
            ProgressMessage.text = evt.Message;
        });
    }
}
