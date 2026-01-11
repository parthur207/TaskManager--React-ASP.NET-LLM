using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Persistence.DataBase.Interface
{
    public interface IDbConnectionFactory
    {
        SqlConnection Create();  
    }
}
