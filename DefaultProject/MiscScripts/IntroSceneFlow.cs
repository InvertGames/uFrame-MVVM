using UnityEngine;
using System.Collections;
using uFrame.Kernel;

/*
 * This is an example of how to use uFrameComponent.
 * uFrameComponent is generally just a monobehaviour connected with the
 * uFrame infrastructure using EventAggregator. As a result, you can
 * Publish and Subscribe to events. Please note, that uFrameComponent
 * is extremely simple, so you can refere to the source code and recreate similar
 * functionality in your own monobehaviour-based classes.
 */
public class IntroSceneFlow : uFrameComponent
{

    /* Sprite to be animated */
    public SpriteRenderer Logo;

    /* Animation progress (transparency in our case) */
    private float progress;

    /* This method is invoked when uFrame infrastructure is ready: 
     * SystemLoaders and SceneLoaders are registered, Services are set up and ready;
     * Simply speaking: kernel is loaded.
     */
    public override void KernelLoaded()
    {
        base.KernelLoaded();
        StartCoroutine(ShowIntro());
    }

    /*
     * Coroutine with animation, notice that it publishes event in the end
     */
    private IEnumerator ShowIntro()
    {
        
        //Make all the fancy stuff happen
        var solidColor = Logo.color;
        var transparentColor = new Color(solidColor.r,solidColor.g,solidColor.b,0);
        Logo.color = transparentColor;

        while (progress < 1f)
        {
            Logo.color = Color.Lerp(transparentColor, solidColor, progress);
            progress += 0.5f*Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (progress > 0f)
        {
            Logo.color = Color.Lerp(transparentColor, solidColor, progress);
            progress -= 0.5f * Time.deltaTime;
            yield return null;
        }


        //In the end let the system know that intro is finished
        Publish(new IntroFinishedEvent());
    }




}
