namespace System.Collections.ObjectModel
{
    #if !NETFX_CORE
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    #endif
}