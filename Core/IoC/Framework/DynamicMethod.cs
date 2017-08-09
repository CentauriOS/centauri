using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    sealed class DynamicMethod {
        readonly DynamicBase Obj;
        readonly MethodInfo Method;

        internal object Invoke(Type[] genericTypes, object[] parameters) {
            Obj.EnsureObject();
            return (Method.IsGenericMethod ? Method.MakeGenericMethod(genericTypes) : Method).Invoke(Obj.Real, parameters);
        }

        public DynamicMethod(DynamicBase obj, string name, Type[] parameters) {
            Obj = obj;
            Method = Obj.Manager.Interface.GetTypeInfo().GetMethod(name, parameters);
        }
    }
}
