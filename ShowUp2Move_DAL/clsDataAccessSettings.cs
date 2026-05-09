using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ShowUp2Move_DAL
{
    public static class clsDataAccessSettings
    {
        public static string ConnectionString { get; set; } = string.Empty;
    }
}
