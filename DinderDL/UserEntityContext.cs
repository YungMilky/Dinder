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

        /* Overrides modelbuilder used for migrations
         * Sets composite keys in the many-to-many relationship between users and posts 
         * (can't be done from the model with annotations)
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //define userID - PostID relationship
            modelBuilder.Entity<UserPosts>().HasKey(up => new { up.UserID, up.PostID }); 

            modelBuilder.Entity<Friendship>()
                .HasOne(p => p.UserOne)
                .WithMany(t => t.Friend1)
                .HasForeignKey(m => m.User1ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(p => p.UserTwo)
                .WithMany(t => t.Friend2)
                .HasForeignKey(m => m.User2ID)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostsEntity> Posts { get; set; }
        public DbSet<UserPosts> UserPosts { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
    }
}
