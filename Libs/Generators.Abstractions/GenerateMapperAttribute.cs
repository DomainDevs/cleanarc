using System;

namespace Generators.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class GenerateMapperAttribute : Attribute
    {
        public Type EntityType { get; }

        public GenerateMapperAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}