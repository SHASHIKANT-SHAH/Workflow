namespace Workflow.Models
{
    public class ApplicationHistory
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int PreviousStatusId { get; set; }  // Foreign Key to ApplicationStatusEntity
        public ApplicationStatusEntity PreviousStatus { get; set; }  // Navigation Property
        public int NewStatusId { get; set; }  // Foreign Key to ApplicationStatusEntity
        public ApplicationStatusEntity NewStatus { get; set; }  // Navigation Property
        public string ChangedBy { get; set; }
        public string Remarks { get; set; }
        public DateTime ChangedAt { get; set; }
    }


}
