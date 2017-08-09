using System;
using System.Reflection;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    abstract class DynamicBase {
        internal ControlManager Manager {
            get;
            private set;
        }

        internal object Real {
            get;
            private set;
        }

        internal void EnsureObject() {
            if (Real == null) {
                if (Manager.CurrentImpl == null) {
                    throw new NotSupportedException("There are no interface implementations loaded");
                }
                Real = Manager.CurrentImpl.Ctor.Invoke(new object[0]);
                foreach (FieldInfo field in Real.GetType().GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) {
                    InjectAttribute attr = field.GetCustomAttribute<InjectAttribute>();
                    if (attr != null) {
                        field.SetValue(Real, Manager.Framework.GetSingleton(field.FieldType));
                    }
                }
            }
        }

        public DynamicBase(ControlManager manager) {
            Manager = manager;
        }
    }
}
