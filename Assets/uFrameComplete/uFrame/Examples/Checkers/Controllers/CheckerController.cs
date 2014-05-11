public class CheckerController : CheckerControllerBase
{
    public override void InitializeChecker(CheckerViewModel checker)
    {
        
    }

    public override void SelectCommand(CheckerViewModel checker)
    {
        CheckerBoardController.SelectChecker(checker);
    }
}