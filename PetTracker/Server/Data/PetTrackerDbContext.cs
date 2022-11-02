using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetTracker.Server.Entities;

namespace PetTracker.Server.Data
{
    public class PetTrackerDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Pet> Pets { get; set; } = default!;
        public DbSet<GpsDevice> GpsDevices { get; set; } = default!;
        public DbSet<PetLocation> PetLocations { get; set; } = default!;
        public DbSet<AirInfoSensor> AirInfoSensors { get; set; } = default!;
        public DbSet<AirInfoData> AirInfoData { get; set; } = default!;
        public DbSet<FlameDetectionSensor> FlameDetectionSensors { get; set; } = default!;
        public DbSet<FlameDetectionData> FlameDetectionData { get; set; } = default!;
        public DbSet<PetAreaCenterLocation> PetAreaCenterLocations { get; set; } = default!;
        public DbSet<PetMovementInfo> PetMovement { get; set; } = default!;

        public PetTrackerDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}