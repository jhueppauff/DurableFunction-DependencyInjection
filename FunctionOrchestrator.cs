using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;


namespace FunctionApp2
{
	internal class FunctionOrchestrator
	{
		private readonly ILogger _logger;
		private readonly IService _service;

		public FunctionOrchestrator(ILoggerFactory loggerFactory, IService service)
		{
			_logger = loggerFactory.CreateLogger<FunctionTrigger>();
			_service = service;
		}

		/// <summary>
		/// Orchestrator function that calls the <see cref="SayHelloUntyped"/> activity function several times consecutively.
		/// </summary>
		/// <param name="requestState">The serialized orchestration state that gets passed to the function.</param>
		/// <returns>Returns an opaque output string with instructions about what actions to persist into the orchestration history.</returns>
		[Function(nameof(HelloCitiesUntyped))]
		public async Task<string> HelloCitiesUntyped([OrchestrationTrigger] TaskOrchestrationContext context)
		{
			string result = "";
			result += await context.CallActivityAsync<string>(nameof(SayHelloUntyped), "Tokyo") + " ";
			result += await context.CallActivityAsync<string>(nameof(SayHelloUntyped), "London") + " ";
			result += await context.CallActivityAsync<string>(nameof(SayHelloUntyped), "Seattle");
			return result;
		}

		/// <summary>
		/// Simple activity function that returns the string "Hello, {input}!".
		/// </summary>
		/// <param name="cityName">The name of the city to greet.</param>
		/// <returns>Returns a greeting string to the orchestrator that called this activity.</returns>
		[Function(nameof(SayHelloUntyped))]
		public string SayHelloUntyped([ActivityTrigger] string cityName, FunctionContext executionContext)
		{
			ILogger logger = executionContext.GetLogger(nameof(SayHelloUntyped));
			logger.LogInformation("Saying hello to {name}", cityName);

			string hello = _service.Hello();

			return $"{hello}, {cityName}!";
		}
	}
}
