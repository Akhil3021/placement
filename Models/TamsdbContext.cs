using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace placement.Models;

public partial class TamsdbContext : DbContext
{
    public TamsdbContext()
    {
    }

    public TamsdbContext(DbContextOptions<TamsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Query> Queries { get; set; }

    public virtual DbSet<QueryResponse> QueryResponses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskLog> TaskLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SWIKTSKU-02\\SQLEXPRESS;Initial Catalog=tamsdb;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Query>(entity =>
        {
            entity.HasKey(e => e.Qid);

            entity.ToTable("queries");

            entity.Property(e => e.Qid).HasColumnName("qid");
            entity.Property(e => e.Attachement)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("attachement");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.RaisedBy).HasColumnName("raised_by");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.RaisedByNavigation).WithMany(p => p.Queries)
                .HasForeignKey(d => d.RaisedBy)
                .HasConstraintName("FK_users_queries");

            entity.HasOne(d => d.Task).WithMany(p => p.Queries)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_tasks_queries");
        });

        modelBuilder.Entity<QueryResponse>(entity =>
        {
            entity.HasKey(e => e.Qrid);

            entity.ToTable("query_response");

            entity.Property(e => e.Qrid).HasColumnName("qrid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("message");
            entity.Property(e => e.QueryId).HasColumnName("query_id");
            entity.Property(e => e.RespondedBy).HasColumnName("responded_by");

            entity.HasOne(d => d.Query).WithMany(p => p.QueryResponses)
                .HasForeignKey(d => d.QueryId)
                .HasConstraintName("FK_queries_query_response");

            entity.HasOne(d => d.RespondedByNavigation).WithMany(p => p.QueryResponses)
                .HasForeignKey(d => d.RespondedBy)
                .HasConstraintName("FK_users_query_response");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Tid);

            entity.ToTable("tasks");

            entity.Property(e => e.Tid).HasColumnName("tid");
            entity.Property(e => e.AssignedBy).HasColumnName("assigned_by");
            entity.Property(e => e.AssignedTo).HasColumnName("assigned_to");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EstimatedHours).HasColumnName("estimated_hours");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TaskAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_users_tasks2");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TaskAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_users_tasks");
        });

        modelBuilder.Entity<TaskLog>(entity =>
        {
            entity.HasKey(e => e.Lid);

            entity.ToTable("task_logs");

            entity.Property(e => e.Lid).HasColumnName("lid");
            entity.Property(e => e.BreakTime)
                .HasColumnType("datetime")
                .HasColumnName("break_time");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskLogs)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_users_task_logs");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
