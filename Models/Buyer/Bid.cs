﻿using Live_Bidding_System_App.Models.Seller;
using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Buyer
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public decimal Amount { get; set; }  // Bid amount
        public DateTime BidTime { get; set; }  // Time bid was placed

        // Foreign Keys
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency control
    }

}