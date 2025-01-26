using Microsoft.AspNetCore.Identity;
using Twewew.Common;
using Twewew.Enums;

namespace Twewew.Entities;

public class Order : EntityBase
{
    public DateTime OrderDate { get; set; }
    public decimal TotalQuantity { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Guid CustomerId { get; set; }
    public virtual IdentityUser<Guid> Customer { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public Order()
    {
        OrderItems = new List<OrderItem>();
    }

}
