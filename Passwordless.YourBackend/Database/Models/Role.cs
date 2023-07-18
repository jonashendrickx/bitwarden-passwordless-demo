using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Passwordless.YourBackend.Database.Models;

public class Role
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<User> Users { get; set; } = new();
}