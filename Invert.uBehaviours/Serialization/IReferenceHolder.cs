using System.Collections.Generic;

public interface IReferenceHolder
{
    List<UnityEngine.Object> ObjectReferences { get; }
}