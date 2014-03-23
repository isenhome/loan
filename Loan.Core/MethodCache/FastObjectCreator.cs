using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public static class FastObjectCreator
    {
        private delegate object CreateOjectHandler(object[] parameters);
        private static readonly Hashtable creatorCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// CreateObject
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object CreateObject(Type type, params object[] parameters)
        {
            string token = type.BaseType.FullName;
            Type[] parameterTypes = GetParameterTypes(ref token, parameters);

            CreateOjectHandler ctor = creatorCache[token] as CreateOjectHandler;
            if (ctor == null)
            {
                lock (creatorCache.SyncRoot)
                {
                    ctor = CreateHandler(type, parameterTypes);
                    if (ctor != null)
                    {
                        creatorCache.Add(token, ctor);
                    }
                }
            }
            //CreateOjectHandler ctor = CreateHandler(type, parameterTypes);
            return ctor.Invoke(parameters);
        }

        /// <summary>
        /// CreateHandler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramsTypes"></param>
        /// <returns></returns>
        private static CreateOjectHandler CreateHandler(Type type, Type[] paramsTypes)
        {
            DynamicMethod method = new DynamicMethod("DynamicCreateOject", typeof(object),
                new Type[] { typeof(object[]) }, typeof(CreateOjectHandler).Module);

            ConstructorInfo constructor = type.GetConstructor(paramsTypes);

            ILGenerator il = method.GetILGenerator();

            for (int i = 0; i < paramsTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                if (paramsTypes[i].IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, paramsTypes[i]);
                }
                else
                {
                    il.Emit(OpCodes.Castclass, paramsTypes[i]);
                }
            }
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            return (CreateOjectHandler)method.CreateDelegate(typeof(CreateOjectHandler));
        }

        /// <summary>
        /// GetParameterTypes
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Type[] GetParameterTypes(ref string token, params object[] parameters)
        {
            if (parameters == null) return new Type[0];
            Type[] values = new Type[parameters.Length];
            var list = new List<string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                values[i] = parameters[i].GetType();
                list.Add(values[i].FullName);
                //token = token + "." + values[i].FullName;
            }
            token = token + String.Format("({0})", string.Join(",", list));
            return values;
        }

        public static void ClearCache()
        {
            creatorCache.Clear();
        }
    }
}
