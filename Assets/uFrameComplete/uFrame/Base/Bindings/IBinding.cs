/// <summary>
/// Interface for all bindings
/// </summary>
public interface IBinding
{
    bool CanTwoWayBind { get; }

    bool IsComponent { get; set; }

    string ModelMemberName { get; set; }

    //ViewBase Source { get; set; }

    bool TwoWay { get; set; }

    void Bind();

    void Unbind();
}