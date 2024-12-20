﻿using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Models.Buyer;
using Live_Bidding_System_App.Models.Chats;
using Live_Bidding_System_App.Models.Seller;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AuctionItemCategory> AuctionItemCategoryTbl { get; set; }
        public DbSet<AuctionItem> AuctionItemsTbl { get; set; }
        public DbSet<Auction> AuctionsTbl { get; set; }
        public DbSet<Bid> BidsTbl { get; set; }
        public DbSet<PaymentHistory> PaymentHistoriesTbl { get; set; }
        public DbSet<ChatRoom> ChatRoomsTbl { get; set; }
        public DbSet<ChatMessage> ChatMessagesTbl { get; set; }

    }
}
