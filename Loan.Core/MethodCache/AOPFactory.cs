using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;
using Common.Logging;


//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public class AOPFactory
    {
        private const string ASSEMBLY_NAME = "EmitWraper";
        private const string MODULE_NAME = "EmitModule_";
        private const string TYPE_NAME = "Emit_";
        private const TypeAttributes TYPE_ATTRIBUTES = TypeAttributes.Public | TypeAttributes.Class;
        private const MethodAttributes METHOD_ATTRIBUTES = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AOPFactory));

        private static readonly Hashtable typeCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// CreateInstance from T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(params object[] parameters) where T : class, new()
        {
            Type baseType = typeof(T);
            Type proxyType = typeCache[baseType] as Type;

            if (proxyType == null)
            {
                lock (typeCache.SyncRoot)
                {
                    proxyType = BuilderType(baseType);
                    if (!typeCache.ContainsKey(baseType))
                    {
                        typeCache.Add(baseType, proxyType);
                    }
                }
            }

            //return (T)Activator.CreateInstance(proxyType, parameters);
            return (T)FastObjectCreator.CreateObject(proxyType, parameters);
        }

        #region build proxy type

        #region BuilderType
        private static Type BuilderType(Type baseType)
        {
            AssemblyName an = new AssemblyName { Name = ASSEMBLY_NAME };
            an.SetPublicKey(Assembly.GetExecutingAssembly().GetName().GetPublicKey());

            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assembly.DefineDynamicModule(MODULE_NAME + baseType.Name);

            TypeBuilder typeBuilder = module.DefineType(TYPE_NAME + baseType.Name, TYPE_ATTRIBUTES, baseType);

            BuildConstructor(baseType, typeBuilder);

            BuildMethod(baseType, typeBuilder);

            Type type = typeBuilder.CreateType();

            return type;
        }
        #endregion

        #region BuildConstructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeBuilder"></param>
        private static void BuildConstructor(Type baseType, TypeBuilder typeBuilder)
        {
            foreach (var ctor in baseType.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var parameterTypes = ctor.GetParameters().Select(u => u.ParameterType).ToArray();
                var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);

                ILGenerator il = ctorBuilder.GetILGenerator();
                for (int i = 0; i <= parameterTypes.Length; ++i)
                {
                    LoadArgument(il, i);
                }
                il.Emit(OpCodes.Call, ctor);
                il.Emit(OpCodes.Ret);
            }
        }
        #endregion

        #region BuildMethod
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeBuilder"></param>
        /*
        public virtual int MockAopTest(int i)
        {
            int result = 0;
            MethodContext context = new MethodContext();
            context.ClassName = "AopTestClass";
            context.MethodName = "MockAopTest";
            context.Executor = this;
            context.Parameters = new object[1];
            context.Parameters[0] = i;
            context.Processed = false;
            context.ReturnValue = result;

            ICallHandler[] handlers = new ICallHandler[1];
            NameValueCollection attrCollection = new NameValueCollection();
            attrCollection.Add("CacheKey", "TestKey");
            attrCollection.Add("DurationMinutes", "35");
            handlers[0] = new CacheCallHandler(attrCollection);

            for (int c = 0; c < handlers.Length; ++c)
            {
                handlers[c].BeginInvoke(context);
            }

            if (context.Processed == false)
            {
                try
                {
                    result = TestMethod2(i);
                    context.ReturnValue = result;
                }
                catch (Exception ex)
                {
                    context.Exception = ex;
                    for (int c = 0; c < handlers.Length; ++c)
                    {
                        handlers[c].OnException(context);
                    }
                }
            }

            for (int c = 0; c < handlers.Length; ++c)
            {
                handlers[c].EndInvoke(context);
            }

            return result;
        }
        */
        private static void BuildMethod(Type baseType, TypeBuilder typeBuilder)
        {
            foreach (var methodInfo in baseType.GetMethods())
            {
                if (!methodInfo.IsVirtual && !methodInfo.IsAbstract) continue;
                object[] attrs = methodInfo.GetCustomAttributes(typeof(AspectAttribute), true);
                int attrCount = attrs.Length;
                if (attrCount == 0) continue;

                ParameterInfo[] parameters = methodInfo.GetParameters();
                Type[] parameterTypes = parameters.Select(u => u.ParameterType).ToArray();

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    methodInfo.Name,
                    METHOD_ATTRIBUTES,
                    methodInfo.ReturnType,
                    parameterTypes);

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, parameters[i].Attributes, parameters[i].Name);
                }
                methodBuilder.SetParameters(parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                // MethodContext context = new MethodContext();
                LocalBuilder localContext = il.DeclareLocal(typeof(MethodContext));

                #region init context
                il.Emit(OpCodes.Newobj, typeof(MethodContext).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc, localContext);
                // context.MethodName = m.Name;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, methodInfo.Name);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_MethodName"), new[] { typeof(string) });
                // context.ClassName = m.DeclaringType.Name;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, methodInfo.DeclaringType.Namespace + "." + methodInfo.DeclaringType.Name);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_ClassName"), new[] { typeof(string) });
                // context.Executor = this;
                //il.Emit(OpCodes.Ldloc, localContext);
                //il.Emit(OpCodes.Ldarg_0);
                //il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Executor"), new[] { typeof(object) });

                //content.DurationMinutes =
                MethodCacheAttribute[] methodCache = (MethodCacheAttribute[])methodInfo.GetCustomAttributes(typeof(MethodCacheAttribute), true);
                int durationMinute = 30;
                var description = "";
                if (methodCache.Count() > 0)
                {
                    durationMinute = methodCache[0].DurationMinutes;
                    description = methodCache[0].Description.ToString();
                }
                if (String.IsNullOrWhiteSpace(description))
                {

                    description = methodInfo.DeclaringType.Namespace + "." + methodInfo.DeclaringType.Name + "." + methodInfo.Name;
                }
                il.Emit(OpCodes.Ldloc, localContext);
                //il.Emit(OpCodes.Ldstr, durationMinute);
                il.Emit(OpCodes.Ldc_I4, durationMinute);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_KeepMinutes"), new[] { typeof(int) });
                //context.Description = 
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, description);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Description"), new[] { typeof(object) });

                var dllPath = methodInfo.DeclaringType.Module.Name;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, dllPath);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_DllPath"), new[] { typeof(object) });

                #endregion

                // set context.Parameters
                #region context.Parameters = new object[Length];
                LocalBuilder tmpParameters = il.DeclareLocal(typeof(object[]));
                il.Emit(OpCodes.Ldc_I4, parameters.Length);
                il.Emit(OpCodes.Newarr, typeof(object));
                il.Emit(OpCodes.Stloc, tmpParameters);
                for (int i = 0; i < parameters.Length; ++i)
                {
                    il.Emit(OpCodes.Ldloc, tmpParameters);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldarg, i + 1);
                    il.Emit(OpCodes.Box, parameterTypes[i]);
                    //il.EmitCall(OpCodes.Call, typeof(object).GetMethod("GetType", new Type[] { }), null);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldloc, tmpParameters);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Parameters"), new[] { typeof(object[]) });
                #endregion

                LocalBuilder localReturnValue = null;
                if (methodInfo.ReturnType != typeof(void)) // has return value
                {
                    localReturnValue = il.DeclareLocal(methodInfo.ReturnType);
                }

                // ICallHandler[] handlers = new ICallHandler[attrCount];
                LocalBuilder localHandlers = il.DeclareLocal(typeof(ICallHandler[]));
                il.Emit(OpCodes.Ldc_I4, attrCount);
                il.Emit(OpCodes.Newarr, typeof(ICallHandler));
                il.Emit(OpCodes.Stloc, localHandlers);

                // create ICallHandler instance
                #region create ICallHandler instance
                for (int i = 0; i < attrCount; ++i)
                {
                    LocalBuilder tmpNameValueCollection = il.DeclareLocal(typeof(NameValueCollection));
                    il.Emit(OpCodes.Newobj, typeof(NameValueCollection).GetConstructor(Type.EmptyTypes));
                    il.Emit(OpCodes.Stloc, tmpNameValueCollection);

                    AspectAttribute attr = (attrs[i] as AspectAttribute);
                    NameValueCollection attrCollection = attr.GetAttrs();
                    foreach (var key in attrCollection.AllKeys)
                    {
                        il.Emit(OpCodes.Ldloc, tmpNameValueCollection);
                        il.Emit(OpCodes.Ldstr, key);
                        il.Emit(OpCodes.Ldstr, attrCollection[key]);
                        il.Emit(OpCodes.Callvirt, typeof(NameValueCollection).GetMethod("Add", new[] { typeof(string), typeof(string) }));
                    }

                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldloc, tmpNameValueCollection);
                    il.Emit(OpCodes.Newobj, attr.CallHandlerType.GetConstructor(new[] { typeof(NameValueCollection) }));
                    il.Emit(OpCodes.Stelem_Ref);
                }
                #endregion

                // BeginInvoke
                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("BeginInvoke"));
                }

                Label endLabel = il.DefineLabel(); // if (context.Processed) goto: ...
                il.Emit(OpCodes.Ldloc, localContext);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("get_Processed"), Type.EmptyTypes);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Beq, endLabel);
                // excute base method
                LocalBuilder localException = il.DeclareLocal(typeof(Exception));
                il.BeginExceptionBlock(); // try {

                il.Emit(OpCodes.Ldloc, localContext);

                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < parameterTypes.Length; ++i)
                {
                    LoadArgument(il, i + 1);
                }
                il.EmitCall(OpCodes.Call, methodInfo, parameterTypes);
                // is has return value, save it
                if (methodInfo.ReturnType != typeof(void))
                {
                    if (methodInfo.ReturnType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, methodInfo.ReturnType);
                    }
                    il.Emit(OpCodes.Stloc, localReturnValue);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Ldloc, localReturnValue);
                    il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_ReturnValue"), new[] { typeof(object) });
                }

                il.BeginCatchBlock(typeof(Exception)); // } catch {
                // OnException
                il.Emit(OpCodes.Stloc, localException);
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldloc, localException);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Exception"), new[] { typeof(Exception) });

                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("OnException"));
                }

                il.EndExceptionBlock(); // }
                // end excute base method


                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("EndInvoke"));
                }

                if (methodInfo.ReturnType != typeof(void))
                {
                    il.Emit(OpCodes.Ldloc, localReturnValue);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }
                il.EmitWriteLine("in side return");
                il.Emit(OpCodes.Ret);
                il.MarkLabel(endLabel);

                // EndInvoke
                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("EndInvoke"));
                }

                if (methodInfo.ReturnType != typeof(void))
                {
                    if (methodInfo.ReturnType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, methodInfo.ReturnType);
                    }
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("get_ReturnValue"), new[] { typeof(object) });
                    il.Emit(OpCodes.Stloc, localReturnValue);
                    il.Emit(OpCodes.Ldloc, localReturnValue);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }
                il.EmitWriteLine("out side return ");
                il.Emit(OpCodes.Ret);
            }
        }
        #endregion

        #region LoadArgument
        /// <summary>
        /// LoadParameter
        /// </summary>
        /// <param name="il"></param>
        /// <param name="index"></param>
        public static void LoadArgument(ILGenerator il, int index)
        {
            switch (index)
            {
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
                    if (index <= 127)
                    {
                        il.Emit(OpCodes.Ldarg_S, index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }
        #endregion

        #endregion
    }
}
