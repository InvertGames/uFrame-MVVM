#if DLL
namespace Invert.MVVM
{
#endif
public interface ITwoWayBinding : IBinding
{
    /// <summary>
    /// Will be called every update frame
    /// </summary>
    void BindReverse();
}
#if DLL
}
#endif