using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp_Final_login_ventas
{
    public class Conexion
    {
        public static SqlConnection Conectar()
        {
            SqlConnection cn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Log_TP_Prog;Data Source=LAPTOP-2K8IVQBJ");
            cn.Open();//abre conexion
            return cn;//retorna el objeto conexion contodos los datos en bruto
        }
        

    }
}
