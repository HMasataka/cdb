using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace CDB
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Int16 Age { get; set; }

        public User(int id, string name, string email, Int16 age)
        {
            ID = id;
            Name = name;
            Email = email;
            Age = age;
        }
    }


    class Program
    {
        private void ShowTables(IDbConnection conn)
        {
            List<String> tableNames = new List<string>();
            IDbCommand command = conn.CreateCommand();
            command.CommandText = "show tables";

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }
            foreach (var t in tableNames)
            {
                Console.WriteLine(t);
            }
        }

        private void ShowUserTable(IDbConnection conn)
        {
            IDbCommand command = conn.CreateCommand();
            command.CommandText = "select * from User";

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    User user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt16(3));
                    Console.WriteLine($"ID:{user.ID} Name:{user.Name} Email:{user.Email} Age:{user.Age}");
                    Console.WriteLine($"ID:{reader["id"]} Name:{reader["name"]} Email:{reader["email"]} Age:{reader["age"]}");
                }
            }
        }

        static void Main()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Uid=user;Pwd=password;Database=db";

            var connection = new MySqlConnection(connectionString);

            Program p = new Program();

            connection.Open();
            using (var scope = connection.BeginTransaction())
            {
                try
                {
                    p.ShowTables(connection);
                    p.ShowUserTable(connection);
                    scope.Commit();
                }
                catch
                {
                    scope.Rollback();
                    Console.WriteLine("roll back");
                }
            }
            connection.Close();
        }
    }
}
