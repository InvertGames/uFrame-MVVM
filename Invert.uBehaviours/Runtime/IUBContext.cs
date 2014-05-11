using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUBContext
{
    //UBInstanceBehaviour Behaviour { get; }
    Stack<IContextItem> StackTrace { get; set; }
    List<UBVariableBase> Variables { get; }
    GameObject GameObject { get; }
    Transform Transform { get; }
    Coroutine StartCoroutine(IEnumerator enumerator);
    /// <summary>
    /// Execute an action sheet in this context
    /// </summary>
    /// <param name="sheet"></param>
    void ExecuteSheet(UBActionSheet sheet);

    /// <summary>
    /// Finds a variable in the current context. Use this for runtime variable searching
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <returns></returns>
    UBVariableBase GetVariable(string name);

    /// <summary>
    /// Gets a variable at runtime by its id.
    /// </summary>
    /// <param name="id">The id of the variable to get.</param>
    /// <returns></returns>
    UBVariableBase GetVariableById(string id);

    /// <summary>
    /// Execute a trigger by id
    /// </summary>
    /// <param name="triggerId"></param>
    void GoTo(string triggerId);

    /// <summary>
    /// Gets a variables value by its name
    /// </summary>
    /// <param name="name">The name of the variable</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetVariableAs<T>(string name);

    /// <summary>
    /// Sets a variable by its name. If it doesn't exist it will be created
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="value"></param>
    void SetVariable(string variableName, object value);

    /// <summary>
    /// Sets a variable by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    void SetVariableById(string id, object value);


    /// <summary>
    /// Sets a variable.  If it already exists it will replace the value with the new one.
    /// </summary>
    /// <param name="variableDeclare"></param>
    void SetVariable(UBVariableBase variableDeclare);
}