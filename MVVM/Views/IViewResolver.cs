using UnityEngine;

namespace uFrame.MVVM
{
    public interface IViewResolver
    {
        /// <summary>
        /// Provides a prefab
        /// </summary>
        /// <param name="model">The model for the view prefab we are looking for</param>
        /// <returns></returns>
        GameObject FindView(ViewModel model);

        /// <summary>
        /// Provides a prefab based off a viewname
        /// </summary>
        /// <param name="viewName">The name of the view prefab we are looking for</param>
        /// <returns></returns>
        GameObject FindView(string viewName);
    }
}