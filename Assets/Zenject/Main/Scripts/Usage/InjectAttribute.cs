using System;
//using JetBrains.Annotations;

namespace Zenject
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Parameter |
                    AttributeTargets.Property | AttributeTargets.Field)]
    // Uncomment for use with ReSharper
    // MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(string identifier)
        {
            Identifier = identifier;
        }

        public InjectAttribute()
        {
        }

        public string Identifier { get; private set; }
    }
}
