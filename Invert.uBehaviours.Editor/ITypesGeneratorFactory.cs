using System;
using System.Collections.Generic;

public interface ITypesGeneratorFactory
{
    IEnumerable<Type> GetTypes();
}