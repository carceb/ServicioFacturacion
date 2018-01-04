using Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioFacturacion
{
    class Parametros
    {
        public static string ObtenerParametro(string nombreParametro)
        {
            string valorParametro = "";
            SqlParameter[] dbParams = new SqlParameter[]
             {
                 DBHelper.MakeParam("@NombreParametro", SqlDbType.VarChar, 0, nombreParametro)
             };

            SqlDataReader dr = DBHelper.ExecuteDataReader("usp_ObtenerValorParametro", dbParams);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    valorParametro = dr.GetString(2).ToString();
                }
            }
            dr.Close();
            return valorParametro;
        }
    }
}
