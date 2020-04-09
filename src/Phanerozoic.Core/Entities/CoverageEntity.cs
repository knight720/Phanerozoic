using System;
using System.Collections.Generic;

namespace Phanerozoic.Core.Entities
{
    public class CoverageEntity
    {
        public string Repository { get; set; }
        public string Project { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int Coverage { get; set; }
        public int LastCoverage { get; private set; }
        public CoverageStatus Status { get; set; }
        public int RawIndex { get; set; }
        public IList<object> RawData { get; set; }
        public string Team { get; set; }
        public string UpdatedDate { get; set; }
        public int TargetCoverage { get; set; }
        public int LastTargetCoverage { get; private set; }
        public bool IsPass { get; private set; }

        public override string ToString()
        {
            return $"{Class}.{Method}:{Coverage}";
        }

        public void UpdateCoverage(CoverageEntity method)
        {
            if (this.Equals(method) == false)
            {
                throw new ApplicationException($"MethodEntity Not Match! {this.ToString()} vs {method.ToString()}");
            }

            if (this.Coverage == method.Coverage)
            {
                this.Status = CoverageStatus.Unchange;
            }
            else if (this.Coverage > method.Coverage)
            {
                this.Status = CoverageStatus.Down;
            }
            else
            {
                this.Status = CoverageStatus.Up;
            }

            this.LastCoverage = this.Coverage;
            this.Coverage = method.Coverage;

            this.CheckTargetCoverage();
        }

        private void CheckTargetCoverage()
        {
            this.IsPass = this.Coverage >= this.TargetCoverage;

            if (this.Coverage > this.TargetCoverage)
            {
                this.LastTargetCoverage = this.TargetCoverage;
                this.TargetCoverage = this.Coverage;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is CoverageEntity)
            {
                var target = (CoverageEntity)obj;
                return (this.Repository == target.Repository &&
                    this.Project == target.Project &&
                    this.Class == target.Class &&
                    this.Method == target.Method);
            }
            return false;
        }
    }
}