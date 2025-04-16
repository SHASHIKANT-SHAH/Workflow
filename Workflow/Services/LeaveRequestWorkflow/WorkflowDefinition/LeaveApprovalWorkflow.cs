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

                .Then<NotifyStep>()
                    .Input(step => step.Message, data => $"Manager Decision: {data.ManagerDecision}")

                .Then<PassThrough>()
                    .WaitFor("HrApproval", data => data.EmployeeName)
                        .Output(data => data.HrDecision, ev => ev.EventData.ToString())

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
}
