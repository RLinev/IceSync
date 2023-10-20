using IceCreamCompanySync.HttpHandler.Models;

namespace IceCreamCompanySync.HttpHandler
{
    public interface IRequestHandler
    {
        Task<List<WorkflowModel>> GetWorkflows();
        Task<string> RunWorkflow(int workFlowId);
    }
}
