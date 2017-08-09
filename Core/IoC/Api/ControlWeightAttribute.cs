using System;

namespace Centauri.IoC.Api {
    /// <summary>
    /// This attribute can be used on a static method of a class that has the
    /// <see cref="ControlAttribute" /> to specify that the function contains
    /// code for a variable weight.  The method this annotates must take no
    /// parameters and return an <see cref="int" /> .
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ControlWeightAttribute : Attribute {
    }
}
