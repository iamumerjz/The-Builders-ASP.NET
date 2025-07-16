using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryDal
{
    public class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection("Data Source=UMER\\SQLEXPRESS;Initial Catalog=Project;Integrated Security=True");
            return con;
        }
    }
}
