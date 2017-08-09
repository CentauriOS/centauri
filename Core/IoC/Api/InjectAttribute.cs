using System;

namespace Centauri.IoC.Api {
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute {
    }
}
