using Workflow.Models;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Services.Workflows
{
    public class CollectApplicantInfo : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("📌 Step 1: Collecting Applicant Information...");
            return ExecutionResult.Next();
        }
    }

    // Step 2: Check Loan Eligibility
    public class CheckEligibility : StepBody
    {
        public bool IsEligible { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("📌 Step 2: Checking Eligibility...");
            this.IsEligible = new Random().Next(0, 2) == 1; // Random eligibility
            return ExecutionResult.Next();
        }
    }

    // Step 3: Manager Approval
    public class ManagerApproval : StepBody
    {
        public bool IsApproved { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("📌 Step 3: Manager Approval in Process...");
            this.IsApproved = new Random().Next(0, 2) == 1; // Random approval
            return ExecutionResult.Next();
        }
    }

    // Step 4: Final Decision
    public class FinalDecision : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("📌 Step 4: Making Final Decision...");
            return ExecutionResult.Next();
        }
    }

}
