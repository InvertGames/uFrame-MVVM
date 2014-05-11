using System.Collections.Generic;
using UnityEngine;

public abstract class UBConditionAction : UBAction
{
    [HideInInspector]
    public UBActionSheet _OnFalse;

    [HideInInspector]
    public UBActionSheet _OnTrue;

    [UBRequireVariable]
    public UBBool _StoreResult = new UBBool();

    public abstract bool PerformCondition(IUBContext context);

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.ConditionStatement(this, WriteExpressionCode(sb), _OnTrue, _OnFalse);
    }

    public virtual string WriteExpressionCode(IUBCSharpGenerator sb)
    {
        return string.Empty;
    }
    /// <summary>
    /// The label that is used for the true action sheet
    /// </summary>
    public virtual string TrueLabel { get { return "OnTrue"; } }
    /// <summary>
    /// The label that is used for the false action sheet
    /// </summary>
    public virtual string FalseLabel { get { return "OnFalse"; } }
     
    public override IEnumerable<UBActionSheetInfo> GetAvailableActionSheets(IUBehaviours behaviours)
    {
        yield return new UBActionSheetInfo()
        {
            Field = GetType().GetField("_OnFalse"),
            Name = FalseLabel,
            Sheet = _OnFalse,
            Owner = this
        };
        yield return new UBActionSheetInfo()
        {
            Field = GetType().GetField("_OnTrue"),
            Name = TrueLabel,
            Sheet = _OnTrue,
            Owner = this
        };
    }

    protected sealed override void PerformExecute(IUBContext context)
    {
        var result = PerformCondition(context);
        if (result)
        {
         
            if (_OnTrue != null)
                _OnTrue.Execute(context);
        }
        else
        {
            if (_OnFalse != null)
                _OnFalse.Execute(context);
        }

        if (_StoreResult != null)
            _StoreResult.SetValue(context, result);
    }
}