namespace Workflow.Models
{
    public class LoanApplicationData
    {
        public string ApplicantName { get; set; }
        public decimal LoanAmount { get; set; }
        public bool IsEligible { get; set; }
        public bool IsManagerApproved { get; set; }
        public string FinalDecision { get; set; }
    }

}
