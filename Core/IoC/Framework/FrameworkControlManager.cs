using System;
using Centauri.IoC.Api;

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The <see cref="ControlManager" /> for the framework interface
    /// (<see cref="IIoCFramework" />).  This class is needed so the framework
    /// does not create more copies of itself, causing references to never be
    /// resoled.
    /// </summary>
    sealed class FrameworkControlManager : ControlManager {
        /// <summary>
        /// Gets the singleton instance of the control proxy.
        /// </summary>
        /// <value>The singleton instance.</value>
        internal override object Singleton {
            get {
                return Framework;
            }
        }

        /// <summary>
        /// Creates a new instance of the control proxy this class manages.
        /// </summary>
        /// <returns>The new instance.</returns>
        internal override object Create() {
            throw new NotSupportedException("The framework cannot be constructed");
        }

        /// <summary>
        /// Adds an implementation to the list of known implementations, then
        /// weighs each of the implementations to determine the best to use.
        /// </summary>
        /// <param name="impl">The new implementation to add.</param>
        internal override void Add(ControlImplementation impl) {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FrameworkControlManager" /> class.
        /// </summary>
        /// <param name="framework">
        /// The framework that this class manages a control for.
        /// </param>
        internal FrameworkControlManager(IoCFramework framework) : base(framework, typeof(IIoCFramework)) {
        }
    }
}
