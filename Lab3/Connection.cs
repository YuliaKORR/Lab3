using Lab3.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Lab3
{
    public class Connection
    {
        static async Task ConnectionToSQL()
        {
            string connectionString = "Server=LAPTOP-FKQ2OA17\\SQLEXPRESS01; Encrypt=False; User=LAPTOP-FKQ2OA17\\yulia; Initial Catalog=ThirdLab; Database=ThirdLab; Integrated Security=SSPI; TrustServerCertificate=True";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                await connection.OpenAsync();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // если подключение открыто
                if (connection.State == ConnectionState.Open)
                {
                    // закрываем подключение
                    await connection.CloseAsync();
                    Console.WriteLine("Подключение закрыто...");
                }
            }

        }
    
    }
}
