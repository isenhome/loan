using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Core.Models
{
    public interface IList<TQuery, TResult>
    {
        IEnumerable<TResult> GetList(TQuery query);

        TResult Get(TQuery query);
    }
}
