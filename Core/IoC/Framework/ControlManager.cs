using System;
using System.Collections.Generic;
using System.Reflection;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    class ControlManager {
        readonly List<ControlImplementation> Implementations;
        ConstructorInfo ProxyCtor;
        object singleton;
        internal readonly IoCFramework Framework;
        internal readonly Type Interface;
        internal event Action ImplementationChanged;
        internal ControlImplementation CurrentImpl {
            get;
            private set;
        }
        internal virtual object Singleton {
            get {
                if (singleton == null) {
                    singleton = Create();
                }
                return singleton;
            }
        }

        internal virtual object Create() {
            if (ProxyCtor == null) {
                ProxyCtor = Framework.CreateProxy(this).GetTypeInfo().GetConstructor(new [] {
                    typeof(ControlManager)
                });
            }
            return ProxyCtor.Invoke(new [] {
                this
            });
        }

        internal virtual void Add(ControlImplementation impl) {
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

        protected ControlManager(IoCFramework framework, Type iface) {
            Framework = framework;
            Implementations = new List<ControlImplementation>();
            Interface = iface;
        }

        internal static ControlManager Create(IoCFramework framework, Type iface) {
            if (iface == typeof(IIoCFramework)) {
                return new FrameworkControlManager(framework);
            } else {
                return new ControlManager(framework, iface);
            }
        }
    }
}
