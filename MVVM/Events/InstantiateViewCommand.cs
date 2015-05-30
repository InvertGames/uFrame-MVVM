using UnityEngine;

public class InstantiateViewCommand
{
    public string Identifier { get; set; }
    public IScene Scene { get; set; }
    public ViewModel ViewModelObject { get; set; }
    public ViewBase Result { get; set; }
    public GameObject Prefab { get; set; }
}