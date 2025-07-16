using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public class LabourerReg
    {
        public int LabourerId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Skill { get; set; }
        public string Experience { get; set; }
        public string Price { get; set; }
        public string LoggedInUserEmail { get; set; }
    public List<Review> Reviews { get; set; } = new List<Review>();
    }

    public class Review
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
