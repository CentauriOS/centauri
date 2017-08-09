using System;
using System.Reflection;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The base class for all dynamically generated proxies.
    /// </summary>
    abstract class DynamicBase {
        /// <summary>
        /// Gets the manager for this control.
        /// </summary>
        /// <value>The manager for this control.</value>
        internal ControlManager Manager {
            get;
            private set;
        }

        /// <summary>
        /// Gets a reference to the real object that is being proxied to.
        /// </summary>
        /// <value>The real object.</value>
        internal object Real {
            get;
            private set;
        }

        /// <summary>
        /// Ensures that the real object exists, and creates it if it does not.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBase" /> class.
        /// </summary>
        /// <param name="manager">The manager for this control.</param>
        public DynamicBase(ControlManager manager) {
            Manager = manager;
        }
    }
}
