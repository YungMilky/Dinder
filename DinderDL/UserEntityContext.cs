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
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FilesEntity> Files { get; set; }

        public UserEntityContext(DbContextOptions<UserEntityContext> options) : base(options)
        {
        }

        /* Overrides modelbuilder used for migrations
         * Sets composite keys in the many-to-many relationship between users and posts 
         * (can't be done from the model with annotations)
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilesEntity>()
                .HasKey(f => f.FileID);

            modelBuilder.Entity<UserEntity>()
                .HasOne<FilesEntity>(f => f.File)
                .WithOne(f => f.User);

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
            
            //constrain to unique Friend1ID+Friend2ID combination
            modelBuilder.Entity<Friendship>(u =>
            {
                u.HasIndex(f => new { f.Friend1ID, f.Friend2ID }).IsUnique();
            });

            modelBuilder.Entity<PostsEntity>()
                .HasOne(u => u.Poster)
                .WithMany(u => u.SentPosts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostsEntity>()
                .HasOne(u => u.Receiver)
                .WithMany(u => u.ReceivedPosts)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
