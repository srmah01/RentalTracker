﻿using RentalTracker.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalTracker.DAL
{
    public class RentalTrackerContext : DbContext
    {
        public RentalTrackerContext() : base("RentalTracker")
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Payee> Payees { get; set; }

        public DbSet<Category> Catgories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
