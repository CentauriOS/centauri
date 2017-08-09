using System;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The interface for the class that the framework will create once it has
    /// finished loading and the bootstrap code has finished.
    /// </summary>
    public interface IFrameworkMain {
        /// <summary>
        /// The entry point for the part of the program that can use the
        /// inversion of control framework.  Once this method is called, the
        /// bootstrap code and framework loading code is finished will all of
        /// its initialization.
        /// </summary>
        void Main();
    }
}
