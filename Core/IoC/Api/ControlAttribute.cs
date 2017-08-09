using System;

namespace Centauri.IoC.Api {
    /// <summary>
    /// An attribute that marks that a class is a control implementation.  When
    /// the IoC framework is constructing an object, it can use the annotated
    /// class to fill any fields marked with <see cref="InjectAttribute" /> if
    /// the type is an interface that the annotated class implements.  Classes
    /// with a higher weight are favored over classes with a lower weight.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ControlAttribute : Attribute {
        /// <summary>
        /// This is the weight of the implementation if, and only if, the class
        /// does not contain a static method annotated with the
        /// <see cref="ControlWeightAttribute" />.
        /// </summary>
        public readonly int Weight;

        /// <summary>
        /// An attribute that marks that a class is a control implementation.
        /// When the IoC framework is constructing an object, it can use the
        /// annotated class to fill any fields marked with
        /// <see cref="InjectAttribute" /> if the type is an interface that the
        /// annotated class implements.  Classes with a higher weight are
        /// favored over classes with a lower weight.
        /// </summary>
        /// <param name="weight">The weight of the implementation.</param>
        public ControlAttribute(int weight = 0) {
            Weight = weight;
        }
    }
}
