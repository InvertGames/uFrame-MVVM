namespace uFrame.MVVM.Bindings
{
    public interface ITwoWayBinding : IBinding
    {
        /// <summary>
        /// Will be called every update frame
        /// </summary>
        void BindReverse();
    }
}
