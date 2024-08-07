namespace Template.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsComplete { get; set; }
        public bool IsActive { get; set; }
    }
}
