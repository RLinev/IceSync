using IceCreamCompanySync.HttpHandler;
using IceCreamCompanySync.HttpHandler.Models;
using IceCreamCompanySync.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IceCreamCompanySync.HttpHandlers
{
    public class RequestHandler : IRequestHandler
    {
        private readonly string  jwToken;
        private readonly string _baseUrl;
        private readonly ILogger<RequestHandler> _logger;


        public RequestHandler(IConfiguration config, ITokenService authHandler, ILogger<RequestHandler> logger)
        {
            _baseUrl = $"{config["UniLoaderAPIUrl"]}/workflows";
            jwToken = authHandler.FetchToken();
            _logger = logger;
        }
      public async Task<List<WorkflowModel>> GetWorkflows()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    _logger.LogInformation($"Calling API to fetch workflows.");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
                    var result = await client.GetAsync(_baseUrl);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var workflowsData = await result.Content.ReadAsStringAsync();
                        var workflows = JsonConvert.DeserializeObject<List<WorkflowModel>>(workflowsData);
                        _logger.LogInformation($"Sucessfull fetch to API. {workflows.Count} workflows returned.");
                        return workflows;
                    }
                    else
                    {
                        _logger.LogInformation($"Unsucessfull feth of workflows. Status code: {result.StatusCode}.");
                    }

                }
                catch (Exception ex )
                {
                    _logger.LogInformation($"Exception thrown durring fetch of workflows from API. Exception message:{ex.Message}");
                    throw;
                }
            }
            return new List<WorkflowModel>();

        }

        public async Task<string> RunWorkflow(int workFlowId)
        {
            try
            {
                _logger.LogInformation($"Calling API to run workflow with ID {workFlowId}.");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
                    var result = await client.PostAsync($"{_baseUrl}/{workFlowId}/run", null);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return "Sucessfull";
                    }

                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception occur while calling API to run workflow. Exception message:{ex.Message}");
                throw;
            }
            
        }
    }
}
