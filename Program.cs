using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using br.com.waltercoan;
using Microsoft.Extensions.DependencyInjection;

namespace azfuncisolated
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<FuncDbContext>();
                })
                .Build();

            host.Run();
        }
    }
}