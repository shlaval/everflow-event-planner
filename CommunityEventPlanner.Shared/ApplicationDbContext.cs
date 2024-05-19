using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventPlanner.Shared
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }

        public DbSet<CommunityEvent> CommunityEvents { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SignUp> SignUps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CommunityEvent>(ce =>
            {
                ce.HasKey(e => e.Id);
                ce.HasMany(e => e.Tags)
                    .WithMany(t => t.CommunityEvents)
                    .UsingEntity("CommunityEventTag");
                ce.HasMany(e => e.SignUps)
                    .WithOne(su => su.CommunityEvent)
                    .HasForeignKey(su => su.CommunityEventId);
                ce.HasOne(ce => ce.Venue)
                    .WithMany(v => v.CommunityEvents);
            });

            builder.Entity<SignUp>(su =>
            {
                su.HasKey(e => e.Id);
                su.HasOne(e => e.CommunityEvent)
                    .WithMany(ce => ce.SignUps);
            });

            builder.Entity<Tag>(t =>
            {
                t.HasKey(e => e.Id);
                t.HasMany(e => e.CommunityEvents)
                    .WithMany(ce => ce.Tags)
                    .UsingEntity("CommunityEventTags");
            });

            builder.Entity<Venue>(v =>
            {
                v.HasKey(e => e.Id);
                v.HasMany(v => v.CommunityEvents)
                    .WithOne(ce => ce.Venue)
                    .HasForeignKey(ce => ce.VenueId);
            });
        }
    }
}
