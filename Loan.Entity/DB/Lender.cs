using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.DB
{
   [PetaPoco.TableName("Lender")]
    public class Lender:BaseDB
    {
       [PetaPoco.Column]
        public int ID { get; set; }
       [PetaPoco.Column]
        public string Name { get; set; }
       [PetaPoco.Column]
        public string Identity { get; set; }
       [PetaPoco.Column]
        public DateTime Birthday { get; set; }
    }
}
