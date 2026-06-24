using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── Company & Users ───────────────────────────────
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users => Set<User>();

        // ── Properties (TPT) ──────────────────────────────
        public DbSet<Property> Properties => Set<Property>();
        public DbSet<ApartmentProperty> ApartmentProperties => Set<ApartmentProperty>();
        public DbSet<VillaProperty> VillaProperties => Set<VillaProperty>();
        public DbSet<OfficeProperty> OfficeProperties => Set<OfficeProperty>();
        public DbSet<WarehouseProperty> WarehouseProperties => Set<WarehouseProperty>();
        public DbSet<LandProperty> LandProperties => Set<LandProperty>();
        public DbSet<BuildingProperty> BuildingProperties => Set<BuildingProperty>();

        // ── Property Related ──────────────────────────────
        public DbSet<PropertyMedia> PropertyMedias => Set<PropertyMedia>();
        public DbSet<PropertyDocument> PropertyDocuments => Set<PropertyDocument>();

        // ── Owners & Clients ──────────────────────────────
        public DbSet<Owner> Owners => Set<Owner>();
        public DbSet<Client> Clients => Set<Client>();

        // ── Contracts & Payments ──────────────────────────
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Cheque> Cheques => Set<Cheque>();

        // ── Appointments ──────────────────────────────────
        public DbSet<Appointment> Appointments => Set<Appointment>();

        // ── Maintenance ───────────────────────────────────
        public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();
        public DbSet<MaintenanceMedia> MaintenanceMedias => Set<MaintenanceMedia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // ── Soft Delete Filters ───────────────────────
            // Property filter covers ALL leaf types (TPT) ✅
            // No need to add separate filters for each leaf type
            modelBuilder.Entity<Property>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Company>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Owner>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Client>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Contract>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Payment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Cheque>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Appointment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<Domain.Common.BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(ct);
        }
    }
}