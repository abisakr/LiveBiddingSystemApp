namespace Live_Bidding_System_App.Repositories.Buyer.DTO
{
    public class ViewBidsDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
