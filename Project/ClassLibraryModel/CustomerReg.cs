using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public class CustomerReg
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string LoggedInUserEmail { get; set; }
        public bool Done { get; set; }

    }
}
