using System;
using System.Collections.Generic;
using System.Linq;

public static class ElementDesignerDataExtensions
{
    public static IEnumerable<IDiagramNode> GetImportableItems(this IElementDesignerData t)
    {
        //return AllowedDiagramItems;
        return
            t.GetAllowedDiagramItems()
                .Where(p => !t.CurrentFilter.Locations.Keys.Contains(p.Identifier))
                .ToArray();
        //return items.Where(p => !Filters.Any(x => x.Locations.Keys.Contains(p.Identifier)));
    }
    public static IEnumerable<ElementData> GetAllElements(this IElementDesignerData t)
    {
        if (t == null)
        {
            throw new Exception("Designer data can't be null.");
        }
        if (t.AllDiagramItems == null)
        {
            throw new Exception("All diagram items is null.");
        }
        return t.AllDiagramItems.OfType<ElementData>();
    }

    public static IEnumerable<IDiagramNode> GetAllowedDiagramItems(this IElementDesignerData t)
    {
        return t.AllDiagramItems.Where(p => t.CurrentFilter.IsAllowed(p, p.GetType()));
    }
    public static IEnumerable<IDiagramNode> GetDiagramItems(this IElementDesignerData t)
    {
        return t.FilterItems(t.AllDiagramItems);
    }

    public static IEnumerable<ElementDataBase> GetElements(this IElementDesignerData t)
    {
        return t.GetDiagramItems().OfType<ElementDataBase>();
    }

    public static IEnumerable<IDiagramFilter> GetFilterPath(this IElementDesignerData t)
    {
        return t.FilterState.FilterStack.Reverse();
    }

