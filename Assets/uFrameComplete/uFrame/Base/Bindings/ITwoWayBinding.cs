public interface ITwoWayBinding : IBinding
{
    /// <summary>
    /// Will be called every update frame
    /// </summary>
    void BindReverse();
}