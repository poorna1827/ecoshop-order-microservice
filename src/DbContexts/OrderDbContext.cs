﻿using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Models;

namespace OrderMicroservice.DbContexts
{
    public class OrderDbContext : DbContext
    {

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }


        public DbSet<Order> Orders { get; set; }
    }
}
