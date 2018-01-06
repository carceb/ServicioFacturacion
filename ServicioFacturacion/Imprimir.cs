using Database.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.IO;
namespace ServicioFacturacion
{
        class Imprimir
    {
        static string resultado;
        public static string ImprimirCola()
        {
            string archivoFactura = @"C:\Users\Public\Factura.txt";
            string titulo = "******* FACTURA DE EJEMPLO CREADA POR EL SERVICIO WINDOWS *******";
            string factura = "";
            string fecha = "";
            string cliente = "";
            string producto = "";
            string precio = "";
            string cantidad = "";
            resultado = "";
            try
            {
                //OBTIENE LA FACTURA EN COLA A IMPRIMIR
                SqlParameter[] dbParams = new SqlParameter[]
                 {

                 };

                SqlDataReader dr = DBHelper.ExecuteDataReader("usp_ObtenerColaImpresion", dbParams);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        factura = dr.GetInt32(0).ToString();
                        fecha = dr.GetDateTime(1).ToString();
                        cliente = dr.GetString(2).ToString();
                        producto = dr.GetString(3).ToString();
                        precio = dr.GetSqlMoney(4).ToString();
                        cantidad = dr.GetInt32(5).ToString();
                    }
                }
                dr.Close();

                //SI EXISTE COLA DE IMPRESION
                if (factura != "")
                {
                    // ESCRIBE LOS DATOS A IMPRIMIR EN UN ARCHIVO DE TEXTO
                    string[] lines = { titulo, fecha, cliente, producto, precio, cantidad };
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(archivoFactura))
                    {
                        foreach (string line in lines)
                        {

                            file.WriteLine(line);
                        }
                    }
                    //ENVIA LOS DATOS DE LA FACTURA A LA IMPRESORA
                    //to do
                    //***********************************************

                    // ACTUALIZA LA FACTURA YA IMPRESA
                    ActualizarFacturaImpresa(Convert.ToInt32(factura));
                    resultado = "Factura número " + factura + " impresa correctamente";
 
                }
                else
                {
                    //ELIMINA EL ARCHIVO YA IMPRESO
                    if (System.IO.File.Exists(archivoFactura))
                    {
                        try
                        {
                            System.IO.File.Delete(archivoFactura);
                            resultado = "Archivo factura " + archivoFactura + " eliminado correctamente";
                        }
                        catch (System.IO.IOException e)
                        {

                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        resultado = "";
                    }
                }
                // ***********************************************************************
            }
            catch (Exception e)
            {
                resultado = "Factura error " + factura + " error al intentar imprimir " + e.StackTrace;
            }


            return resultado;
        }

        
        private static int ActualizarFacturaImpresa(int facturaID) 
        {
            SqlParameter[] dbParams = new SqlParameter[]
            {
                    DBHelper.MakeParam("@FacturaID", SqlDbType.Int, 0, facturaID)

            };
            return Convert.ToInt32(DBHelper.ExecuteScalar("usp_ServicioImpresion_ActualizarColaImpresion", dbParams));
        }
    }
}
