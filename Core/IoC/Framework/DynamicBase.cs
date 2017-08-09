using System;

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
            }
        }

        public DynamicBase(ControlManager manager) {
            Manager = manager;
        }
    }
}
