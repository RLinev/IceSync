using IceCreamCompanySync.Database.Intefaces;
using IceCreamCompanySync.HttpHandler;
using IceCreamCompanySync.HttpHandler.Models;
using System.Data.SqlClient;

namespace IceCreamCompanySync
{
    public class IceCreamBackgrounWorker : BackgroundService
    {
        private readonly ILogger<IceCreamBackgrounWorker> _logger;
        private readonly IDatabaseManager _manager;
        private readonly IRequestHandler _requestHandler;
        private readonly string _spName;
        public IceCreamBackgrounWorker(ILogger<IceCreamBackgrounWorker> logger, IDatabaseManager manager, IConfiguration config, IRequestHandler requestHandler)
        {
            _logger = logger;
            _manager= manager;
            _spName= config.GetValue<string>("UpdateWorkflowsSP");
            _requestHandler = requestHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workflows = await _requestHandler.GetWorkflows();
                    _logger.LogInformation("IceCreamBackgrounWorker running at: {time}", DateTimeOffset.Now);
                    var parameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@Workflows", workflows.ToDataWorkflowTable())
                    };
                    _manager.ExecuteNotQuery(_spName, parameters);
                    //run every 30 minutes
                    await Task.Delay(1800000, stoppingToken);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}