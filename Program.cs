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
        static void Main()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Uid=user;Pwd=password;Database=db";

            var connection = new MySqlConnection(connectionString);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    var users = db.Query().From("User").Get<User>();
                    foreach (var user in users)
                        Console.WriteLine(user.ID + ", " + user.Name + ", " + user.Email + ", " + user.Age);
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
