using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// This class handles the loading of assemblies from the file system.
    /// </summary>
    sealed class AssemblyLoader : AssemblyLoadContext {
        /// <summary>
        /// A list of the assemblies that need to be overridden (otherwise, a
        /// bunch of errors are caused by the same types existing in multiple
        /// forms).
        /// </summary>
        static readonly IAssemblyOverride[] Overrides = {
            new AssemblyOverride<IIoCFramework>(),
            new AssemblyOverride<IoCFramework>()
        };
        /// <summary>
        /// The base directory to load assemblies from.
        /// </summary>
        internal readonly string BaseDir;

        /// <summary>
        /// Loads the specified assembly given its name.
        /// </summary>
        /// <returns>The loaded assembly.</returns>
        /// <param name="assemblyName">The name of the assembly to load.</param>
        protected override Assembly Load(AssemblyName assemblyName) {
            return Overrides.Concat(new[] {
                new AssemblyLoadOverride(this)
            }).Select(o => o[assemblyName]).First(a => a != null);
        }

        /// <summary>
        /// Loads all assemblies in the <see cref="BaseDir" /> .
        /// </summary>
        /// <returns>A list of all assemblies that were loaded.</returns>
        public IEnumerable<Assembly> LoadAll() {
            return Directory.GetFiles(BaseDir, "*.dll").Where(f => !Overrides.Any(o => o[f])).Select(f => LoadFromAssemblyPath(f));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader" />
        /// class.
        /// </summary>
        internal AssemblyLoader() {
            UriBuilder uri = new UriBuilder(Assembly.GetEntryAssembly().CodeBase);
            BaseDir = Path.GetDirectoryName(uri.Path);
        }
    }
}
