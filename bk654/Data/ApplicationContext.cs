using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using bk654.Models;

namespace bk654.Data;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeletedWorker> DeletedWorkers { get; set; }

    public virtual DbSet<PerformanceReview> PerformanceReviews { get; set; }

    public virtual DbSet<PerformanceReviewSummary> PerformanceReviewSummaries { get; set; }

    public virtual DbSet<PositionAtWork> PositionAtWorks { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<WorkShift> WorkShifts { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("data source=127.0.0.1;uid=root;pwd=root;database=bk1113", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.19-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DeletedWorker>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("deleted_workers");

            entity.HasIndex(e => e.PositionId, "fk_deleted_workers_position_at_work1_idx");

            entity.HasIndex(e => e.RestaurantId, "fk_deleted_workers_restaurant1_idx");

            entity.HasIndex(e => e.WorkerId, "fk_deleted_workers_worker1_idx");

            entity.Property(e => e.DeletedTimestamp)
                .HasColumnType("timestamp")
                .HasColumnName("deleted_timestamp");
            entity.Property(e => e.DismissalReason)
                .HasMaxLength(100)
                .HasColumnName("dismissal_reason")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(30)
                .HasColumnName("patronymic")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Position).WithMany()
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deleted_workers_position_at_work1");

            entity.HasOne(d => d.Restaurant).WithMany()
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deleted_workers_restaurant1");

            entity.HasOne(d => d.Worker).WithMany()
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deleted_workers_worker1");
        });

        modelBuilder.Entity<PerformanceReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("performance_reviews");

            entity.HasIndex(e => e.WorkerId, "fk_performance_reviews_worker1_idx");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(250)
                .HasColumnName("comments");
            entity.Property(e => e.PerformanceRating).HasColumnName("performance_rating");
            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime")
                .HasColumnName("review_date");
            entity.Property(e => e.ReviewerName)
                .HasMaxLength(25)
                .HasColumnName("reviewer_name");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Worker).WithMany(p => p.PerformanceReviews)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_performance_reviews_worker1");
        });

        modelBuilder.Entity<PerformanceReviewSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("performance_review_summary");

            entity.Property(e => e.AvgRating)
                .HasPrecision(14, 4)
                .HasColumnName("avg_rating");
            entity.Property(e => e.MaxRating).HasColumnName("max_rating");
            entity.Property(e => e.MinRating).HasColumnName("min_rating");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
        });

        modelBuilder.Entity<PositionAtWork>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PRIMARY");

            entity.ToTable("position_at_work");

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .HasColumnName("name")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.SalaryPerHour)
                .HasPrecision(3)
                .HasColumnName("salary_per_hour");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PRIMARY");

            entity.ToTable("restaurant");

            entity.HasIndex(e => e.RestaurantId, "worker_id");

            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.EmployeesCount).HasColumnName("employees_count");
            entity.Property(e => e.Mall)
                .HasMaxLength(100)
                .HasColumnName("mall");
            entity.Property(e => e.RestaurantCode)
                .HasMaxLength(10)
                .HasColumnName("restaurant_code");
            entity.Property(e => e.Town)
                .HasMaxLength(25)
                .HasColumnName("town");
        });

        modelBuilder.Entity<WorkShift>(entity =>
        {
            entity.HasKey(e => e.WorkShiftId).HasName("PRIMARY");

            entity.ToTable("work_shift");

            entity.HasIndex(e => e.WorkerId, "fk_work_shift_worker_idx");

            entity.Property(e => e.WorkShiftId).HasColumnName("work_shift_id");
            entity.Property(e => e.DescriptionManualEntry)
                .HasMaxLength(45)
                .HasColumnName("description_manual_entry");
            entity.Property(e => e.EndShift)
                .HasColumnType("datetime")
                .HasColumnName("end_shift");
            entity.Property(e => e.StartShift)
                .HasColumnType("datetime")
                .HasColumnName("start_shift");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.Worker).WithMany(p => p.WorkShifts)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_work_shift_worker");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.WorkerId).HasName("PRIMARY");

            entity.ToTable("worker");

            entity.HasIndex(e => e.PositionId, "fk_worker_position_at_work1_idx");

            entity.HasIndex(e => e.RestaurantId, "fk_worker_restaurant1_idx");

            entity.HasIndex(e => e.WorkerId, "idx_worker_id");

            entity.Property(e => e.WorkerId).HasColumnName("worker_id");
            entity.Property(e => e.DismissalReason)
                .HasMaxLength(100)
                .HasColumnName("dismissal_reason")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(30)
                .HasColumnName("patronymic")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname")
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            entity.HasOne(d => d.Position).WithMany(p => p.Workers)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_worker_position_at_work1");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Workers)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_worker_restaurant1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
