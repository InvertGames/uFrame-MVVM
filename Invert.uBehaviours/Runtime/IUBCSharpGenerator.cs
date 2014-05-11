using System;
using UBehaviours.Actions;

/// <summary>
/// An interface for generating code from a behaviour.
/// </summary>
public interface IUBCSharpGenerator
{
    /// <summary>
    /// Create a field on the class being generated
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    void AddField(string name, Type type);

    /// <summary>
    /// Make this current class being generated extend from an interface
    /// </summary>
    /// <param name="type"></param>
    void AddInterface(Type type);

    /// <summary>
    /// Appends text to the current code generation method.
    /// </summary>
    /// <param name="text"></param>
    void Append(string text);

    /// <summary>
    /// Writes an expression based code to the current code generation method.  It can be anything and the variables will automatically be resolved.
    /// </summary>
    /// <param name="expressionTemplate"></param>
    /// <returns></returns>
    void AppendExpression(string expressionTemplate);

    /// <summary>
    /// Similar to string.format
    /// </summary>
    /// <param name="text"></param>
    /// <param name="args"></param>
    void AppendFormat(string text, params object[] args);

    /// <summary>
    /// A simple variable assign statement
    /// </summary>
    /// <param name="v">The UBVariable you want to assign to.</param>
    /// <param name="fieldName"></param>
    /// <param name="rightExpression"></param>
    void AssignStatement(UBVariableBase v, string fieldName, string rightExpression);

    /// <summary>
    /// Creates a if statement based off of an UBConditionAction
    /// </summary>
    /// <param name="ubConditionAction"></param>
    /// <param name="expression"></param>
    /// <param name="trueSheet"></param>
    /// <param name="falseSheet"></param>
    void ConditionStatement(UBConditionAction ubConditionAction, string expression, UBActionSheet trueSheet, UBActionSheet falseSheet);
    /// <summary>
    /// Returns an expression based code.  It can be anything and the variables will automatically be resolved.
    /// </summary>
    /// <param name="expressionTemplate"></param>
    /// <returns></returns>
    string Expression(string expressionTemplate);
    /// <summary>
    /// Does a field already exist in the generating code.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool HasField(string name);

    /// <summary>
    /// A simple method that will generate code to invoke a template
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns>The code to execute the template sheet.</returns>
    string InvokeTemplateSheet(string templateName);
    /// <summary>
    /// A method that returns the code that will execute a trigger sheet.
    /// </summary>
    /// <param name="sheet"></param>
    /// <returns></returns>
    string InvokeTriggerSheet(UBActionSheet sheet = null);

    /// <summary>
    /// Get a variable name that is used in the current class. Use this for any variable to ensure variables have unique names.
    /// </summary>
    /// <param name="v">The UBVariable to used</param>
    /// <param name="fieldName">The name of the field on the action this variable belongs to.</param>
    /// <returns>The variable name</returns>
    string VariableName(UBVariableBase v, string fieldName);
}