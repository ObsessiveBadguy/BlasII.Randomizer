﻿
namespace BlasII.Randomizer.Benchmarks.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BenchmarkParametersAttribute : Attribute
{
    internal string ParameterProperty { get; }
    internal object[] Parameters { get; }

    public BenchmarkParametersAttribute(params object[] parameters)
    {
        Parameters = parameters;
    }

    public BenchmarkParametersAttribute(string property)
    {
        ParameterProperty = property;
    }
}
