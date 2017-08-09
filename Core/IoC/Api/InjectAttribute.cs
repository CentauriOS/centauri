using System;

namespace Centauri.IoC.Api {
    /// <summary>
    /// This attribute specifies that a field in a control class should be set
    /// to a singleton instance of the control that they represent.  Before any
    /// method is ever called on the control, all fields annotated with this
    /// attribute will be filled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute {
    }
}
