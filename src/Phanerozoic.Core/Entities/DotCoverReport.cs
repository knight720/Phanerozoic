using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Phanerozoic.Core.Entities
{
    public partial class DotCoverReport
    {
        public string DotCoverVersion { get; set; }
        public Kind Kind { get; set; }
        public long CoveredStatements { get; set; }
        public long TotalStatements { get; set; }
        public long CoveragePercent { get; set; }
        public List<DotCoverReportChild> Children { get; set; }
    }

    public partial class DotCoverReportChild
    {
        public Kind Kind { get; set; }
        public string Name { get; set; }
        public long CoveredStatements { get; set; }
        public long TotalStatements { get; set; }
        public long CoveragePercent { get; set; }
        public List<DotCoverReportChild> Children { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Kind
    {
        Root,
        AnonymousMethod,
        Assembly,
        AutoProperty,
        Constructor,
        InternalCompiledMethod,
        Method,
        Namespace,
        OwnCoverage,
        Property,
        PropertyGetter,
        PropertySetter,
        Type,
        Event,
        EventAdder,
        EventRemover,
        LocalFunction,
        UnresolvedStateMachineMethod,
    };
}