
namespace uFrame.MVVM.Bindings
{

    using System;

    /// <summary>
    /// Interface for all bindings
    /// </summary>
    public interface IBinding
    {

        bool IsComponent { get; set; }

        string ModelMemberName { get; set; }

        //ViewBase Source { get; set; }

        bool TwoWay { get; set; }

        void Bind();

        void Unbind();
    }

    public class GenericBinding : IBinding
    {
        public bool IsComponent { get; set; }
        public string ModelMemberName { get; set; }
        public bool TwoWay { get; set; }

        public Action BindAction { get; set; }
        public Action UnbindAction { get; set; }

        public GenericBinding(Action bindAction, Action unbindAction)
        {
            BindAction = bindAction;
            UnbindAction = unbindAction;
        }

        public void Bind()
        {
            if (BindAction != null)
            {
                BindAction();
            }
        }

        public void Unbind()
        {
            if (UnbindAction != null)
            {
                UnbindAction();
            }
        }
    }
}