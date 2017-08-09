using System;
using System.IO;
using System.Reflection;

namespace Centauri.IoC.Framework {
    sealed class AssemblyLoadOverride : IAssemblyOverride {
        readonly AssemblyLoader Loader;

        public Assembly this[AssemblyName name] {
            get {
                string file = Path.Combine(Loader.BaseDir, string.Concat(name.Name, ".dll"));
                if (File.Exists(file)) {
                    return Loader.LoadFromAssemblyPath(file);
                } else {
                    return Assembly.Load(name);
                }
            }
        }

        public bool this[string path] {
            get {
                return true;
            }
        }

        internal AssemblyLoadOverride(AssemblyLoader loader) {
            Loader = loader;
        }
    }
}
