public interface IBindingProvider
{
    void Bind(ViewBase view);

    void Unbind(ViewBase viewBase);
}