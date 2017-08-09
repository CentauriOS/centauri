using System;
using System.IO;
using System.Reflection;

namespace Centauri.IoC.Framework {
    sealed class AssemblyOverride<T> : IAssemblyOverride {
        readonly Assembly Assembly;
        readonly string Name;

        public Assembly this[AssemblyName name] {
            get {
                return name.Name == Name ? Assembly : null;
            }
        }

        public bool this[string path] {
            get {
                return Path.GetFileNameWithoutExtension(path) == Name;
            }
        }

        internal AssemblyOverride() {
            Assembly = typeof(T).GetTypeInfo().Assembly;
            Name = Assembly.GetName().Name;
        }
    }
}
