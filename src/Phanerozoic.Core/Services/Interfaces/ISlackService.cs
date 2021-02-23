using System.Threading.Tasks;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ISlackService
    {
        Task SendAsync(string webHookUrl, string slackMessageJson);
    }
}