using Dapper;
using MySql.Data.MySqlClient;

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

            connection.Open();

            string sql = "SELECT * FROM User";
            var users = connection.Query<User>(sql);

            foreach (var user in users)
            {
                Console.WriteLine(user.Name + ", " + user.ID);
            }
        }
    }
}
