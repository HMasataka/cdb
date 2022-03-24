using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;


namespace CDB
{
    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public User(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }


    class Program
    {
        static void Main()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Uid=user;Pwd=password;Database=db";

            var connection = new MySqlConnection(connectionString);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            var users = db.Query().From("User").Get<User>();
            foreach (var user in users)
                Console.WriteLine(user.ID + ", " + user.Name);
        }
    }
}
