/// <summary>
/// A base class for binding to a ViewModel command.
/// </summary>
public class ModelCommandBinding : CommandBinding
{
    public ComponentCommandBinding Component { get; set; }

    public ModelCommandBinding()
    {
    }

    public override void Bind()
    {
        base.Bind();

        if (Component != null)
            Component.enabled = true;
    }

    public override void Unbind()
    {
        base.Unbind();
        if (Component != null)
        {
            // Make sure we set the binding to null so that it recreates
            Component.Binding = null;
            // Disable the component while unbound
            Component.enabled = false;
        }
    }
}