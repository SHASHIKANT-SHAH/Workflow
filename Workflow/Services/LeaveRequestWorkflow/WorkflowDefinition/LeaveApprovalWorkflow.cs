using Workflow.Services.LeaveRequestWorkflow.model;
using Workflow.Services.LeaveRequestWorkflow.WorkflowStep;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Services.LeaveRequestWorkflow.WorkflowDefinition
{
    public class LeaveApprovalWorkflow : IWorkflow<LeaveRequestData>
    {
        public string Id => "LeaveApprovalWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<LeaveRequestData> builder)
        {
            builder
                .StartWith<NotifyStep>()
                    .Input(step => step.Message, data => $"Leave request submitted by {data.EmployeeName}")

                .Then<PassThrough>() // a dummy step just to continue
                    .WaitFor("ManagerApproval", data => data.EmployeeName)
                        .Output(data => data.ManagerDecision, ev => ev.EventData.ToString())

                .Then<PrintDecisionStep>()
                        .Input(step => step.Title, data => "After Manager Approval")
                        .Input(step => step.Data, data => data)

                .Then<NotifyStep>()
                    .Input(step => step.Message, data => $"Manager Decision: {data.ManagerDecision}")

                .Then<PassThrough>()
                    .WaitFor("HrApproval", data => data.EmployeeName)
                        .Output(data => data.HrDecision, ev => ev.EventData.ToString())

                .Then<PrintDecisionStep>()
                        .Input(step => step.Title, data => "After HR Approval")
                        .Input(step => step.Data, data => data)

                .Then<NotifyStep>()
                    .Input(step => step.Message, data =>
                        $"Final Status: {(data.ManagerDecision == "Approved" && data.HrDecision == "Approved" ? "Approved" : "Rejected")}"
                    );
        }
    }

    // Optional PassThrough step if not already defined
    public class PassThrough : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
    public class PrintDecisionStep : StepBody
    {
        public string Title { get; set; }  // Custom label or purpose of the log
        public LeaveRequestData Data { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"--- {Title} ---");
            Console.WriteLine($"Employee Name   : {Data.EmployeeName}");
            Console.WriteLine($"Manager Decision: {Data.ManagerDecision}");
            Console.WriteLine($"HR Decision     : {Data.HrDecision}");
            Console.WriteLine("--------------------------");

            return ExecutionResult.Next();
        }
    }


    //public class PrintDecisionStep : StepBody
    //{
    //    public string ManagerDecision { get; set; }
    //    public string HrDecision { get; set; }

    //    public override ExecutionResult Run(IStepExecutionContext context)
    //    {
    //        if (!string.IsNullOrEmpty(ManagerDecision))
    //            Console.WriteLine($"[LOG] Manager Decision: {ManagerDecision}");

    //        if (!string.IsNullOrEmpty(HrDecision))
    //            Console.WriteLine($"[LOG] HR Decision: {HrDecision}");

    //        return ExecutionResult.Next();
    //    }
    //}


}
