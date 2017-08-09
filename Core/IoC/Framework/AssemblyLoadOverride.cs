using System;
using System.IO;
using System.Reflection;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// An implementation of <see cref="IAssemblyOverride" /> that contains the
    /// default loading code.
    /// </summary>
    sealed class AssemblyLoadOverride : IAssemblyOverride {
        /// <summary>
        /// The assembly loading context.
        /// </summary>
        readonly AssemblyLoader Loader;

        /// <summary>
        /// Attempts to load the assembly given its name.
        /// </summary>
        /// <returns>
        /// The loaded assembly, or <see cref="null" /> if the loading failed or
        /// is not supported.
        /// </returns>
        /// <param name="name">The name of the assembly to load.</param>
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

        /// <summary>
        /// Determines if the assembly at the specified path should be loaded or
        /// not.
        /// </summary>
        /// <param name="path">The path to the assembly.</param>
        public bool this[string path] {
            get {
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AssemblyLoadOverride" /> class.
        /// </summary>
        /// <param name="loader">The assembly loading context.</param>
        internal AssemblyLoadOverride(AssemblyLoader loader) {
            Loader = loader;
        }
    }
}
