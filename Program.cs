using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;


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

        private void ShowUserTable(QueryFactory db)
        {
            var users = db.Query().From("User").Get<User>();
            foreach (var user in users)
            {
                Console.WriteLine(user.ID + ", " + user.Name + ", " + user.Email + ", " + user.Age);
            }
        }

        static void Main()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Uid=user;Pwd=password;Database=db";

            var connection = new MySqlConnection(connectionString);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            Program p = new Program();

            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {

                try
                {
                    p.ShowTables(connection);
                    p.ShowUserTable(db);
                    scope.Commit();
                }
                catch
                {
                    scope.Rollback();
                    Console.WriteLine("roll back");
                }
            }
            db.Connection.Close();
        }
    }
}
