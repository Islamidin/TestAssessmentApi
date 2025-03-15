﻿using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
                    .HasIndex(p => p.FirstName)
                    .IsUnique(false);

        modelBuilder.Entity<Person>()
                    .HasIndex(p => p.LastName)
                    .IsUnique(false);
    }
}