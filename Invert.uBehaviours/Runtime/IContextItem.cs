/// <summary>
/// For stack trace info.  Basically groups anything with a name
/// </summary>
public interface IContextItem
{
    string Name { get; }
}