using System;
using Phanerozoic.Core.Services;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Test.Services
{
    internal class StubCoverageProcessor : CoverageProcessor
    {
        public INotifyer StubSlackNotifyer { get; set; }
        public INotifyer StubEmailNotifyer { get; set; }

        public StubCoverageProcessor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override INotifyer GetSlackNotifyer()
        {
            return this.StubSlackNotifyer;
        }

        protected override INotifyer GetEmailNotifyer()
        {
            return this.StubEmailNotifyer;
        }
    }
}