# Centauri IoC System
This directory contains the inversion of control system.
Below is an example of how to use it.

### `Api.dll`
```cs
using Centauri.IoC.Api;

namespace MyProgram {
    public interface IMyObject {
        int AddNumbers(int a, int b);

        void SayHi();
    }
}
```

### `Entry.dll`
```cs
using System;
using Centauri.IoC.Api;
using Centauri.IoC.Framework;

namespace MyProgram {
    static class Entry {
        static void Main(string[] args) {
            FrameworkBootstrap.Run();
        }
    }

    [Control]
    class MainControl : IFrameworkMain {
        [Inject]
        readonly IMyObject MyObj;

        public void Main() {
            MyObj.SayHi();
            Console.WriteLine("2 + 2 = {0}", MyObj.AddNumbers(2, 2));
        }
    }
}
```

### `ImplA.dll`
```cs
using System;
using Centauri.IoC.Api;

namespace MyProgram {
    // The weight of this control will always be 100
    [Control(100)]
    class MyObjectA : IMyObject {
        int AddNumbers(int a, int b) {
            return a + b;
        }

        void SayHi() {
            Console.WriteLine("Hello, world!");
        }
    }
}
```

### `ImplB.dll`
```cs
using System;
using Centauri.IoC.Api;

namespace MyProgram {
    // The weight of this component is going to change between 50 and 150
    // depending on the value on an environmental variable
    [Control]
    class MyObjectB : IMyObject {
        [ControlWeight]
        static int DetermineWeight() {
            if (Environment.GetEnvironmentalVariable("USE_OBJ_B") == "yes") {
                return 150;
            } else {
                return 50;
            }
        }

        int AddNumbers(int a, int b) {
            return a + b + 1;
        }

        void SayHi() {
            Console.WriteLine("Hello from object B!");
        }
    }
}
```

## Running
```bash
$ USE_OBJ_B=no dotnet Entry.dll
Hello, world!
2 + 2 = 4
$ USE_OBJ_B=yes dotnet Entry.dll
Hello from object B!
2 + 2 = 5
```
