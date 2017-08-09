using System;

namespace Centauri.IoC.Api {
    public interface IIoCFramework {
        T Create<T>();

        void Load(string assembly = null);
    }
}
