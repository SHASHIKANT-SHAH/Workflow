namespace Workflow.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public decimal LoanAmount { get; set; }

        public int StatusId { get; set; }  // Foreign Key to ApplicationStatusEntity
        public ApplicationStatusEntity Status { get; set; }  // Navigation Property

        public List<ApplicationHistory> ApplicationHistories { get; set; }
    }

}
