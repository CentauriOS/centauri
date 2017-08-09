using System;

namespace Centauri.IoC.Api {
    public interface IIoCFramework {
        T GetSingleton<T>();

        T Create<T>();

        void Load(string assembly = null);
    }
}
