using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CityPuzzleWebSer.Models
{
    public partial class CityPuzzleContext : DbContext
    {
        public CityPuzzleContext()
        {
        }

        public CityPuzzleContext(DbContextOptions<CityPuzzleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Puzzle> Puzzles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
 //To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:citypuzzle.database.windows.net,1433;Initial Catalog=CityPuzzle;Persist Security Info=False;User ID=citypuzzle;Password=User123*+;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.HasKey("RoomPin");
                entity.HasKey("UserId");

                entity.Property(e => e.RoomPin).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomPin).IsUnicode(false);

                entity.Property(e => e.Tasks).IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.PuzzleId).HasColumnName("PuzzleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
