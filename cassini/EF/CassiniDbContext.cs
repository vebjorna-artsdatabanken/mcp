using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace cassini.EF;

/// <summary>
/// Database context for accessing Cassini mission data
/// </summary>
public partial class CassiniDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the CassiniDbContext
    /// </summary>
    public CassiniDbContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the CassiniDbContext with options
    /// </summary>
    /// <param name="options">DbContext configuration options</param>
    public CassiniDbContext(DbContextOptions<CassiniDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the mission activity observations
    /// </summary>
    public virtual DbSet<MissionActivity> MissionActivities { get; set; }

    /// <summary>
    /// Configures the database connection
    /// </summary>
    /// <param name="optionsBuilder">Options builder for configuration</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuration is handled through dependency injection in Program.cs
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Data/master_plan.db");
        }
    }

    /// <summary>
    /// Configures entity models and relationships
    /// </summary>
    /// <param name="modelBuilder">Model builder for entity configuration</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MissionActivity>(entity =>
        {
            entity.ToTable("master_plan");

            entity.HasIndex(e => e.Date, "idx_date");

            entity.HasIndex(e => e.SpassType, "idx_spass_type");

            entity.HasIndex(e => e.Target, "idx_target");

            entity.HasIndex(e => e.Team, "idx_team");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.LibraryDefinition).HasColumnName("library_definition");
            entity.Property(e => e.RequestName).HasColumnName("request_name");
            entity.Property(e => e.SpassType).HasColumnName("spass_type");
            entity.Property(e => e.StartTimeUtc).HasColumnName("start_time_utc");
            entity.Property(e => e.Target).HasColumnName("target");
            entity.Property(e => e.Team).HasColumnName("team");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
