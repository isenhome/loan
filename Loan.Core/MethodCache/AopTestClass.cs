using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public class TempParams
    {
        public string word { get; set; }
        public string keyword { get; set; }
    }
    public class AopTestClass
    {
        public AopTestClass()
        {
        }

        [MethodCache(Description = "123", DurationMinutes = 35)]
        public virtual string TestMethod(string word, string keyword)
        {
            //throw new Exception("test exception");
            return "Hello: " + word;
        }

        [MethodCache(Description = "TestKey", DurationMinutes = 35)]
        public virtual int TestMethod2(int i)
        {
            return i * i;
        }

        [MethodCache(Description = "TestKey", DurationMinutes = 35)]
        public virtual void TestMethod3(int i)
        {
            int result = i * i;
            //Console.WriteLine(result);
        }

        [MethodCache(Description = "TestKey", DurationMinutes = 35)]
        public virtual string TestMethod4(TempParams param)
        {
            return "this is reuslt";
        }
        [MethodCache(DurationMinutes = 35)]
        public virtual string TestMethodCache(int minute)
        {
            Thread.Sleep(minute * 1000);
            return "run over";
        }

        /// <summary>
        /// mock emit code
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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

        public static void Invoke()
        {
            AopTestClass instance = new AopTestClass();
            AopTestClass proxyInstance = AOPFactory.CreateInstance<AopTestClass>();

            Console.WriteLine(proxyInstance.TestMethod("Jack", "keyword"));

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < 1000000; ++i)
            {
                //proxyInstance.TestMethod2(i);
                //instance.MockAopTest(i);


                AOPFactory.CreateInstance<AopTestClass>().TestMethod2(i);
                //new AopTestClass().MockAopTest(i);
            }

            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }

    public class AopTestClass1
    {
        [MethodCache(DurationMinutes = 35)]
        public virtual string TestMethodCache(int minute)
        {
            Thread.Sleep(minute * 1000);
            return "run over";
        }
    }

    public class AopTestClass2
    {

        public AopTestClass2()
        {

        }

        public string property { get; set; }
        public AopTestClass2(string property)
        {
            this.property = property;   
        }
        [MethodCache(DurationMinutes = 35)]
        public virtual string TestMethodCache(int minute)
        {
            Thread.Sleep(minute * 1000);
            return "run over";
        }
    }
}