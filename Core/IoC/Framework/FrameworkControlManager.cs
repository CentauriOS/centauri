using System;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    sealed class FrameworkControlManager : ControlManager {
        internal override object Singleton {
            get {
                return Framework;
            }
        }

        internal override object Create() {
            throw new NotSupportedException("The framework cannot be constructed");
        }

        internal override void Add(ControlImplementation impl) {
        }

        internal FrameworkControlManager(IoCFramework framework) : base(framework, typeof(IIoCFramework)) {
        }
    }
}
