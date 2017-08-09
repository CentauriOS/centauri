using System;

namespace Centauri.IoC.Api {
    /// <summary>
    /// The interface for the inversion of control framework.
    /// </summary>
    public interface IIoCFramework {
        /// <summary>
        /// Gets the singleton instance of a control class.
        /// </summary>
        /// <returns>The singleton instance.</returns>
        /// <typeparam name="T">
        /// The interface that the return control needs to implement.
        /// </typeparam>
        T GetSingleton<T>();

        /// <summary>
        /// Creates a new instance of a control class.
        /// </summary>
        /// <returns>The new instance.</returns>
        /// <typeparam name="T">
        /// The interface that the return control needs to implement.
        /// </typeparam>
        T Create<T>();

        /// <summary>
        /// Loads the framework, or optionally a single assembly into the
        /// currently loaded framework.
        /// </summary>
        /// <param name="assembly">
        /// An assembly to load, or <see cref="null" /> if the entire framework
        /// should be loaded.
        /// </param>
        void Load(string assembly = null);
    }
}
