using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Core.Repository.External
{
    /// <summary>
    /// 数据表接口
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// 获取表结构
        /// </summary>
        Type Scheme { get; }

        /// <summary>
        /// 获取有效的记录集
        /// </summary>
        List<RecordBase> Assembly
        {
            get;
        }

        /// <summary>
        /// 获取标记删除的记录集
        /// </summary>
        List<RecordBase> DeletedAssembly
        {
            get;
        }

        /// <summary>
        /// 获取被污染的记录集
        /// </summary>
        List<RecordBase> DirtyAssembly
        {
            get;
        }


        /// <summary>
        /// 获取新增的记录集
        /// </summary>
        List<RecordBase> NewAssembly
        {
            get;
        }

        /// <summary>
        /// 获取未变化的记录集
        /// </summary>
        List<RecordBase> CleanAssembly
        {
            get;
        }

        /// <summary>
        /// 获取所有记录集
        /// </summary>
        List<RecordBase> TotalAssembly
        {
            get;
        }

        /// <summary>
        /// 清空记录，暴力清空，不触发任何事件
        /// </summary>
        void ViolenceClear();

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        RecordBase Create();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        void Delete(RecordBase record);

        /// <summary>
        /// 删除
        /// </summary>
        void Delete();

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="record">表记录</param>
        void ResetData(RecordBase record);

        /// <summary>
        /// 重置数据
        /// </summary>
        void ResetData();

        /// <summary>
        /// 重置状态
        /// </summary>
        /// <param name="record">表记录</param>
        void ResetState(RecordBase record);

        /// <summary>
        /// 重置整张表的状态
        /// </summary>
        void ResetState();

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        void Attach(RecordBase record);

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="records">表记录集</param>
        void Attach(IEnumerable<RecordBase> records);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        bool IsExist(RecordBase record, bool isIncludeDeleted = false);

        /// <summary>
        /// 统计符合条件的记录数
        /// </summary>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        int Count(Func<RecordBase, bool> where, bool isIncludeDeleted = false);

        /// <summary>
        /// 筛选符合条件的所有记录
        /// </summary>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<RecordBase> Find(Func<RecordBase, bool> where, bool isIncludeDeleted = false);
        
        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        RecordBase FirstOrDefault(bool isIncludeDeleted = false);

        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="match"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        RecordBase FirstOrDefault(Func<RecordBase, bool> match, bool isIncludeDeleted = false);

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        UniTask<object> Package(RecordBase record, params object[] extraObjects);

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <param name="arg1">参数</param>
        /// <returns></returns>
        UniTask<TRecord> Parse<TRecord, TJson, TArg>(TJson jsonModels, TArg arg1)
            where TRecord : RecordBase;
        
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <returns></returns>
        UniTask<TRecord> Parse<TRecord, TJson>(TJson jsonModels)
            where TRecord : RecordBase;
        
        /// <summary>
        /// 获取主键值
        /// </summary>
        /// <param name="record">表记录</param>
        /// <returns></returns>
        object GetPrimaryKey(RecordBase record);
        
        /// <summary>
        /// 释放记录
        /// </summary>
        /// <param name="record">表记录</param>
        void Dispose(RecordBase record);
        
    }
}