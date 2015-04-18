using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class ImportantNotes : uFrameMVVMPage<MVVMPage>
{

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("Although uFrame does its best to support and guide you to the creation of better games, it is extremely important to realize that the implementation is still always left to you.  While uFrame tries to separate things clearly, there are still countless ways you can circumvent and break the patterns put in place.  The most common violation is through mixing core game logic with representation logic, where some poor developer has created a nightmare trying to access fields on Views from Controllers or other Views.  These separations exist for a reason, so one puzzle piece can content itself with handling its own functionality and not worry how other pieces are handling themselves.");
        _.Break();
        _.Paragraph("There are also many situations with multiple valid solutions, and it all depends on how you decide to arrange your implementation.  When prototyping, many things are forgivable and easily redesigned as the project evolves and features are settled on.  It's very easy to bind/subscribe to properties in order to initiate logic, but you should always consider from where you want that logic handled.  For example, imagine something odd happens and your RobotEnemy suddenly stops working correctly.  If all of your modifications are done in RobotEnemyController, then you've only got one place to look for the problem.  If you've allowed any parent, or child, or practically anything with a reference to RobotEnemy, to make modifications to it, then you've got more places to check...");
        _.Break();
        _.Paragraph(" Above all, stick to the separation of Controller <-> ViewModel <- View.");
        _.Paragraph(" - Controllers handle the layer of core game logic,");
        _.Paragraph(" - ViewModels are effectively the data layer,");
        _.Paragraph(" - and Views are the presentation layer, handling how the game is displayed and represented in Unity.");
    }
}