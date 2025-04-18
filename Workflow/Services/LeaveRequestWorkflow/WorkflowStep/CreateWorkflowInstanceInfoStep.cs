using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Workflow.Data;
using Workflow.Services.LeaveRequestWorkflow.model;
using Workflow.Models;

public class CreateWorkflowInstanceInfoStep : StepBodyAsync
{
    private readonly ApplicationDbContext _context;

    public CreateWorkflowInstanceInfoStep(ApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var data = context.Workflow.Data as LeaveRequestData;

        var instance = new WorkflowInstanceInfo
        {
            Id = data.WorkflowInstanceId,
            StartDate = DateTime.UtcNow,
            Status = "InProgress"
        };

        _context.WorkflowInstanceInfos.Add(instance);
        await _context.SaveChangesAsync();

        // Now create and save the LeaveRequest, linking it to the WorkflowInstanceInfo
        var leaveRequest = new LeaveRequest
        {
            EmployeeName = data.EmployeeName,
            WorkflowInstanceInfoId = instance.Id, // Link to the new WorkflowInstanceInfo
            ManagerDecisionId = 1, // Assuming 1 = Pending
            HrDecisionId = 1 // Assuming 1 = Pending
        };

        _context.LeaveRequests.Add(leaveRequest);
        await _context.SaveChangesAsync();

        return ExecutionResult.Next();
    }
}
