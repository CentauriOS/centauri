﻿using System;
using Centauri.IoC.Api;
using Centauri.IoC.Framework;
using Centauri.Rendering.Api;

namespace Centauri.Ui {
    [Control]
    public class UiMain : IFrameworkMain {
        [Inject]
        readonly IRenderer Renderer;

        public void Main() {
            Renderer.Init();
        }
    }
}
