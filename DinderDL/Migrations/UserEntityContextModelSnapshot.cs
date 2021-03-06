﻿// <auto-generated />
using System;
using DinderDL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DinderDL.Migrations
{
    [DbContext(typeof(UserEntityContext))]
    partial class UserEntityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DinderDL.Models.FilesEntity", b =>
                {
                    b.Property<int>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Filename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FileID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("Files");
                });

            modelBuilder.Entity("DinderDL.Models.Friendship", b =>
                {
                    b.Property<int>("FriendshipID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Friend1ID")
                        .HasColumnType("int");

                    b.Property<int>("Friend2ID")
                        .HasColumnType("int");

                    b.Property<bool>("FriendStatus")
                        .HasColumnType("bit");

                    b.HasKey("FriendshipID");

                    b.HasIndex("Friend2ID");

                    b.HasIndex("Friend1ID", "Friend2ID")
                        .IsUnique();

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("DinderDL.Models.PostsEntity", b =>
                {
                    b.Property<int>("PostID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PosterID")
                        .HasColumnType("int");

                    b.Property<int>("ReceiverID")
                        .HasColumnType("int");

                    b.HasKey("PostID");

                    b.HasIndex("PosterID");

                    b.HasIndex("ReceiverID");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("DinderDL.Models.UserEntity", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Phone")
                        .HasColumnType("int");

                    b.Property<byte[]>("ProfilePic")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DinderDL.Models.FilesEntity", b =>
                {
                    b.HasOne("DinderDL.Models.UserEntity", "User")
                        .WithOne("File")
                        .HasForeignKey("DinderDL.Models.FilesEntity", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DinderDL.Models.Friendship", b =>
                {
                    b.HasOne("DinderDL.Models.UserEntity", "Friend1")
                        .WithMany("Friendship1")
                        .HasForeignKey("Friend1ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DinderDL.Models.UserEntity", "Friend2")
                        .WithMany("Friendship2")
                        .HasForeignKey("Friend2ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Friend1");

                    b.Navigation("Friend2");
                });

            modelBuilder.Entity("DinderDL.Models.PostsEntity", b =>
                {
                    b.HasOne("DinderDL.Models.UserEntity", "Poster")
                        .WithMany("SentPosts")
                        .HasForeignKey("PosterID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DinderDL.Models.UserEntity", "Receiver")
                        .WithMany("ReceivedPosts")
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Poster");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("DinderDL.Models.UserEntity", b =>
                {
                    b.Navigation("File");

                    b.Navigation("Friendship1");

                    b.Navigation("Friendship2");

                    b.Navigation("ReceivedPosts");

                    b.Navigation("SentPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
