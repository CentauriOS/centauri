using System;
using System.Reflection;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    sealed class ControlImplementation {
        readonly int weight;
        readonly Func<int> WeightFunc;
        internal readonly Type Type;
        internal readonly ConstructorInfo Ctor;

        internal int Weight {
            get {
                try {
                    return WeightFunc?.Invoke() ?? weight;
                } catch {
                    return int.MinValue;
                }
            }
        }

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
