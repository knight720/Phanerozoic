using System;
using System.Collections.Generic;

namespace Phanerozoic.Core.Entities
{
    public class CoverageEntity
    {
        public string Repository { get; set; }
        public string Project { get; set; }
        public string Namespace { get; set; }
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
        public int NewTargetCoverage { get; private set; }
        public bool IsUpdate { get; private set; }

        public bool IsPass
        {
            get
            {
                //// 目標涵蓋率小於0則不檢查
                return this.TargetCoverage < 0 || this.Coverage >= this.TargetCoverage;
            }
        }

        public override string ToString()
        {
            if (this.Namespace == "*")
            {
                return $"Project - {this.Project}: {this.Coverage}";
            }
            else if (this.Class == "*")
            {
                return $"Namespace - {this.Namespace}: {this.Coverage}";
            }
            else if (this.Method == "*")
            {
                return $"Class - {this.Namespace}.{this.Class}: {this.Coverage}";
            }
            return $"{Namespace}.{Class}.{Method}: {Coverage}";
        }

        public void UpdateCoverage(CoverageEntity method)
        {
            if (this.Equals(method) == false)
            {
                throw new ApplicationException($"MethodEntity Not Match! {this.ToString()} vs {method.ToString()}");
            }

            this.IsUpdate = true;

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

            this.UpdateTargetCoverage();
        }

        private void UpdateTargetCoverage()
        {
            this.NewTargetCoverage = this.TargetCoverage;

            if (this.Coverage > this.TargetCoverage)
            {
                this.NewTargetCoverage = this.Coverage;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is CoverageEntity)
            {
                var target = (CoverageEntity)obj;
                return (this.Repository == target.Repository &&
                    this.Project == target.Project &&
                    this.Namespace == target.Namespace &&
                    this.Class == target.Class &&
                    this.Method == target.Method);
            }
            return false;
        }
    }
}