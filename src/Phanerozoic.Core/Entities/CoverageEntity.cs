namespace Phanerozoic.Core.Entities
{
    public class CoverageEntity
    {
        public string Class { get; set; }
        public string Method { get; set; }
        public int Coverage { get; set; }

        public override string ToString()
        {
            return $"{Class}.{Method}:{Coverage}";
        }
    }
}