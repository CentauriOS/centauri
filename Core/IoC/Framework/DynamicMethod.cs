using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The code that handles how methods are called from the dynamic proxies.
    /// </summary>
    sealed class DynamicMethod {
        /// <summary>
        /// The dynamic object that has this method.
        /// </summary>
        readonly DynamicBase Obj;
        /// <summary>
        /// The reflected method information.
        /// </summary>
        readonly MethodInfo Method;

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <returns>The result.</returns>
        /// <param name="genericTypes">
        /// The types of each generic parameter.
        /// </param>
        /// <param name="parameters">The values of each parameter.</param>
        internal object Invoke(Type[] genericTypes, object[] parameters) {
            Obj.EnsureObject();
            return (Method.IsGenericMethod ? Method.MakeGenericMethod(genericTypes) : Method).Invoke(Obj.Real, parameters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethod" />
        /// class.
        /// </summary>
        /// <param name="obj">The dynamic object that has this method.</param>
        /// <param name="name">The name of this method.</param>
        /// <param name="parameters">The types of each parameter.</param>
        public DynamicMethod(DynamicBase obj, string name, Type[] parameters) {
            Obj = obj;
            Method = Obj.Manager.Interface.GetTypeInfo().GetMethod(name, parameters);
        }
    }
}
