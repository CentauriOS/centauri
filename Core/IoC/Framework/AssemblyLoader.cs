using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    sealed class AssemblyLoader : AssemblyLoadContext {
        static readonly IAssemblyOverride[] Overrides = {
            new AssemblyOverride<IIoCFramework>(),
            new AssemblyOverride<IoCFramework>()
        };
        internal readonly string BaseDir;

        protected override Assembly Load(AssemblyName assemblyName) {
            return Overrides.Concat(new[] {
                new AssemblyLoadOverride(this)
            }).Select(o => o[assemblyName]).First(a => a != null);
        }

        public IEnumerable<Assembly> LoadAll() {
            return Directory.GetFiles(BaseDir, "*.dll").Where(f => !Overrides.Any(o => o[f])).Select(f => LoadFromAssemblyPath(f));
        }

        internal AssemblyLoader() {
            UriBuilder uri = new UriBuilder(Assembly.GetEntryAssembly().CodeBase);
            BaseDir = Path.GetDirectoryName(uri.Path);
        }
    }
}
