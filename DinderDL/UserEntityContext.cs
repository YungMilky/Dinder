using DinderDL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DinderDL
{
    public class UserEntityContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostsEntity> Posts { get; set; }
        public DbSet<UserPosts> UserPosts { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

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

            //modelBuilder.Entity<UserEntity>().Property(ol => ol.Friend1ID).IsRequired(true);
            //modelBuilder.Entity<UserEntity>().Property(ol => ol.Friend2ID).IsRequired(true);

            modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Friend1)
            .WithMany(t => t.Friendship1)
            .HasForeignKey(t => t.Friend1ID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Friend2)
            .WithMany(t => t.Friendship2)
            .HasForeignKey(t => t.Friend2ID)
            .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserEntity>()
            //    .HasOne(p => p.Friend1ID)
            //    .WithMany(t => t.Friend1)
            //    .HasForeignKey(m => m.UserID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserEntity>()
            //    .HasOne(p => p.Friend2ID)
            //    .WithMany(t => t.Friend2)
            //    .HasForeignKey(m => m.UserID)
            //    .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
