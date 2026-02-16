#region Using

using System.Threading.Tasks;

#endregion

namespace WiseX.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}