using System.Collections.Generic;

namespace uFrame.MVVM
{
    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewModelManager : IEnumerable<ViewModel>
    {
        void Add(ViewModel viewModel);
        void Remove(ViewModel viewModel);
    }

    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewModelManager<T> : IViewModelManager
    {

    }
}