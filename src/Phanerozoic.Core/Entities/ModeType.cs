namespace Phanerozoic.Core.Entities
{
    public enum ModeType
    {
        /// <summary>
        /// 單一報告完整執行
        /// </summary>
        Full = 1,

        /// <summary>
        /// 解析報告後並收集
        /// </summary>
        Parse = 2,

        /// <summary>
        /// 更新並發出通知
        /// </summary>
        Update = 3,
    }
}