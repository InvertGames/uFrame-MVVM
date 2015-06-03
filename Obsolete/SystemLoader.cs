using System;
namespace uFrame.Kernel {
    public partial class SystemLoader
    {
        [Obsolete("Regenerate your diagrams or use the extension method this.CreateInstanceViewModel<TViewModel>")]
        public TViewModel CreateInstanceViewModel<TViewModel>(string identifier)
        {
            throw new Exception(string.Format("{0} needs to be regenerated", this.GetType().Name));
        }
    }
}