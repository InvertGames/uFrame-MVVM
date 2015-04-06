using System.Collections.Specialized;

namespace System.Collections.ObjectModel
{
    #if !NETFX_CORE
    public delegate void NotifyCollectionChangedEventHandler(Object sender, NotifyCollectionChangedEventArgs changeArgs);
    #endif
}