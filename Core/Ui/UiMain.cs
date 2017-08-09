﻿using System;
using Centauri.IoC.Api;
using Centauri.IoC.Framework;

namespace Centauri.Ui {
    [Control]
    public class UiMain : IFrameworkMain {
        public void Main() {
            Console.WriteLine("Hello, world!");
        }
    }
}
