using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Centauri.IoC.Api;
using Centauri.IoC.Framework;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(IoCFramework.DynamicAsmName)]

namespace Centauri.IoC.Framework {
    /// <summary>
    /// The framework that allows for inversion of control and dependency
    /// injection.
    /// </summary>
    [Control]
    public class IoCFramework : IIoCFramework {
        /// <summary>
        /// The name of the dynamic assembly that is created with all of the
        /// proxy classes.
        /// </summary>
        internal const string DynamicAsmName = "Centauri Dynamic IoC";
        /// <summary>
        /// A list of all the assemblies that have been loaded into the
        /// framework so far.
        /// </summary>
        readonly List<Assembly> LoadedAssemblies;
        /// <summary>
        /// The assembly loading context.
        /// </summary>
        readonly AssemblyLoader Loader;
        /// <summary>
        /// The controls that this framework is currently aware of.
        /// </summary>
        readonly Dictionary<Type, ControlManager> Controls;
        /// <summary>
        /// The assembly builder for the dynamic proxies.
        /// </summary>
        readonly AssemblyBuilder AsmBldr;
        /// <summary>
        /// The module builder for the dynamic proxies.
        /// </summary>
        readonly ModuleBuilder ModBldr;
        /// <summary>
        /// The namespace that all of the dynamic proxies should reside in.
        /// </summary>
        readonly string DynamicsNamespace;

        /// <summary>
        /// Emits an instruction to load an integer into the il bytecode.
        /// </summary>
        /// <param name="il">The il generator to emit to.</param>
        /// <param name="val">The value to emit to the il.</param>
        void EmitLoadInt(ILGenerator il, int val) {
            switch (val) {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    il.Emit(OpCodes.Ldc_I4, val);
                    break;
            }
        }

        /// <summary>
        /// Emits an instruction to load an argument into the il bytecode.
        /// </summary>
        /// <param name="il">The il generator to emit to.</param>
        /// <param name="i">The index of the argument to load.</param>
        void EmitLoadArg(ILGenerator il, int i) {
            switch (i) {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    EmitLoadInt(il, i);
                    il.Emit(OpCodes.Ldarg);
                    break;
            }
        }

        /// <summary>
        /// Discovers what needs to be proxied for a single control, then
        /// generates a dynamic type the implements the interface to complete
        /// the proxy.
        /// </summary>
        /// <returns>The newly generated proxy type.</returns>
        /// <param name="manager">
        /// The control that is requesting the proxy type.
        /// </param>
        internal Type CreateProxy(ControlManager manager) {
            Type baseType = typeof(DynamicBase);
            TypeBuilder type = ModBldr.DefineType(string.Concat(DynamicsNamespace, manager.Interface.FullName), TypeAttributes.UnicodeClass, baseType, new[] {
                manager.Interface
            });
            TypeInfo iface = manager.Interface.GetTypeInfo();
            List<DynamicMethodInfo> methods = new List<DynamicMethodInfo>();
            Dictionary<string, int> overloads = new Dictionary<string, int>();
            foreach (MethodInfo iMethod in iface.DeclaredMethods) {
                int overload = 0;
                if (overloads.ContainsKey(iMethod.Name)) {
                    overload = overloads[iMethod.Name];
                }
                overloads[iMethod.Name] = overload + 1;
                methods.Add(new DynamicMethodInfo {
                    Name = iMethod.Name,
                    Overload = overload,
                    ReturnType = iMethod.ReturnType,
                    ParameterTypes = iMethod.GetParameters().Select(p => p.ParameterType).ToArray(),
                    GenericParameters = iMethod.GetGenericArguments()
                });
            }
            // Define fields
            foreach (DynamicMethodInfo method in methods) {
                method.Field = type.DefineField(method.FieldName, typeof(DynamicMethod), FieldAttributes.Private);
            }
            // Implement methods
            ILGenerator il;
            MethodInfo GetTypeFromHandle = typeof(Type).GetTypeInfo().GetMethod("GetTypeFromHandle");
            TypeInfo DynamicMethod = typeof(DynamicMethod).GetTypeInfo();
            MethodInfo Invoke = DynamicMethod.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (DynamicMethodInfo method in methods) {
                MethodBuilder bldr = type.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, method.ParameterTypes);
                string[] generics = method.GenericParameters.Select((t, i) => string.Format("T{0}", i)).ToArray();
                if (generics.Length > 0) {
                    bldr.DefineGenericParameters(generics);
                }
                il = bldr.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, method.Field);
                EmitLoadInt(il, method.GenericParameters.Length);
                il.Emit(OpCodes.Newarr, typeof(Type));
                for (int i = 0; i < method.GenericParameters.Length; ++i) {
                    il.Emit(OpCodes.Dup);
                    EmitLoadInt(il, i);
                    il.Emit(OpCodes.Ldtoken, method.GenericParameters[i]);
                    il.EmitCall(OpCodes.Call, GetTypeFromHandle, new Type[0]);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                EmitLoadInt(il, method.ParameterTypes.Length);
                il.Emit(OpCodes.Newarr, typeof(object));
                for (int i = 0; i < method.ParameterTypes.Length; ++i) {
                    il.Emit(OpCodes.Dup);
                    EmitLoadInt(il, i);
                    EmitLoadArg(il, i + 1);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                il.EmitCall(OpCodes.Callvirt, Invoke, new Type[0]);
                if (method.ReturnType == typeof(void)) {
                    il.Emit(OpCodes.Pop);
                }
                il.Emit(OpCodes.Ret);
            }
            // Define constructor
            ConstructorBuilder ctor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new [] {
                typeof(ControlManager)
            });
            il = ctor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            TypeInfo baseInfo = baseType.GetTypeInfo();
            il.Emit(OpCodes.Call, baseInfo.GetConstructor(new [] {
                typeof(ControlManager)
            }));
            ConstructorInfo methodCtor = DynamicMethod.GetConstructor(new Type[] {
                typeof(DynamicBase),
                typeof(string),
                typeof(Type[])
            });
            foreach (DynamicMethodInfo method in methods) {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, method.Name);
                EmitLoadInt(il, method.ParameterTypes.Length);
                il.Emit(OpCodes.Newarr, typeof(Type));
                for (int i = 0; i < method.ParameterTypes.Length; ++i) {
                    il.Emit(OpCodes.Dup);
                    EmitLoadInt(il, i);
                    il.Emit(OpCodes.Ldtoken, method.ParameterTypes[i]);
                    il.EmitCall(OpCodes.Call, GetTypeFromHandle, new Type[0]);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                il.Emit(OpCodes.Newobj, methodCtor);
                il.Emit(OpCodes.Stfld, method.Field);
            }
            il.Emit(OpCodes.Ret);
            return type.CreateTypeInfo().AsType();
        }

