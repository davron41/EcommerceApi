using Twewew.Common;

namespace Twewew.Entities;

public class Category : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public Category()
    {
        Products = new HashSet<Product>();
    }
}
