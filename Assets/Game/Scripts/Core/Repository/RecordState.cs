namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 表记录状态
    /// </summary>
    public enum RecordState : byte
    {
        /// <summary>
        /// 纯净
        /// </summary>
        Clean,
        
        /// <summary>
        /// 新建
        /// </summary>
        New,
        
        /// <summary>
        /// 污染,即被修改过
        /// </summary>
        Dirty,
        
        /// <summary>
        /// 被标记删除
        /// </summary>
        Delete,
    }
}