        /// <summary>
        /// Gets the singleton instance of a control class.
        /// </summary>
        /// <returns>The singleton instance.</returns>
        /// <param name="type">
        /// The interface that the return control needs to implement.
        /// </param>
        internal object GetSingleton(Type type) {
            ControlManager manager;
            if (Controls.ContainsKey(type)) {
                manager = Controls[type];
            } else {
                manager = Controls[type] = ControlManager.Create(this, type);
            }
            return manager.Singleton;
        }

        /// <summary>
        /// Gets the singleton instance of a control class.
        /// </summary>
        /// <returns>The singleton instance.</returns>
        /// <typeparam name="T">
        /// The interface that the return control needs to implement.
        /// </typeparam>
        public T GetSingleton<T>() {
            return (T) GetSingleton(typeof(T));
        }

        /// <summary>
        /// Creates a new instance of a control class.
        /// </summary>
        /// <returns>The new instance.</returns>
        /// <param name="type">
        /// The interface that the return control needs to implement.
        /// </param>
        internal object Create(Type type) {
            ControlManager manager;
            if (Controls.ContainsKey(type)) {
                manager = Controls[type];
            } else {
                manager = Controls[type] = ControlManager.Create(this, type);
            }
            return manager.Create();
        }

        /// <summary>
        /// Creates a new instance of a control class.
        /// </summary>
        /// <returns>The new instance.</returns>
        /// <typeparam name="T">
        /// The interface that the return control needs to implement.
        /// </typeparam>
        public T Create<T>() {
            return (T) Create(typeof(T));
        }

        /// <summary>
        /// Loads an assembly into the framework and discovers its control
        /// implementations.
        /// </summary>
        /// <param name="asm">The assembly to load.</param>
        void LoadAssembly(Assembly asm) {
            foreach (Type asmType in asm.GetExportedTypes()) {
                TypeInfo type = asmType.GetTypeInfo();
                foreach (CustomAttributeData data in type.CustomAttributes) {
                    if (data.AttributeType.FullName == typeof(ControlAttribute).FullName) {
                        ControlAttribute attr = new ControlAttribute((int) data.ConstructorArguments[0].Value);
                        ControlImplementation impl = new ControlImplementation(asmType, type, attr);
                        foreach (Type iface in type.ImplementedInterfaces) {
                            ControlManager manager;
                            if (Controls.ContainsKey(iface)) {
                                manager = Controls[iface];
                            } else {
                                manager = Controls[iface] = ControlManager.Create(this, iface);
                            }
                            manager.Add(impl);
                        }
                    }
                }
            }
            LoadedAssemblies.Add(asm);
        }

        /// <summary>
        /// Loads the framework, or optionally a single assembly into the
        /// currently loaded framework.
        /// </summary>
        /// <param name="assembly">
        /// An assembly to load, or <see cref="null" /> if the entire framework
        /// should be loaded.
        /// </param>
        public void Load(string assembly = null) {
            if (assembly == null) {
                foreach (Assembly asm in Loader.LoadAll()) {
                    LoadAssembly(asm);
                }
            } else {
                LoadAssembly(Loader.LoadFromAssemblyPath(assembly));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCFramework" /> class.
        /// </summary>
        internal IoCFramework() {
            LoadedAssemblies = new List<Assembly>();
            Loader = new AssemblyLoader();
            Controls = new Dictionary<Type, ControlManager>();
            AsmBldr = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(DynamicAsmName), AssemblyBuilderAccess.Run);
            ModBldr = AsmBldr.DefineDynamicModule(DynamicAsmName);
            DynamicsNamespace = string.Format("{0}.{1:X8}.", typeof(IoCFramework).GetTypeInfo().Assembly.GetName().Name, new Random().Next());
        }
    }
}
