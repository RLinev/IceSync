using Newtonsoft.Json;

namespace IceCreamCompanySync.HttpHandler.Models
{
    public class WorkflowModel
    {
        [JsonProperty("id")]
        public int WorkflowID { get; set; }
        [JsonProperty("name")]
        public string WorkflowName { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        public string MultiExecBehavior { get; set; }
    }
}
