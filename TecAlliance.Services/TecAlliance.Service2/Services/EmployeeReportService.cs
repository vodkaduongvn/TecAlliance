using Newtonsoft.Json;
using TecAlliance.Service2.Models;

namespace TecAlliance.Service2.Services
{
    public class EmployeeReportService : IHostedService, IDisposable
    {
        private readonly ILogger<EmployeeReportService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private Timer _timer;

        public EmployeeReportService(ILogger<EmployeeReportService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmployeeReportService is starting.");

            // Set up a timer to execute the reporting task periodically
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Adjust the interval as needed (e.g., 24 hours)

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Employee report generation started.");

            try
            {
                // Fetch data from Service 1 API (Employee Management API)
                using var client = _httpClientFactory.CreateClient();
                string apiUrl = _configuration["EmployeeApiUrl"]; // Get the API URL from configuration
                HttpResponseMessage response = client.GetAsync(apiUrl).Result; // Adjust this based on your API endpoints
                if (response.IsSuccessStatusCode)
                {
                    var jsonEmployees = response.Content.ReadAsStringAsync().Result; // Deserialize the response into Employee objects

                    // Generate employee reports here
                    var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonEmployees);

                    _logger.LogInformation("Employee data:");
                    foreach (var employee in employees)
                    {
                        var report = $"ID: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}, Hiring Date: {employee.HiringDate}, Salary: {employee.Salary}";
                        _logger.LogInformation(report);
                    }

                    Directory.CreateDirectory("Reports");
                    var fileName = Path.Combine("Reports", "Employees.txt");
                    File.WriteAllLines(
                        fileName
                        , employees.Select(employee => $"ID: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}, Hiring Date: {employee.HiringDate}, Salary: {employee.Salary}")
                        );

                    _logger.LogInformation($"Employee data written to {fileName}");
                }
                else
                {
                    _logger.LogError($"Failed to fetch data from Employee Management API. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during employee report generation: {ex}");
            }

            _logger.LogInformation("Employee report generation completed.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmployeeReportService is stopping.");

            // Stop the timer when the service is stopped
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
