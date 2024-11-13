namespace Profescipta_Sales_Order.Models
{
    public class Order
    {
        public SoOrder SalesHeader { get; set; }
        public List<SoItem> Items { get; set; }
    }
}
