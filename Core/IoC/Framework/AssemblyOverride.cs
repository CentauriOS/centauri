using System;
using System.IO;
using System.Reflection;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// An implementation of <see cref="IAssemblyOverride" /> that ensures that
    /// an assembly is not loaded by the <see cref="AssemblyLoader" />, but
    /// instead the already loaded version should be used (to avoid conflicts
    /// between different versions of classes).
    /// </summary>
    /// <typeparam name="T">
    /// Any type that exists inside the assembly that should be blocked from
    /// being loaded by the <see cref="AssemblyLoader" />.
    /// </typeparam>
    sealed class AssemblyOverride<T> : IAssemblyOverride {
        /// <summary>
        /// The assembly that is already loaded.
        /// </summary>
        readonly Assembly Assembly;
        /// <summary>
        /// The name of the assembly to block from being loaded.
        /// </summary>
        readonly string Name;

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
                return name.Name == Name ? Assembly : null;
            }
        }

        /// <summary>
        /// Determines if the assembly at the specified path should be loaded or
        /// not.
        /// </summary>
        /// <param name="path">The path to the assembly.</param>
        public bool this[string path] {
            get {
                return Path.GetFileNameWithoutExtension(path) == Name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyOverride{T}"/>
        /// class.
        /// </summary>
        internal AssemblyOverride() {
            Assembly = typeof(T).GetTypeInfo().Assembly;
            Name = Assembly.GetName().Name;
        }
    }
}
