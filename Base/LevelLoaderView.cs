public abstract class LevelLoaderView : View<LevelLoadViewModel>
{

    public override void Bind()
    {
        this.BindProperty(() => Model._Status, StatusChanged);
        this.BindProperty(() => Model._Progress, ProgressChanged);
        GameManager.Instance.StartCoroutine(GameManager.Load());
    }

    protected virtual void ProgressChanged(float progressValue)
    {
        
    }

    protected virtual void StatusChanged(string statusMessage)
    {
            
    }

    public override ViewModel CreateModel()
    {
        return GameManager.Progress;
    }

    protected override void InitializeViewModel(LevelLoadViewModel viewViewModel)
    {
    }

}