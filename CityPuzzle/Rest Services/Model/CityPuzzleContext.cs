//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;



//namespace CityPuzzle.Rest_Services.Model
//{
//    public partial class CityPuzzleContext : DbContext
//    {
//        public CityPuzzleContext()
//        {
//        }

//        public CityPuzzleContext(DbContextOptions<CityPuzzleContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Participant> Participants { get; set; }
//        public virtual DbSet<Puzzle> Puzzles { get; set; }
//        public virtual DbSet<Room> Rooms { get; set; }
//        public virtual DbSet<RoomTask> RoomTasks { get; set; }
//        public virtual DbSet<Task> Tasks { get; set; }
//        public virtual DbSet<User> Users { get; set; }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

//            modelBuilder.Entity<Participant>(entity =>
//            {
//                entity.HasKey(e => new { e.RoomId, e.UserId });

//                entity.Property(e => e.RoomId).HasColumnName("RoomID");

//                entity.Property(e => e.UserId).HasColumnName("UserID");
//            });

//            modelBuilder.Entity<Room>(entity =>
//            {
//                entity.Property(e => e.RoomPin).IsUnicode(false);
//            });

//            modelBuilder.Entity<RoomTask>(entity =>
//            {
//                entity.HasKey(e => new { e.RoomId, e.PuzzleId })
//                    .HasName("PK__tmp_ms_x__0319429BB202ABED");

//                entity.Property(e => e.RoomId).HasColumnName("RoomID");

//                entity.Property(e => e.PuzzleId).HasColumnName("PuzzleID");
//            });

//            modelBuilder.Entity<Task>(entity =>
//            {
//                entity.HasKey(e => new { e.UserId, e.PuzzleId })
//                    .HasName("PK__Tasks__2617B72EFC15A233");

//                entity.Property(e => e.UserId).HasColumnName("UserID");

//                entity.Property(e => e.PuzzleId).HasColumnName("PuzzleID");
//            });

//            OnModelCreatingPartial(modelBuilder);
//        }

//        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//    }
//}
