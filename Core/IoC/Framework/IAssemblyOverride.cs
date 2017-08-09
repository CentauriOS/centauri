using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    interface IAssemblyOverride {
        Assembly this[AssemblyName name] {
            get;
        }

        bool this[string path] {
            get;
        }
    }
}
