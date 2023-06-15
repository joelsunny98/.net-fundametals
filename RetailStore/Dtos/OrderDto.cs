namespace RetailStore.Dtos
{
    /// <summary>
    /// Data transfer object for querying order table. 
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Gets and sets Unique Identification number for order entity
        /// </summary>
        /// <example> 1 </example> 
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets Name of the customer to be referred
        /// </summary>
        /// <example> "Aleena" </example> 
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets and sets total amount of the order
        /// </summary>
        /// <example> 110.5 </example> 
        public string Amount { get; set; }

        /// <summary>
        /// Gets and sets total amount of the order
        /// </summary>
        /// <example> 110.5 </example> 
        public string Discount { get; set; }

        /// <summary>
        /// Gets and sets amount of the order
        /// </summary>
        /// <example> 70.5 </example> 
        public string TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the details of order.
        /// </summary>
        public List<OrderDetailDto> Details { get; set; }

    }
}
