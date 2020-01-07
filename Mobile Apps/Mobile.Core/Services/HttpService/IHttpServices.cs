using System.Threading.Tasks;

namespace Mobile.Core.Interface
{
    public interface IHttpServices<T>
    {
        Task<string> DeleteAsync(string requestString, T body, string token);
        Task<string> GetAsync(string requestString, string token = null);
        Task<string> PostAsync(string requestString, T body, string token = null);
        Task<string> PutAsync(string requestString, T body, string token);
    }
}