    public static IEnumerable<IDiagramFilter> GetFilters(this IElementDesignerData t)
    {
        return t.AllDiagramItems.OfType<IDiagramFilter>();
    }
    public static void Prepare(this IElementDesignerData designerData)
    {
        designerData.RefactorCount = 0;
        foreach (var allDiagramItem in designerData.AllDiagramItems)
        {
            allDiagramItem.Data = designerData;
        }
        designerData.Initialize();
    }
    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData, IEnumerable<IDiagramNode> allDiagramItems)
    {
        return designerData.FilterItems(designerData.CurrentFilter, allDiagramItems);
    }   
    public static void FilterLeave(this IElementDesignerData data)
    {
    }

    public static void ApplyFilter(this IElementDesignerData designerData)
    {
        designerData.UpdateLinks();
        //foreach (var item in DiagramItems)
        //{
        //    item.Filter = CurrentFilter;
        //}
        //UpdateLinks();
    }

    public static void CleanUpFilters(this IElementDesignerData designerData)
    {
        var diagramItems = designerData.AllDiagramItems.Select(p => p.Identifier);

        foreach (var diagramFilter in designerData.GetFilters())
        {
            var removeKeys = diagramFilter.Locations.Keys.Where(p => !diagramItems.Contains(p)).ToArray();
            foreach (var removeKey in removeKeys)
            {
                diagramFilter.Locations.Remove(removeKey);
            }
        }
        //UpdateLinks();
    }

    public static string GetUniqueName(this IElementDesignerData designerData, string name)
    {
        var tempName = name;
        var index = 1;
        while (designerData.AllDiagramItems.Any(p => p.Name.ToUpper() == tempName.ToUpper()))
        {
            tempName = name + index;
            index++;
        }
        return tempName;
    }

    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData,IDiagramFilter filter, IEnumerable<IDiagramNode> allDiagramItems)
    {
        
        foreach (var item in allDiagramItems)
        {
            if (filter.IsAllowed(item, item.GetType()))
            {
                if (filter.ImportedOnly && filter != item)
                {
                    if (filter.Locations.Keys.Contains(item.Identifier))
                    {
                        yield return item;
                       
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }

    public static void PopFilter(this IElementDesignerData designerData,List<string> filterStack)
    {
        designerData.FilterLeave();
        //filterStack.Remove(designerData.FilterStack.Peek().Name);

        designerData.FilterState.FilterPoped(designerData.FilterState.FilterStack.Pop());
        designerData.ApplyFilter();
    }

    public static void PopToFilter(this IElementDesignerData designerData, IDiagramFilter filter1)
    {
        while (designerData.CurrentFilter != filter1)
        {
            designerData.PopFilter(null);
        }
    }

    public static void PopToFilter(this IElementDesignerData designerData, string filterName)
    {
        while (designerData.CurrentFilter.Name != filterName)
        {
            designerData.PopFilter(null);
        }
    }

    public static void PushFilter(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        designerData.FilterLeave();
        designerData.FilterState.FilterStack.Push(filter);
        designerData.FilterState.FilterPushed(filter);
        designerData.ApplyFilter();
    }



    public static void ReloadFilterStack(this IElementDesignerData designerData,List<string> filterStack )
    {
        if (filterStack.Count != (designerData.FilterState.FilterStack.Count))
        {
            foreach (var filterName in filterStack)
            {
                var filter = designerData.GetFilters().FirstOrDefault(p => p.Name == filterName);
                if (filter == null)
                {
                    filterStack.Clear();
                    designerData.FilterState.FilterStack.Clear();
                    break;
                }
                designerData.PushFilter(filter);
            }
        }
    }

    public static void UpdateLinks(this IElementDesignerData designerData)
    {
        designerData.CleanUpFilters();
        designerData.Links.Clear();

        var items = designerData.GetDiagramItems().SelectMany(p => p.Items).Where(p => designerData.CurrentFilter.IsItemAllowed(p, p.GetType())).ToArray();
        var diagramItems = designerData.GetDiagramItems().ToArray();
        foreach (var item in items)
        {
            designerData.Links.AddRange(item.GetLinks(diagramItems));
        }

        var diagramFilter = designerData.CurrentFilter as IDiagramNode;
        if (diagramFilter != null)
        {
            var diagramFilterItems = diagramFilter.Items.OfType<IDiagramNode>().ToArray();
            foreach (var diagramItem in diagramItems)
            {
                designerData.Links.AddRange(diagramItem.GetLinks(diagramFilterItems));
            }
            //foreach (var diagramFilterItem in diagramFilterItems)
            //{
            //    Links.AddRange(diagramFilterItem.GetLinks(diagramItems));
            //}
        }

        var models = designerData.GetDiagramItems().ToArray();

        foreach (var viewModelData in models)
        {
            //viewModelData.Filter = CurrentFilter;
            designerData.Links.AddRange(viewModelData.GetLinks(diagramItems));
        }
    }
    public static IEnumerable<ElementDataBase> GetAssociatedElementsInternal(this IElementDesignerData designerData, ElementDataBase data)
    {
        var derived = GetAllBaseItems(designerData, data);
        foreach (var viewModelItem in derived)
        {
            var element = GetElement(designerData,viewModelItem);
            if (element != null)
            {
                yield return element;
                var subItems = GetAssociatedElementsInternal(designerData, element);
                foreach (var elementDataBase in subItems)
                {
                    yield return elementDataBase;
                }
            }
        }
    }
    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        return FilterItems(designerData, filter, designerData.AllDiagramItems);
    }

    public static IEnumerable<IViewModelItem> GetAllBaseItems(this IElementDesignerData designerData, ElementDataBase data)
    {
        var current = data;
        while (current != null)
        {
            foreach (var item in current.Items)
            {
                if (item is IViewModelItem)
                {
                    yield return item as IViewModelItem;
                }
            }

            current = designerData.GetAllElements().FirstOrDefault(p => p.AssemblyQualifiedName == current.BaseTypeName);
        }
    }

    public static ElementDataBase[] GetAssociatedElements(this IElementDesignerData designerData, ElementDataBase data)
    {
        return GetAssociatedElementsInternal(designerData, data).Concat(new[] { data }).Distinct().ToArray();
    }

    public static IDiagramNode RelatedNode(this IViewModelItem item)
    {
        return item.Node.Data.AllDiagramItems.FirstOrDefault(p=>p.Name == item.RelatedTypeName);
    }
    public static ElementDataBase GetElement(this IElementDesignerData designerData, IViewModelItem item)
    {
        
        if (item.RelatedTypeName == null)
        {
            return null;
        }
        return designerData.GetAllElements().FirstOrDefault(p =>p!=null && p.Name == item.RelatedTypeName);
    }

    public static IEnumerable<IDiagramFilter> GetFilters(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        //yield return DefaultFilter;
        //yield return SceneFlowFilter;

        foreach (var allDiagramItem in designerData.FilterItems(filter).OfType<IDiagramFilter>())
        {
            yield return allDiagramItem;
        }
    }

    //public static string GetUniqueName(this IElementDesignerData designerData, string name)
    //{
    //    var tempName = name;
    //    var index = 1;
    //    while (designerData.AllDiagramItems.Any(p => p.Name.ToUpper() == tempName.ToUpper()))
    //    {
    //        tempName = name + index;
    //        index++;
    //    }
    //    return tempName;
    //}

    public static ElementData GetViewModel(this IElementDesignerData designerData, string elementName)
    {
        return designerData.Elements.FirstOrDefault(p => p.Name == elementName);
    }

}