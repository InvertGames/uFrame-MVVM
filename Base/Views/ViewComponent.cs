using UnityEngine;

public abstract class ViewComponent : MonoBehaviour, IBindingProvider
{
    [SerializeField]
    private ViewBase _view;


    public ViewBase View
    {
        get { return _view != null ? _view : this.GetView(); }
        set { _view = value; }
    }

    public virtual void Awake()
    {
        if (View == null) return;
        if (View.IsBound)
        {
            this.Bind(View);
        }
        else
        {
            View.BindingProviders.Add(this);
        }
        
    }

    public virtual void Bind(ViewBase view)
    {

    }

    public virtual void Unbind(ViewBase viewBase)
    {
        
    }
}