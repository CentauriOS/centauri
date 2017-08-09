﻿using System;

namespace Centauri.IoC.Framework {
    public static class FrameworkBootstrap {
        public static void Run() {
            IoCFramework framework = new IoCFramework();
            framework.Load();
            framework.Create<IFrameworkMain>().Main();
        }
    }
}
