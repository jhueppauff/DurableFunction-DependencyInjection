using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;


namespace FunctionApp2
{
    public class FunctionTrigger
    {
        private readonly ILogger _logger;

        public FunctionTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionTrigger>();
        }

		/// <summary>
		/// HTTP-triggered function that starts the <see cref="HelloCitiesUntyped"/> orchestration.
		/// </summary>
		/// <param name="req">The HTTP request that was used to trigger this function.</param>
		/// <param name="client">The Durable Functions client that is used to start and manage orchestration instances.</param>
		/// <param name="executionContext">The Azure Functions execution context, which is available to all function types.</param>
		/// <returns>Returns an HTTP response with more information about the started orchestration instance.</returns>
		[Function(nameof(StartHelloCitiesUntyped))]
		public static async Task<HttpResponseData> StartHelloCitiesUntyped(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
			[DurableClient] DurableTaskClient client,
			FunctionContext executionContext)
		{
			ILogger logger = executionContext.GetLogger(nameof(StartHelloCitiesUntyped));

			string instanceId = await client.ScheduleNewOrchestrationInstanceAsync("HelloCitiesUntyped");
			logger.LogInformation("Created new orchestration with instance ID = {instanceId}", instanceId);

			return client.CreateCheckStatusResponse(req, instanceId);
		}
	}
}
