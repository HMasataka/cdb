namespace CDB
{
    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string id, string name, int age)
        {
            ID = id;
            Name = name;
            Age = age;
        }
    }
}
