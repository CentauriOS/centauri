using System;
using System.Reflection;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// A class that represents an implementation of a specific control.
    /// </summary>
    sealed class ControlImplementation {
        /// <summary>
        /// The fixed weight of the control.
        /// </summary>
        readonly int weight;
        /// <summary>
        /// The function that determines the variable weight of the control.
        /// </summary>
        readonly Func<int> WeightFunc;
        /// <summary>
        /// The type that implements the control interface.
        /// </summary>
        internal readonly Type Type;
        /// <summary>
        /// The default constructor for creating new instances of the control.
        /// </summary>
        internal readonly ConstructorInfo Ctor;

        /// <summary>
        /// Determines the weight, by taking into account both the fixed weight
        /// and the variable weight.
        /// </summary>
        /// <value>The actual weight of the control.</value>
        internal int Weight {
            get {
                try {
                    return WeightFunc?.Invoke() ?? weight;
                } catch {
                    return int.MinValue;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ControlImplementation" /> class.
        /// </summary>
        /// <param name="type">
        /// The type that implements the control interface.
        /// </param>
        /// <param name="info">The information about the type.</param>
        /// <param name="ctrlAttr">
        /// The attribute that the class is annotated with to specify that it is
        /// a control.
        /// </param>
        internal ControlImplementation(Type type, TypeInfo info, ControlAttribute ctrlAttr) {
            weight = ctrlAttr.Weight;
            foreach (MethodInfo method in info.GetMethods(BindingFlags.Static)) {
                ControlWeightAttribute attr = method.GetCustomAttribute<ControlWeightAttribute>();
                if (attr != null) {
                    if (WeightFunc == null) {
                        if (method.ReturnType != typeof(int)) {
                            throw new TypeLoadException("Invalid return type for weight function");
                        }
                        if (method.GetParameters().Length > 0) {
                            throw new TypeLoadException("Invalid parameters for weight function");
                        }
                        WeightFunc = (Func<int>) method.CreateDelegate(typeof(Func<int>));
                    } else {
                        throw new TypeLoadException("Each control may only have a single weight function");
                    }
                }
            }
            Type = type;
            Ctor = info.GetConstructor(new Type[0]);
        }
    }
}
