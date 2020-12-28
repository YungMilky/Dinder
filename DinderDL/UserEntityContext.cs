using DinderDL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DinderDL
{
    public class UserEntityContext : DbContext
    {
        public UserEntityContext(DbContextOptions<UserEntityContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
