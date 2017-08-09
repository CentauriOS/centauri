using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    sealed class DynamicMethodInfo {
        internal string Name;
        internal int Overload;
        internal Type ReturnType;
        internal Type[] ParameterTypes;
        internal Type[] GenericParameters;
        internal FieldInfo Field;

        internal string FieldName {
            get {
                return string.Format("del_{0}{1}", Name, Overload);
            }
        }
    }
}
