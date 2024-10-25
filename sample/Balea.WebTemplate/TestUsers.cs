namespace Balea.WebTemplate.Services
{
    public class TestUsers
    {
        public static List<User> Users { get; }

        static TestUsers()
        {
            Users = [
                new() {
                    SubjectId = "818727",
                    Username = "alice",
                },
                new() {
                    SubjectId = "88421113",
                    Username = "bob",
                },
                new() {
                    SubjectId = "88421114",
                    Username = "mary",
                }
            ];
        }
    }
}