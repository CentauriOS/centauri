using System;

namespace Centauri.IoC.Api {
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ControlAttribute : Attribute {
        public readonly int Weight;

        public ControlAttribute(int weight = 0) {
            Weight = weight;
        }
    }
}
