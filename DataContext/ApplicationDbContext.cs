using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Models.Buyer;
using Live_Bidding_System_App.Models.Chats;
using Live_Bidding_System_App.Models.Seller;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AuctionItem> AuctionItemsTbl { get; set; }
        public DbSet<Auction> AuctionsTbl { get; set; }
        public DbSet<Bid> BidsTbl { get; set; }
        public DbSet<PaymentHistory> PaymentHistoriesTbl { get; set; }
        public DbSet<ChatRoom> ChatRoomsTbl { get; set; }
        public DbSet<ChatMessage> ChatMessagesTbl { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(c => c.Id);
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatRoom>()
                 .HasMany(c => c.Participants)
                 .WithMany(u => u.ChatRooms);

            // Additional configurations...
        }

    }
}
