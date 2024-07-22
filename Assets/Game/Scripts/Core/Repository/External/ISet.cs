using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Scripts.Core.Repository.External
{
    /// <summary>
    /// 数据库接口
    /// </summary>
    public interface ISet
    {
        /// <summary>
        /// 获取库ID
        /// </summary>
        int SetIdentity { get; }

        /// <summary>
        /// 获取表结构集
        /// </summary>
        List<Type> Schemes { get; }

        /// <summary>
        /// 判断指定表是否存在
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        bool IsExist(Type scheme);

        /// <summary>
        /// 判断指定表是否存在
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        bool IsExist<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 分配表
        /// </summary>
        /// <param name="scheme">表结构</param>
        void Alloc(Type scheme);

        /// <summary>
        /// 分配表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void Alloc<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 释放表
        /// </summary>
        /// <param name="scheme">表结构</param>
        void Free(Type scheme);

        /// <summary>
        /// 释放表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void Free<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 清空所有表，暴力清空，不触发任何事件
        /// </summary>
        void ViolenceClear();

        /// <summary>
        /// 清空某张表，暴力清空，不触发任何事件
        /// </summary>
        /// <param name="scheme">表结构</param>
        void ViolenceClear(Type scheme);

        /// <summary>
        /// 清空某张表，暴力清空，不触发任何事件
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void ViolenceClear<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        ITable GetTable(Type scheme);

        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        ITable GetTable<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="record">表记录</param>
        /// <returns></returns>
        ITable GetTable(RecordBase record);
        

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        void Delete(RecordBase record);

        /// <summary>
        /// 删除某张表
        /// </summary>
        /// <param name="scheme">表结构</param>
        void Delete(Type scheme);

        /// <summary>
        /// 删除某张表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void Delete<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 删除整个库
        /// </summary>
        void Delete();

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="type"></param>
        RecordBase Create(Type type);

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        TTable Create<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 创建并附加
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        RecordBase CreateAndAttach(Type type);

        /// <summary>
        /// 创建并附加
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        TTable CreateAndAttach<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        void Attach(RecordBase record);

        /// <summary>
        /// 附加(为了性能,此处人为约束。集合项类型必须一致)
        /// </summary>
        /// <param name="records">表记录集</param>
        void Attach(ICollection<RecordBase> records);

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="record">表记录</param>
        void ResetData(RecordBase record);

        /// <summary>
        /// 重置某张表的数据
        /// </summary>
        /// <param name="scheme">表结构</param>
        void ResetData(Type scheme);

        /// <summary>
        /// 重置某张表的数据
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void ResetData<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 重置整个库的数据
        /// </summary>
        void ResetData();

        /// <summary>
        /// 重置状态
        /// </summary>
        /// <param name="record">表记录</param>
        void ResetState(RecordBase record);

        /// <summary>
        /// 重置某张表的状态
        /// </summary>
        /// <param name="scheme">表结构</param>
        void ResetState(Type scheme);

        /// <summary>
        /// 重置某张表的状态
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        void ResetState<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 重置整个库的状态
        /// </summary>
        void ResetState();

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        bool IsExist(RecordBase record, bool isIncludeDeleted = false);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        bool IsExist(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        bool IsExist<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase;

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        int Count(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false);

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        int Count<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase;

        /// <summary>
        /// 获取记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<RecordBase> GetAssembly(Type scheme, bool isIncludeDeleted = false);

        /// <summary>
        /// 获取记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<TTable> GetAssembly<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase;

        /// <summary>
        /// 获取未污染记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        List<RecordBase> GetCleanAssembly(Type scheme);

        /// <summary>
        /// 获取未污染记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        List<TTable> GetCleanAssembly<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 获取更新记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        List<RecordBase> GetDirtyAssembly(Type scheme);

        /// <summary>
        /// 获取更新记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        List<TTable> GetDirtyAssembly<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 获取新增记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        List<RecordBase> GetNewAssembly(Type scheme);

        /// <summary>
        /// 获取新增记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        List<TTable> GetNewAssembly<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 获取软删除记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        List<RecordBase> GetDeleteAssembly(Type scheme);

        /// <summary>
        /// 获取软删除记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        List<TTable> GetDeleteAssembly<TTable>() where TTable : RecordBase;

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        UniTask<object> Package(RecordBase record, params object[] extraObjects);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<RecordBase> Find(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false);

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<TTable> Find<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase;

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        List<TTable> Find<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase;
        
        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        RecordBase FirstOrDefault(Type scheme,bool isIncludeDeleted = false);

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        RecordBase FirstOrDefault(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false);

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        TTable FirstOrDefault<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false)
            where TTable : RecordBase;

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        TTable FirstOrDefault<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase;
        
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <typeparam name="TRecord">表类型</typeparam>
        /// <typeparam name="TJson">json数据类型</typeparam>
        /// <returns></returns>
        UniTask<TRecord> Parse<TRecord, TJson>(TJson jsonModels) where TRecord : RecordBase;

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <param name="arg1">参数</param>
        /// <typeparam name="TRecord">表类型</typeparam>
        /// <typeparam name="TJson">json数据类型</typeparam>
        /// <typeparam name="TArg">参数类型</typeparam>
        /// <returns></returns>
        UniTask<TRecord> Parse<TRecord, TJson,TArg>(TJson jsonModels,TArg arg1) where TRecord : RecordBase;
        
    }
}