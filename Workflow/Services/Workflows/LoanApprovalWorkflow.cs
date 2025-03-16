using Workflow.Models;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Services.Workflows
{
    public class LoanApprovalWorkflow : IWorkflow<LoanApplicationData>
    {
        public string Id => "LoanApprovalWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<LoanApplicationData> builder)
        {
            builder
                .StartWith<CollectApplicantInfo>()
                .Then<CheckEligibility>()
                    .Output(data => data.IsEligible, step => step.IsEligible)

                // First Condition: If Eligible
                .If(data => data.IsEligible).Do(then => then
                    .StartWith<ManagerApproval>()
                        .Output(data => data.IsManagerApproved, step => step.IsApproved)

                    // If Manager Approves
                    .If(data => data.IsManagerApproved).Do(approved => approved
                        .StartWith<FinalDecision>()
                        .Then(context => Console.WriteLine("✅ Loan Approved!")))

                    // If Manager Rejects
                    .If(data => !data.IsManagerApproved).Do(rejected => rejected
                        .Then(context => Console.WriteLine("❌ Loan Rejected by Manager!"))))

                // Second Condition: If Not Eligible
                .If(data => !data.IsEligible).Do(rejected => rejected
                    .Then(context => Console.WriteLine("❌ Loan Rejected: Not Eligible!")));
        }
    }

}
