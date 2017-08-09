using System;
using Centauri.IoC.Api;
using Centauri.Rendering.Api;

namespace Centauri.Rendering.WinForms {
    [Control]
    public class Renderer : IRenderer {
        public void Init() {
            Console.WriteLine("Hello, world!");
        }
    }
}
