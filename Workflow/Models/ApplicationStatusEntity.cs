namespace Workflow.Models
{
    public class ApplicationStatusEntity
    {
        public int Id { get; set; }  // Matches the Enum value
        public string Name { get; set; }  // Status name (Pending, Approved, etc.)
        public string Description { get; set; }  // Optional description
    }

}
