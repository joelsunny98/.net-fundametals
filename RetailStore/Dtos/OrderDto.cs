﻿namespace RetailStore.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public List<OrderDetailDto> Details { get; set; }

    }
}
