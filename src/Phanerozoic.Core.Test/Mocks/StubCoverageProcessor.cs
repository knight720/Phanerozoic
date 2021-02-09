using System;
using Phanerozoic.Core.Services;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Test.Mocks
{
    internal class StubCoverageProcessor : CoverageProcessor
    {
        public INotifyer StubSlackNotifyer { get; set; }
        public INotifyer StubEmailNotifyer { get; set; }
        public bool StubIsSendSlack { get; set; }

        public StubCoverageProcessor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override INotifyer GetSlackNotifyer()
        {
            return StubSlackNotifyer;
        }

        protected override INotifyer GetEmailNotifyer()
        {
            return StubEmailNotifyer;
        }

        protected override bool IsSendSlack()
        {
            return StubIsSendSlack;
        }
    }
}