﻿using System;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// Bootstraps the framework code and launches the heaviest
    /// <see cref="IFrameworkMain" /> found in the base directory of the loader.
    /// </summary>
    public static class FrameworkBootstrap {
        /// <summary>
        /// Bootstraps the framework code and launches the heaviest
        /// <see cref="IFrameworkMain" /> found in the base directory of the
        /// loader.
        /// </summary>
        public static void Run() {
            IoCFramework framework = new IoCFramework();
            framework.Load();
            framework.Create<IFrameworkMain>().Main();
        }
    }
}
