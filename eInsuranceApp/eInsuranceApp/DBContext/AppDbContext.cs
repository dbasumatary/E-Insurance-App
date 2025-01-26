using eInsuranceApp.Entities.Admin;
using eInsuranceApp.Entities.AgentDTO;
using eInsuranceApp.Entities.CustomerDTO;
using eInsuranceApp.Entities.EmployeeDTO;
using eInsuranceApp.Entities.Login;
using eInsuranceApp.Entities.Payment;
using eInsuranceApp.Entities.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace eInsuranceApp.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<AdminEntity> Admin { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<AgentEntity> Agents { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<AgentEntity> CustomerAgent { get; set; }

        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<PremiumCalculationDTO> Premiums { get; set; }
        public DbSet<CommissionEntity> Commission { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin
            modelBuilder.Entity<AdminEntity>()
                .ToTable("Admins")
                .HasKey(a => a.AdminID);
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.Password)
                .IsRequired();
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.FullName)
                .IsRequired();
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.Role)
                .HasDefaultValue(UserRole.Admin);
            modelBuilder.Entity<AdminEntity>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            //var adminId = Guid.NewGuid();
            modelBuilder.Entity<AdminEntity>().HasData(new AdminEntity
            {
                AdminID = 1,
                Username = "mainadmin",
                Email = "admin@gmail.com",
                Password = "abc123@",
                FullName = "Main Admin",
                //CreatedAt = DateTime.UtcNow
                CreatedAt = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            });

            // Employee
            modelBuilder.Entity<EmployeeEntity>()
                .ToTable("Employees")
                .HasKey(e => e.EmployeeID);
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.Password)
                .IsRequired(); 
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.FullName)
                .IsRequired();
            modelBuilder.Entity<EmployeeEntity>()
                .Property(a => a.Role)
                .HasDefaultValue(UserRole.Employee);
            modelBuilder.Entity<EmployeeEntity>()
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Agent
            modelBuilder.Entity<AgentEntity>()
                .ToTable("Agents")
                .HasKey(a => a.AgentID);
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.Password)
                .IsRequired();
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.FullName)
                .IsRequired();
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.Role)
                .HasDefaultValue(UserRole.Agent);
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<AgentEntity>()
                .Property(a => a.CommissionRate)
                .HasColumnType("DECIMAL(10, 2)");



            // Customer
            modelBuilder.Entity<CustomerEntity>()
                .ToTable("Customers")
                .HasKey(c => c.CustomerID);
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.FullName)
                .IsRequired();
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.Username)
                .IsRequired();
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.Password)
                .IsRequired();
            modelBuilder.Entity<CustomerEntity>()
                .Property(a => a.Role)
                .HasDefaultValue(UserRole.Customer);
            modelBuilder.Entity<CustomerEntity>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");



            modelBuilder.Entity<AgentEntity>()
                        .HasMany(a => a.Customers)
                        .WithOne(c => c.Agent)
                        .HasForeignKey(c => c.AgentID)
                        .OnDelete(DeleteBehavior.Restrict);

            //Plans
            modelBuilder.Entity<InsurancePlan>()
                        .HasKey(i => i.PlanID);
            modelBuilder.Entity<Scheme>()
                        .HasKey(s => s.SchemeID);
            modelBuilder.Entity<Policy>()
                        .HasKey(p => p.PolicyID);

            modelBuilder.Entity<Policy>(entity =>
            {
                
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Status).HasDefaultValue("Pending");
            });

            modelBuilder.Entity<Scheme>(entity =>
            {
                entity.ToTable("Schemes").HasKey(a => a.SchemeID);
                entity.Property(e => e.SchemeFactor)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.PlanID).IsRequired();
                entity.Property(e => e.SchemeName).IsRequired();
                entity.Property(e => e.SchemeDetails).IsRequired();

            });

            modelBuilder.Entity<Scheme>()
                    .HasOne(s => s.Plan)
                    .WithMany(p => p.Schemes)
                    .HasForeignKey(s => s.PlanID);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.CustomerID);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Scheme)
                .WithMany(s => s.Policies)
                .HasForeignKey(p => p.SchemeID);

            //Payment
            modelBuilder.Entity<PaymentEntity>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.Status).HasDefaultValue("Pending");
                entity.Property(e => e.PaymentType).IsRequired();
            });


            modelBuilder.Entity<PaymentEntity>()
                        .HasOne(p => p.Customer)
                        .WithMany(c => c.Payments)
                        .HasForeignKey(p => p.CustomerID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentEntity>()
                        .HasOne(p => p.Policy)
                        .WithMany(policy => policy.Payments)
                        .HasForeignKey(p => p.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentEntity>()
                        .HasOne(p => p.Premium)
                        .WithMany(c => c.Payments) 
                        .HasForeignKey(p => p.PremiumID)
                        .OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<PremiumCalculationDTO>(entity =>
            {
                entity.ToTable("Premiums").HasKey(a => a.PremiumID);
                entity.Property(e => e.CustomerID).IsRequired();
                entity.Property(e => e.PolicyID).IsRequired();
                entity.Property(e => e.SchemeID).IsRequired();
                entity.Property(e => e.CustomerID).IsRequired();
                entity.Property(e => e.BaseRate)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.CalculatedPremium)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            modelBuilder.Entity<PremiumCalculationDTO>()
                        .HasOne(p => p.Scheme)
                        .WithMany(s => s.Premiums)
                        .HasForeignKey(p => p.SchemeID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PremiumCalculationDTO>()
                        .HasOne(p => p.Customer)
                        .WithMany(a => a.PremiumCalculations)  
                        .HasForeignKey(p => p.CustomerID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                        .HasMany(p => p.PremiumCalculations)
                        .WithOne(pc => pc.Policy)
                        .HasForeignKey(pc => pc.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);

            //Commission

            modelBuilder.Entity<CommissionEntity>(entity =>
            {
                entity.ToTable("Commission").HasKey(a => a.CommissionID);
                entity.Property(e => e.CommissionAmount)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<CommissionEntity>()
                        .HasOne(c => c.Agent)
                        .WithMany(a => a.Commissions)
                        .HasForeignKey(c => c.AgentID);

            modelBuilder.Entity<CommissionEntity>()
                        .HasOne(c => c.Premium)
                        .WithMany()
                        .HasForeignKey(c => c.PremiumID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
