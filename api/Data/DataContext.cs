using Microsoft.EntityFrameworkCore;
using api.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace api.Data
{
    public class DataContext : IdentityDbContext<AppUser,AppRole,int
    ,IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,
    IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
     
         public DbSet<UserLike> Likes {get;set;}
        public DbSet<Message> Messages {get;set;}
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(ur=>ur.User)
            .HasForeignKey(ur=>ur.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(ur=>ur.UserRoles)
            .WithOne(ur=>ur.Role)
            .HasForeignKey(ur=>ur.RoleId)
            .IsRequired();

            
            builder.Entity<UserLike>().HasKey(k=> new{k.SourceUserId,k.LikedUserId});
           
            builder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserLike>()
            .HasOne(s=>s.LikedUser)
            .WithMany(l=>l.LikedByUsers)
            .HasForeignKey(s=>s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u=>u.Recipient)
            .WithMany(c=>c.MessagesRecevied)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u=>u.Sender)
            .WithMany(c=>c.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}