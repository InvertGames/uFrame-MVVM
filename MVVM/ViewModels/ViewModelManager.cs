using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace uFrame.MVVM
{
    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewModelManager<T> : IViewModelManager<T> where T : ViewModel
    {
        private List<T> _viewModels = new List<T>();

        public List<T> ViewModels
        {
            get { return _viewModels; }
            set { _viewModels = value; }
        }

        public IEnumerator<ViewModel> GetEnumerator()
        {
            return ViewModels.Cast<ViewModel>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ViewModel viewModel)
        {
            if (!ViewModels.Contains((T) viewModel))
                ViewModels.Add((T) viewModel);

        }

        public void Remove(ViewModel viewModel)
        {
            ViewModels.Remove((T) viewModel);

        }


    }
}