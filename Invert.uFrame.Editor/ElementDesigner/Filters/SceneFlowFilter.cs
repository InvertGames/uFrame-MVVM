using System;

[Serializable]
public class SceneFlowFilter : DefaultFilter
{
    public override bool ImportedOnly
    {
        get { return true; }
    }

    public override string Name
    {
        get { return "Scene Flow"; }
    }

    public override bool IsItemAllowed(object item, Type t)
    {
        if (t == typeof (IDiagramItem))
        {
            return true;
        }
        if (t == typeof (ViewModelCommandData))
        {
            return true;
        }
        if (t == typeof(AdditiveSceneData))
        {
            return true;
        }
        return base.IsItemAllowed(item, t);
    }

    public override bool IsAllowed(object item, Type t)
    {
        if (t == typeof(ViewModelCommandData)) return true;
        if (t == typeof(SceneManagerData)) return true;
        if (t == typeof(SubSystemData)) return true;
        

        //var element = item as ElementDataBase;
        //if (element != null && !element.IsMultiInstance)
        //{
        //    return true;
        //}
        return false;
    }
}