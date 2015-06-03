using UniRx;
using System;
using uFrame.Kernel;

// Required for WP8 and Store APPS

namespace uFrame.MVVM
{
    public static class AggregatorExtensions
    {
        public static IObservable<TViewModel> OnViewModelCreated<TViewModel>(this IEventAggregator ea)
            where TViewModel : ViewModel
        {
            return
                ea.GetEvent<ViewModelCreatedEvent>()
                    .Where(p => p.ViewModel is TViewModel)
                    .Select(p => (TViewModel) p.ViewModel);
        }

        public static IObservable<TViewModel> OnViewModelDestroyed<TViewModel>(this IEventAggregator ea)
            where TViewModel : ViewModel
        {
            return
                ea.GetEvent<ViewModelDestroyedEvent>()
                    .Where(p => p.ViewModel is TViewModel)
                    .Select(p => (TViewModel) p.ViewModel);
        }
    }
}