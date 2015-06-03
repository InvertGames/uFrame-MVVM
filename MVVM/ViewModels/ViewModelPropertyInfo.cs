namespace uFrame.MVVM
{
    public class ViewModelPropertyInfo
    {
        public bool IsComputed { get; set; }
        public bool IsCollectionProperty { get; set; }

        public bool IsElementProperty { get; set; }

        public bool IsEnum { get; set; }

        public IObservableProperty Property { get; set; }

        public ViewModelPropertyInfo(IObservableProperty property, bool isElementProperty, bool isCollectionProperty,
            bool isEnum, bool isComputed = false)
        {
            Property = property;
            IsElementProperty = isElementProperty;
            IsCollectionProperty = isCollectionProperty;
            IsEnum = isEnum;
            IsComputed = isComputed;
        }
    }
}