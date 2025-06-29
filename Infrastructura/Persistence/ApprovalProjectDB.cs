using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class ApprovalProjectDB : DbContext
    {
        public ApprovalProjectDB(DbContextOptions<ApprovalProjectDB> options): base(options) { }

        public ApprovalProjectDB() : base() { }

        public DbSet<ApprovalRule> ApprovalRule { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ApproverRole> ApproverRole { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public DbSet<ProjectProposal> ProjectProposal { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalStep { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relationships

            modelBuilder.Entity<ApprovalRule>()
                .HasOne(s => s.Areas)
                .WithMany(g => g.ApprovalRules)
                .HasForeignKey(s => s.Area)
                .IsRequired(false);

            modelBuilder.Entity<ApprovalRule>()
                .HasOne(s => s.ProjectType)
                .WithMany(g => g.ApprovalRules)
                .HasForeignKey(s => s.Type)
                .IsRequired(false);

            modelBuilder.Entity<ApprovalRule>()
                .HasOne(s => s.ApproverRole)
                .WithMany(g => g.ApprovalRules)
                .HasForeignKey(s => s.ApproverRoleId);

            modelBuilder.Entity<User>()
                .HasOne(s => s.ApproverRole)
                .WithMany(g => g.User)
                .HasForeignKey(s => s.Role);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(s => s.Areas)
                .WithMany(g => g.ProjectProposals)
                .HasForeignKey(s => s.Area);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(s => s.ProjectType)
                .WithMany(g => g.ProjectProposals)
                .HasForeignKey(s => s.Type);

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(s => s.ApprovalStatus)
                .WithMany(g => g.ProjectProposals)
                .HasForeignKey(s => s.Status);
                

            modelBuilder.Entity<ProjectProposal>()
                .HasOne(s => s.User)
                .WithMany(g => g.ProjectProposals)
                .HasForeignKey(s => s.CreateBy);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(s => s.ProjectProposal)
                .WithMany(g => g.ProjectApprovalSteps)
                .HasForeignKey(s => s.ProjectProposalId)
                .OnDelete(DeleteBehavior.NoAction); //Evitamos eliminar el proyecto si hay pasos de aprobación asociados

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(s => s.User)
                .WithMany(g => g.ProjectApprovalSteps)
                .HasForeignKey(s => s.ApproverUserId)
                .IsRequired(false);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(s => s.ApproverRole)
                .WithMany(g => g.ProjectApprovalSteps)
                .HasForeignKey(s => s.ApproverRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProjectApprovalStep>()
                .HasOne(s => s.ApprovalStatus)
                .WithMany(g => g.ProjectApprovalSteps)
                .HasForeignKey(s => s.Status)
                .OnDelete(DeleteBehavior.NoAction);


            //Table configuration
            modelBuilder.Entity<ApprovalRule>().ToTable("ApprovalRule");
            modelBuilder.Entity<ApprovalRule>().HasKey(s => s.Id);
            modelBuilder.Entity<ApprovalRule>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApprovalRule>().Property(s => s.MinAmount).HasPrecision(18,2).IsRequired();
            modelBuilder.Entity<ApprovalRule>().Property(s => s.MaxAmount).HasPrecision(18,2).IsRequired();
            modelBuilder.Entity<ApprovalRule>().Property(s => s.StepOrder).IsRequired();
            modelBuilder.Entity<ApprovalRule>().Property(s => s.ApproverRoleId).IsRequired();


            modelBuilder.Entity<ApprovalStatus>().ToTable("ApprovalStatus");
            modelBuilder.Entity<ApprovalStatus>().HasKey(s => s.Id);
            modelBuilder.Entity<ApprovalStatus>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApprovalStatus>().Property(s => s.Name).HasColumnType("varchar(25)").IsRequired();

            modelBuilder.Entity<ApproverRole>().ToTable("ApproverRole");
            modelBuilder.Entity<ApproverRole>().HasKey(s => s.Id);
            modelBuilder.Entity<ApprovalRule>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApproverRole>().Property(s => s.Name).HasColumnType("varchar(25)").IsRequired();

            modelBuilder.Entity<Area>().ToTable("Area");
            modelBuilder.Entity<Area>().HasKey(s => s.Id);
            modelBuilder.Entity<Area>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Area>().Property(s => s.Name).HasColumnType("varchar(25)").IsRequired();

            modelBuilder.Entity<ProjectApprovalStep>().ToTable("ProjectApprovalStep");
            modelBuilder.Entity<ProjectApprovalStep>().HasKey(s => s.Id);
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.ProjectProposalId).IsRequired();
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.ApproverRoleId).IsRequired();
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.Status).IsRequired();
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.StepOrder).IsRequired();
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.DecisionDate).HasColumnType("datetime").IsRequired(false);
            modelBuilder.Entity<ProjectApprovalStep>().Property(s => s.Observations).HasColumnType("varchar(max)").IsRequired(false);

            modelBuilder.Entity<ProjectProposal>().ToTable("ProjectProposal");
            modelBuilder.Entity<ProjectProposal>().HasKey(s => s.Id);
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Title).HasColumnType("varchar(225)").IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Description).HasColumnType("varchar(max)").IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Area).IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Type).IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.EstimatedAmount).HasPrecision(18,2).IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.EstimatedDuration).IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.Status).IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.CreateAt).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<ProjectProposal>().Property(s => s.CreateBy).IsRequired(); 

            modelBuilder.Entity<ProjectType>().ToTable("ProjectType");
            modelBuilder.Entity<ProjectType>().HasKey(s => s.Id);
            modelBuilder.Entity<ProjectType>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProjectType>().Property(s => s.Name).HasColumnType("varchar(25)").IsRequired();

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(s => s.Id );
            modelBuilder.Entity<User>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(s => s.Name).HasColumnType("varchar(25)").IsRequired();
            modelBuilder.Entity<User>().Property(s => s.Email).HasColumnType("varchar(100)").IsRequired();
            modelBuilder.Entity<User>().Property(s => s.Role).IsRequired();

            //Seed data
            modelBuilder.Entity<ApproverRole>().HasData(
                new ApproverRole { Id = 1, Name = "Líder de Área" },
                new ApproverRole { Id = 2, Name = "Gerente" },
                new ApproverRole { Id = 3, Name = "Director" },
                new ApproverRole { Id = 4, Name = "Comité Técnico" }
            );

            modelBuilder.Entity<ApprovalStatus>().HasData(
                new ApprovalStatus { Id = 1, Name = "Pending" },
                new ApprovalStatus { Id = 2, Name = "Approved" },
                new ApprovalStatus { Id = 3, Name = "Rejected" },
                new ApprovalStatus { Id = 4, Name = "Observed" }
            );

            modelBuilder.Entity<ProjectType>().HasData(
                new ProjectType { Id = 1, Name = "Mejora de Procesos" },
                new ProjectType { Id = 2, Name = "Innovación y Desarrollo" },
                new ProjectType { Id = 3, Name = "Infraestructura" },
                new ProjectType { Id = 4, Name = "Capacitación Interna" }
            );

            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Finanzas" },
                new Area { Id = 2, Name = "Tecnología" },
                new Area { Id = 3, Name = "Recursos Humanos" },
                new Area { Id = 4, Name = "Operaciones" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "José Ferreyra", Email = "jferreyra@unaj.com", Role = 2 },
                new User { Id = 2, Name = "Ana Lucero", Email = "alucero@unaj.com", Role = 1 },
                new User { Id = 3, Name = "Gonzalo Molinas", Email = "gmolinas@unaj.com", Role = 2 },
                new User { Id = 4, Name = "Lucas Olivera", Email = "lolivera@unaj.com", Role = 3 },
                new User { Id = 5, Name = "Danilo Fagundez", Email = "dfagundez@unaj.com", Role = 4 },
                new User { Id = 6, Name = "Gabriel Galli", Email = "ggalli@unaj.com", Role = 4 }
            );

            modelBuilder.Entity<ApprovalRule>().HasData(
                new ApprovalRule { Id = 1, MinAmount = 0, MaxAmount = 100000, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 2, MinAmount = 5000, MaxAmount = 20000, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 3, MinAmount = 0, MaxAmount = 20000, Area = 2, Type= 2, StepOrder = 1, ApproverRoleId = 2 },
                new ApprovalRule { Id = 4, MinAmount = 20000, MaxAmount = 0, StepOrder = 3, ApproverRoleId = 3 },
                new ApprovalRule { Id = 5, MinAmount = 5000, MaxAmount = 0,Area = 1, Type = 1 ,StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 6, MinAmount = 0, MaxAmount = 10000, Type = 2, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 7, MinAmount = 0, MaxAmount = 10000, Area = 2, Type = 1, StepOrder = 1, ApproverRoleId = 4 },
                new ApprovalRule { Id = 8, MinAmount = 10000, MaxAmount = 30000, Area = 2, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 9, MinAmount = 30000, MaxAmount = 0, Area = 3, StepOrder = 2, ApproverRoleId = 3 },
                new ApprovalRule { Id = 10, MinAmount = 0, MaxAmount = 50000, Type = 4, StepOrder = 1, ApproverRoleId = 4 }
            );





        }


    }
}
