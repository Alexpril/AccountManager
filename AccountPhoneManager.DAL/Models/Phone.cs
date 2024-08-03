using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountPhoneManager.DAL.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public bool IsDeleted { get; set; } = true;
    }
}
