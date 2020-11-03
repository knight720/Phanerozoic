using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Services.Interface;

namespace Phanerozoic.Core.Services.Notifications
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Phanerozoic.Core.Services.INotifyer" />
    public class EmailNotifyer : INotifyer
    {
        private readonly IEmailService _emailService;
        private readonly string _from;
        private readonly List<string> _toList;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotifyer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public EmailNotifyer(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            this._emailService = serviceProvider.GetService<IEmailService>();

            this._from = configuration["Notification:From"];
            var to = configuration["Notification:To"];
            if (string.IsNullOrWhiteSpace(to) == false)
            {
                this._toList = to.Split(',').ToList();
            }
        }

        public void Notify(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            Console.WriteLine($"Email From: {this._from}");
            Console.WriteLine($"To: {string.Join(',', this._toList)}");

            var projectMethod = methodList.Where(i => i.Repository == coverageEntity.Repository && i.Project == coverageEntity.Project).ToList();
            var downMethod = projectMethod.Where(i => i.Status == CoverageStatus.Down).ToList();
            var updateMethodCount = projectMethod.Count(i => i.IsUpdate);

            var subject = $"Phanerozic - {downMethod.Count}/{updateMethodCount}/{projectMethod.Count} - {coverageEntity.Repository} - {coverageEntity.Project}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Repository: {coverageEntity.Repository}");
            stringBuilder.AppendLine($"Project: {coverageEntity.Project}");
            stringBuilder.AppendLine($"Project Method Count: {projectMethod.Count}");
            stringBuilder.AppendLine($"Update Method Count: {updateMethodCount}");
            stringBuilder.AppendLine($"Coverage Down Method Count: {downMethod.Count}");
            downMethod.ForEach(i => stringBuilder.AppendLine($"{i.Class}.{i.Method}: {i.TargetCoverage} → {i.Coverage}"));

            this._emailService.Send(this._from, this._toList, subject, stringBuilder.ToString());
        }
    }
}