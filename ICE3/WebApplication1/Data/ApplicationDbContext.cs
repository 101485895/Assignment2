using AspNetCoreGeneratedDocument;
using COMP2139_ICE.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication1.Areas.ProjectManagement.Models;
using WebApplication1.Models;

namespace WebApplication1.Data;

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure Identity Configurations and Table are created
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Identity");

            // Define One-to-Many Relationship: One Project has Many ProjectTasks
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seeding Projects with complete information including dates and status
            modelBuilder.Entity<Project>().HasData()
                new Project
                {
                    ProjectId = 1,
                    Name = "Website Redesign",
                    Description  = 
                        "Complete overhaul of company website including new UI/UX design and backend optimization",
                    StartDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 3, day: 1), DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 6, day: 30), DateTimeKind.Utc),
                    Status = "In Progress"
                }
                new Project
                {
                    ProjectId = 2,
                    Name = "Mobile App Developement",
                    Description = "Developement of a cross-platform movile application for customer engagement",
                    StartDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 2, day: 15), DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 8, day: 15), DateTimeKind.Utc),
                    Status = "Planning"
                },
                new Project
                {
                    ProjectId = 3,
                    Name = "Database Migration",
                    Description = "Migrate existing SQL Server database to PostgreSQL with minimal downtime",
                    StartDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 4, day: 1), DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(new DateTime(year: 2025,, month: 5, day: 15), DateTimeKind.Utc),
                    Status = "Not started"
                };

                //Seeding ProjectTasks with related tasks for each project
                modelBuilder.Entity<ProjectTask>().HasData(
                    //tasks for Website Redesign (ProjectId = 1)
                new ProjectTask
                {
                    ProjectTaskId = 1,
                    ProjectId = 1,
                    Title = "UI Design Mockups",
                    Description = "Create initial wireframes and high-fidility mockups for new website design"
                },
                new ProjectTask
                {
                    ProjectTaskId = 2,
                    ProjectId = 1,
                    Title = "Backend API Developement",
                    Description = "Develop RestFul APIs for the new website functionality"
                },
                new ProjectTask
                {
                    ProjectTaskId = 3,
                    ProjectId = 1,
                    Title = "FrontEnd Implimentation",
                    Description = "Implement responsive frontend using React and Tailwind CSS"
                },
                new ProjectTask
                {
                    ProjectTaskId = 4,
                    ProjectId = 2,
                    Title = "Requirements Gathering",
                    Description = "Meet with stakeholders to define app features and requirements"
                },
               new ProjectTask
                {
                    ProjectTaskId = 5,
                    ProjectId = 2,
                    Title = "App Architecture Design",
                    Description = "Design the technical architecture for the mobile application"
                },
                new ProjectTask
                {
                    ProjectTaskId = 6,
                    ProjectId = 2,
                    Title = "Prototype Development",
                    Description = "Create initial prototype for user testing and feedback"
                },

                // Tasks for Database Migration (ProjectId = 3)
                new ProjectTask
                {
                    ProjectTaskId = 7,
                    ProjectId = 3,
                    Title = "Schema Analysis",
                    Description = "Analyze current SQL Server schema and plan PostgreSQL conversion"
                },
                new ProjectTask
                {
                    ProjectTaskId = 8,
                    ProjectId = 3,
                    Title = "Data Mapping",
                    Description = "Create mapping documentation for data type conversions"
                },
                new ProjectTask
                {
                    ProjectTaskId = 9,
                    ProjectId = 3,
                    Title = "Migration Testing",
                    Description = "Test migration process in staging environment"
                }
            };
            
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserTokens<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRolesClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
        }   
    }   