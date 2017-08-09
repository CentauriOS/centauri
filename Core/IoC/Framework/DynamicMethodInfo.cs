using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// This class contains information about methods that are being added to
    /// dynamic proxies.
    /// </summary>
    sealed class DynamicMethodInfo {
        /// <summary>
        /// The name of the method.
        /// </summary>
        internal string Name;
        /// <summary>
        /// The number of times this method has already been overloaded.  This
        /// number will be unique for any group of methods belonging to the same
        /// class with the same name.
        /// </summary>
        internal int Overload;
        /// <summary>
        /// The type that the method return.
        /// </summary>
        internal Type ReturnType;
        /// <summary>
        /// The parameter types.
        /// </summary>
        internal Type[] ParameterTypes;
        /// <summary>
        /// The generic parameter types.
        /// </summary>
        internal Type[] GenericParameters;
        /// <summary>
        /// The field that has been created in the dynamic proxy.
        /// </summary>
        internal FieldInfo Field;

        /// <summary>
        /// Gets the name of the field that should be created in the dynamic
        /// proxy.
        /// </summary>
        /// <value>The name of the field.</value>
        internal string FieldName {
            get {
                return string.Format("del_{0}{1}", Name, Overload);
            }
        }
    }
}
