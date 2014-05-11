using System;

public class CodeGeneratorAttribute : Attribute
{
    public Type GeneratorType { get; set; }

    public CodeGeneratorAttribute(Type generatorType)
    {
        GeneratorType = generatorType;
    }
}