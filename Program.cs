using MySql.Data.MySqlClient;

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
        private void ShowTables(MySqlConnection conn)
        {
            List<String> tableNames = new List<string>();
            string query = "show tables";
            MySqlCommand command = new MySqlCommand(query, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
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

        private void ShowUserTable(MySqlConnection conn)
        {
            string query = "select * from User";
            MySqlCommand command = new MySqlCommand(query, conn);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
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
