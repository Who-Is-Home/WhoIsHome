﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WhoIsHome.DataAccess.Models;

namespace WhoIsHome.DataAccess;

public class WhoIsHomeContext(DbContextOptions<WhoIsHomeContext> options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }
    
    public DbSet<OneTimeEventModel> OneTimeEvents { get; set; }
    
    public DbSet<RepeatedEventModel> RepeatedEvents { get; set; }
    
    public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
    
    public DbSet<UserSettingsModel> UserSettings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OneTimeEventModel
        modelBuilder.Entity<OneTimeEventModel>()
            .Property(e => e.Date)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));
    
        modelBuilder.Entity<OneTimeEventModel>()
            .Property(e => e.StartTime)
            .HasConversion(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));
    
        modelBuilder.Entity<OneTimeEventModel>()
            .Property(e => e.EndTime)
            .HasConversion(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));
    
        modelBuilder.Entity<OneTimeEventModel>()
            .Property(e => e.DinnerTime)
            .HasConversion(
                t => t.HasValue ? t.Value.ToTimeSpan() : (TimeSpan?)null,
                t => t.HasValue ? TimeOnly.FromTimeSpan(t.Value) : null);
        
        // RepeatedEventModel
        modelBuilder.Entity<RepeatedEventModel>()
            .Property(e => e.FirstOccurrence)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));
        
        modelBuilder.Entity<RepeatedEventModel>()
            .Property(e => e.LastOccurrence)
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));
    
        modelBuilder.Entity<RepeatedEventModel>()
            .Property(e => e.StartTime)
            .HasConversion(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));
    
        modelBuilder.Entity<RepeatedEventModel>()
            .Property(e => e.EndTime)
            .HasConversion(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));
    
        modelBuilder.Entity<RepeatedEventModel>()
            .Property(e => e.DinnerTime)
            .HasConversion(
                t => t.HasValue ? t.Value.ToTimeSpan() : (TimeSpan?)null,
                t => t.HasValue ? TimeOnly.FromTimeSpan(t.Value) : null);
    }
}

// Used for EF Core Migrations
// ReSharper disable once UnusedType.Global
public class WhoIsHomeContextFactory : IDesignTimeDbContextFactory<WhoIsHomeContext>
{
    public WhoIsHomeContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<WhoIsHomeContext>();
        optionBuilder.UseMySQL("Server=localhost;Database=local-whoishome;User=root;Password=1234");
        return new WhoIsHomeContext(optionBuilder.Options);
    }
}