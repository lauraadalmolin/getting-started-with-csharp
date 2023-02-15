using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models
{
    public class User
    {
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public State State { get; set; }
        public string? PostalCode { get; set; }
        public AddressType AddressType { get; set; }
        public DateTime CreatedAt { get; set; }

    }

    public enum State
    {
        RS, SP, RJ, BA, PA, SC
    }

    public enum AddressType
    {
        House, Apartment, Job
    }
}
