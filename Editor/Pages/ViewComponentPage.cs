using System;
using Invert.Core.GraphDesigner;

namespace Invert.uFrame.MVVM {
    public class ViewComponentPage : ViewComponentPageBase {
        public override Type ParentPage
        {
            get { return typeof (ViewPage); }
        }

        public override string Name
        {
            get { return "View Components"; }
        }

        public override decimal Order
        {
            get { return 1; }
        }

        public override void GetContent(IDocumentationBuilder _) {
            base.GetContent(_);
            _.ImageByUrl("http://i.imgur.com/2G5Amgx.png");
            _.Paragraph("ViewComponents can be used for a wide variety of things, and are meant to provide a further level of extensibility to Views.  They are derived from MonoBehaviour, and require a View in order to function.  A ViewComponent will bind to the same ViewModel instance to which its corresponding View is bound.  In essence, they are extremely simplistic, offering these things:");
            _.Paragraph(" - Access to a particular View to which it is bound ");
            _.Paragraph("   - Access to that specific ViewModel instance");
            _.Paragraph(" - Matching execute methods for the corresponding View's commands");
            _.Paragraph(" - Awake, Bind, and Unbind methods ");
            _.Paragraph("   - These can be used to implement manual bindings where desired, among other things");
            _.Break();
            _.Paragraph("While entirely optional, there are a lot of creative uses for ViewComponents, including interchangeable behaviours, toggleable logic, and even extending logical ownership for a View.");
            _.Break();
            _.Paragraph("For example, we'll outline a scenario where you wish to have a system that detects hit damage to specific player body parts in an FPS game, in order to have damage multipliers.  In the example diagram below, we have separated out ViewComponents for each type of body part.");
            _.ImageByUrl("http://i.imgur.com/wNxFcQt.png");
            _.Paragraph("On the Player GameObject, we would attach the PlayerView.  Assuming that we have a rigged character model parented to that Player GameObject, we would want to set up Colliders on the joint references of each of the corresponding body parts, similar to the screenshot below:");
            _.ImageByUrl("http://i.imgur.com/GuwVQzf.png");
            _.Paragraph("We can either add ViewComponents at runtime, or assign them directly to each Collider individually:");
            _.ImageByUrl("http://i.imgur.com/RX8hZeY.png");
            _.Paragraph("Once this is done, we simply need to add a short piece of code to each ViewComponent to detect collisions and execute TakeDamage, something like:");
            _.ShowGist("2c2c6ca621f91280b693","PlayerArmComponent.cs");
            _.Paragraph("So now with similar code on all of our body part GameObjects, they will each provide separate collision checks for individual body parts.  If a body part is hit by a bullet GameObject with a BulletView on it, the damage of that BulletViewModel instance will be passed, along with the type of body part hit, to the TakeDamage command on the PlayerController.");

        }
    }
}
