using System.Collections.Generic;

namespace Phanerozoic.Core.Services.Interfaces
{
    /// <summary>
    /// 電子郵件發送服務
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toList"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        void Send(string from, List<string> toList, string subject, string body);
    }
}