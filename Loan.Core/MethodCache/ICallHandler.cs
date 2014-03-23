//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public interface ICallHandler
    {
        void BeginInvoke(MethodContext context);

        void EndInvoke(MethodContext context);

        void OnException(MethodContext context);
    }
}