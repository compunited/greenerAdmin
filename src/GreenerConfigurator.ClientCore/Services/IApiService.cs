using System.Threading.Tasks;

namespace GreenerConfigurator.ClientCore.Services
{
    public interface IApiService
    {
        Task<string> SendGetRequestAsync(string apiUrl, object bodyContent = null);
        Task<string> SendPostRequestAsync(string apiUrl, object bodyContent);
        // Add other methods as needed, e.g. Put, Delete
    }
}
