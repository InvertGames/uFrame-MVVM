using System.Collections.Generic;

[UBCategory("Default")]
public class UBOnLevelWasLoadedTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnLevelWasLoadedTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "loadedLevelIndex", ValueType = typeof(int) };
    }

    public static void OnLevelWasLoaded(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnLevelWasLoaded(int loadedLevelIndex)
    {
        ExecuteSheetWithVars(new UBInt(loadedLevelIndex) { Name = "loadedLevelIndex" });
    }
}