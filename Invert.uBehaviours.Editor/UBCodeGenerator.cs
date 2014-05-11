using System;
using System.Collections.Generic;
using System.Linq;

public abstract class UBCodeGenerator
{
    private static Dictionary<Type, UBCodeGenerator> _generators;

    public static Dictionary<Type, UBCodeGenerator> Generators
    {
        get
        {
            if (_generators == null)
            {
                GetGenerators();
            }
            return _generators;
        }
        set { _generators = value; }
    }

    public static void GetGenerators()
    {
        _generators = new Dictionary<Type, UBCodeGenerator>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(CodeGeneratorAttribute), true);
                if (attributes.Length < 1) continue;
                var attribute = attributes.First() as CodeGeneratorAttribute;
                var generator = Activator.CreateInstance(attribute.GeneratorType) as UBCodeGenerator;
                if (generator == null)
                    throw new Exception(string.Format("Couldn't load generator for type {0}.  It must derive from UBCodeGenerator", type.Name));
                _generators.Add(type, generator);
            }
        }
    }

    public abstract void Generate(UBehaviourCSharpGenerator generator);
}