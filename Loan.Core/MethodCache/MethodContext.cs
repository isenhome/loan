using System;
using System.Reflection;

//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public class MethodContext
    {
        /// <summary>
        /// 
        /// </summary>
        public object Executor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] ParameterTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int KeepMinutes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DllPath { get;set; }

    }
}