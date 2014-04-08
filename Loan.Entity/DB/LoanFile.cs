using Loan.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.DB
{
    [PetaPoco.TableName("LoanFile")]
    public class LoanFile:BaseDB
    {
        [PetaPoco.Column]
        public string Guid { get; set; }
        [PetaPoco.Column]
        public string Name { get; set; }
        [PetaPoco.Column]
        public string OriginalName { get; set; }
        [PetaPoco.Column]
        public LoanFileType Type { get; set; }

    }
}
