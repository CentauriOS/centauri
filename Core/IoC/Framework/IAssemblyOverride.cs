using System;
using System.Reflection;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The interface for a method of loading assemblies.
    /// </summary>
    interface IAssemblyOverride {
        /// <summary>
        /// Attempts to load the assembly given its name.
        /// </summary>
        /// <returns>
        /// The loaded assembly, or <see cref="null" /> if the loading failed or
        /// is not supported.
        /// </returns>
        /// <param name="name">The name of the assembly to load.</param>
        Assembly this[AssemblyName name] {
            get;
        }

        /// <summary>
        /// Determines if the assembly at the specified path should be loaded or
        /// not.
        /// </summary>
        /// <param name="path">The path to the assembly.</param>
        bool this[string path] {
            get;
        }
    }
}
