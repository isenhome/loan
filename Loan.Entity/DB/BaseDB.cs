using Loan.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.DB
{
    public class BaseDB
    {
        [PetaPoco.Column]
        public Status Status { get; set; }
        [PetaPoco.Column]
        public DateTime CreateTime { get; set; }
        [PetaPoco.Column]
        public DateTime LastChanged { get; set; }
    }
}
