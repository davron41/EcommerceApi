using Twewew.Common;

namespace Twewew.Entities;

public class Product : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public byte[] ImageUrl { get; set; }
    public DateTime AddedDate { get; set; }
    public bool IsFrozen { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public Product()
    {
        Orders = new HashSet<Order>();
        OrderItems = new List<OrderItem>();
    }
}
