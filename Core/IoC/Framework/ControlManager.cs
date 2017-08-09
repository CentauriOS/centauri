using System;
using System.Collections.Generic;
using System.Reflection;

namespace Centauri.IoC.Framework {
    sealed class ControlManager {
        readonly List<ControlImplementation> Implementations;
        ConstructorInfo ProxyCtor;
        internal readonly IoCFramework Framework;
        internal readonly Type Interface;
        internal event Action ImplementationChanged;
        internal ControlImplementation CurrentImpl {
            get;
            private set;
        }

        internal object Create() {
            if (ProxyCtor == null) {
                ProxyCtor = Framework.CreateProxy(this).GetTypeInfo().GetConstructor(new [] {
                    typeof(ControlManager)
                });
            }
            return ProxyCtor.Invoke(new [] {
                this
            });
        }

        internal void Add(ControlImplementation impl) {
            Implementations.Add(impl);
            int weight = int.MinValue;
            ControlImplementation heaviest = null;
            foreach (ControlImplementation control in Implementations) {
                int w = control.Weight;
                if (w >= weight) {
                    weight = w;
                    heaviest = control;
                }
            }
            if (heaviest != CurrentImpl) {
                CurrentImpl = heaviest;
                ImplementationChanged?.Invoke();
            }
        }

        internal ControlManager(IoCFramework framework, Type iface) {
            Framework = framework;
            Implementations = new List<ControlImplementation>();
            Interface = iface;
        }
    }
}
