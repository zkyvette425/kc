using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Repository.External;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 仓储
    /// </summary>
    internal static class Repository
    {
        private static Dictionary<int, ModelSet> _sets;
        
        /// <summary>
        /// 构建仓储
        /// </summary>
        static Repository()
        {
            _sets = new Dictionary<int, ModelSet>();
        }

        /// <summary>
        /// 查询数据库是否存在
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <returns></returns>
        public static bool IsExist(int setId)
        {
            return _sets.ContainsKey(setId);
        }
        
        /// <summary>
        /// 查询指定库中指定表是否存在
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <param name="scheme">数据表结构</param>
        /// <returns></returns>
        public static bool IsExist(int setId, Type scheme)
        {
            return IsExist(setId) && GetSet(setId).IsExist(scheme);
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <returns></returns>
        public static ISet GetSet(int setId)
        {
            if (!IsExist(setId))
            {
                Alloc(setId);
            }
            return _sets[setId];
        }

        /// <summary>
        /// 获取库
        /// </summary>
        /// <param name="record">数据记录</param>
        /// <returns></returns>
        public static ISet GetSet(RecordBase record)
        {
            return GetSet(record.SetId);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <param name="scheme">数据表结构</param>
        /// <returns></returns>
        public static ITable GetTable(int setId, Type scheme)
        {
            return GetSet(setId).GetTable(scheme);
        }
        
        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="record">数据记录</param>
        /// <returns></returns>
        public static ITable GetTable(RecordBase record)
        {
            return GetSet(record.SetId).GetTable(record);
        }

        /// <summary>
        /// 分配数据库
        /// </summary>
        /// <param name="setId">数据库ID</param>
        public static void Alloc(int setId)
        {
            if (!IsExist(setId))
            {
                var set = Framework.core.ReferencePool.ReferencePool.Acquire<ModelSet>();
                set.Ctor(setId);
                _sets.Add(setId, set);
            }
        }
        
        /// <summary>
        /// 分配数据表
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void Alloc(int setId, Type scheme)
        {
            GetSet(setId).Alloc(scheme);
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="setId">数据库ID</param>
        public static void Release(int setId)
        {
            if (IsExist(setId))
            {
                var set = _sets[setId];
                Framework.core.ReferencePool.ReferencePool.Release(set);
                _sets.Remove(setId);
            }
        }
        
        /// <summary>
        /// 释放数据表
        /// </summary>
        /// <param name="setId">数据库ID</param>
        public static void Free(int setId)
        {
            if (IsExist(setId))
            {
                Framework.core.ReferencePool.ReferencePool.Release(_sets[setId]);
                _sets.Remove(setId);
            }
        }

        /// <summary>
        /// 释放数据表
        /// </summary>
        /// <param name="setId">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void Free(int setId, Type scheme)
        {
            GetSet(setId).Free(scheme);
        }
        
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public static RecordBase Create(int setIdentity, Type scheme)
        {
            return GetSet(setIdentity).Create(scheme);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <returns></returns>
        public static TRecord Create<TRecord>(int setIdentity) where TRecord : RecordBase
        {
            return GetSet(setIdentity).Create(typeof(TRecord)) as TRecord;
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        public static void Attach(RecordBase record)
        {
            GetSet(record).Attach(record);
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="records">表记录集</param>
        public static void Attach(ICollection<RecordBase> records)
        {
            foreach (var item in records)
            {
                Attach(item);
            }
        }
        
        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="records">表记录集</param>
        public static void Attach<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            foreach (var item in records)
            {
                Attach(item);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        public static void Delete(RecordBase record)
        {
            GetSet(record).Delete(record);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="records">表记录集</param>
        public static void Delete(ICollection<RecordBase> records)
        {
            foreach (var item in records)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public static void Delete<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            foreach (var item in records)
            {
                Delete(item);
            }
        }

        /// <summary>
        /// 删除某张表的数据
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void Delete(int setIdentity, Type scheme)
        {
            GetSet(setIdentity).Delete(scheme);
        }

        /// <summary>
        /// 删除某张表的数据
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void Delete<TRecord>(int setIdentity) where TRecord : RecordBase
        {
            GetSet(setIdentity).Delete(typeof(TRecord));
        }

        /// <summary>
        /// 删除某个库的数据
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        public static void Delete(int setIdentity)
        {
            GetSet(setIdentity).Delete();
        }

        /// <summary>
        /// 重置某条记录的数据
        /// </summary>
        /// <param name="record">表记录</param>
        public static void ResetData(RecordBase record)
        {
            GetSet(record).ResetData(record);
        }

        /// <summary>
        /// 重置记录的数据
        /// </summary>
        /// <param name="records">表记录集</param>
        public static void ResetData(ICollection<RecordBase> records)
        {
            foreach (var item in records)
            {
                ResetData(item);
            }
        }

        /// <summary>
        /// 重置记录的数据
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public static void ResetData<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            foreach (var item in records)
            {
                ResetData(item);
            }
        }

        /// <summary>
        /// 重置某张表的数据
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void ResetData(int setIdentity, Type scheme)
        {
            GetSet(setIdentity).ResetData(scheme);
        }

        /// <summary>
        /// 重置表
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        public static void ResetData<TRecord>(int setIdentity) where TRecord : RecordBase
        {
            ResetData(setIdentity, typeof(TRecord));
        }

        /// <summary>
        /// 重置某个库的数据
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        public static void ResetData(int setIdentity)
        {
            GetSet(setIdentity).ResetData();
        }

        /// <summary>
        /// 重置某条记录的状态
        /// </summary>
        /// <param name="record">表记录</param>
        public static void ResetState(RecordBase record)
        {
            GetSet(record).ResetState(record);
        }

        /// <summary>
        /// 重置记录的状态
        /// </summary>
        /// <param name="records">表记录集</param>
        public static void ResetState(ICollection<RecordBase> records)
        {
            foreach (var item in records)
            {
                ResetState(item);
            }
        }

        /// <summary>
        /// 重置记录的状态
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public static void ResetState<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            foreach (var item in records)
            {
                ResetState(item);
            }
        }

        /// <summary>
        /// 重置某张表的状态
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        public static void ResetState(int setIdentity, Type scheme)
        {
            GetSet(setIdentity).ResetState(scheme);
        }

        /// <summary>
        /// 重置表状态
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        public static void ResetState<TRecord>(int setIdentity) where TRecord : RecordBase
        {
            ResetState(setIdentity, typeof(TRecord));
        }

        /// <summary>
        /// 重置某个库的状态
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        public static void ResetState(int setIdentity)
        {
            GetSet(setIdentity).ResetState();
        }

        /// <summary>
        /// 获取记录集
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static List<RecordBase> GetAssembly(int setIdentity, Type scheme, bool isIncludeDeleted = false)
        {
            return GetSet(setIdentity).GetAssembly(scheme, isIncludeDeleted);
        }

        /// <summary>
        /// 获取新增记录集
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public static List<RecordBase> GetNewAssembly(int setIdentity, Type scheme)
        {
            return GetSet(setIdentity).GetNewAssembly(scheme);
        }

        /// <summary>
        /// 获取更新记录集
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public static List<RecordBase> GetDirtyAssembly(int setIdentity, Type scheme)
        {
            return GetSet(setIdentity).GetDirtyAssembly(scheme);
        }

        /// <summary>
        /// 获取删除记录集
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public static List<RecordBase> GetDeleteAssembly(int setIdentity, Type scheme)
        {
            return GetSet(setIdentity).GetDeleteAssembly(scheme);
        }

        /// <summary>
        /// 获取干净记录集
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public static List<RecordBase> GetCleanAssembly(int setIdentity, Type scheme)
        {
            return GetSet(setIdentity).GetCleanAssembly(scheme);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static bool IsExist(RecordBase record, bool isIncludeDeleted = false)
        {
            return GetSet(record).IsExist(record, isIncludeDeleted);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static bool IsExist(int setIdentity, Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return GetSet(setIdentity).IsExist(scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static bool IsExist<TRecord>(int setIdentity, Func<TRecord, bool> where, bool isIncludeDeleted = false) where TRecord : RecordBase
        {
            return IsExist(setIdentity, typeof(TRecord), p => where?.Invoke(p as TRecord) ?? true, isIncludeDeleted);
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static int Count(int setIdentity, Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return GetSet(setIdentity).Count(scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static int Count<TRecord>(int setIdentity, Func<TRecord, bool> where, bool isIncludeDeleted = false) where TRecord : RecordBase
        {
            return Count(setIdentity, typeof(TRecord), p => where?.Invoke(p as TRecord) ?? true, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static List<RecordBase> Find(int setIdentity, Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted)
        {
            return GetSet(setIdentity).Find(scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static List<TRecord> Find<TRecord>(int setIdentity, Func<TRecord, bool> where, bool isIncludeDeleted) where TRecord : RecordBase
        {
            return Find(setIdentity, typeof(TRecord), p => where?.Invoke(p as TRecord) ?? true, isIncludeDeleted).Select(p => p as TRecord).ToList();
        }
        
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static RecordBase FirstOrDefault(int setIdentity, Type scheme,bool isIncludeDeleted)
        {
            return GetSet(setIdentity).FirstOrDefault(scheme, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static RecordBase FirstOrDefault(int setIdentity, Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted)
        {
            return GetSet(setIdentity).FirstOrDefault(scheme, where, isIncludeDeleted);
        }
        
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static TRecord FirstOrDefault<TRecord>(int setIdentity,bool isIncludeDeleted) where TRecord : RecordBase
        {
            return FirstOrDefault(setIdentity, typeof(TRecord), isIncludeDeleted) as TRecord;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="where"></param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public static TRecord FirstOrDefault<TRecord>(int setIdentity, Func<TRecord, bool> where, bool isIncludeDeleted) where TRecord : RecordBase
        {
            return FirstOrDefault(setIdentity, typeof(TRecord), p => where?.Invoke(p as TRecord) ?? true, isIncludeDeleted) as TRecord;
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public static UniTask<object> Package(RecordBase record, params object[] extraObjects)
        {
            return GetSet(record).Package(record, extraObjects);
        }

        // /// <summary>
        // /// 解析
        // /// </summary>
        // /// <param name="setIdentity">数据库ID</param>
        // /// <param name="scheme">表结构</param>
        // /// <param name="jsonModels">Json数据</param>
        // /// <param name="extraObjects">额外数据</param>
        // /// <returns></returns>
        // public static UniTask Parse(int setIdentity, Type scheme, object jsonModels, params object[] extraObjects)
        // {
        //     return GetSet(setIdentity).Parse(scheme, jsonModels, extraObjects);
        // }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <typeparam name="TJson">Json数据类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="jsonModels">Json数据</param>
        /// <returns></returns>
        public static UniTask<TRecord> Parse<TRecord,TJson>(int setIdentity, TJson jsonModels) where TRecord : RecordBase
        {
            return GetSet(setIdentity).Parse<TRecord,TJson>(jsonModels);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <typeparam name="TJson">Json数据类型</typeparam>
        /// <typeparam name="TArg">参数类型</typeparam>
        /// <param name="setIdentity">数据库ID</param>
        /// <param name="jsonModels">Json数据</param>
        /// <param name="arg1">参数</param>
        /// <returns></returns>
        public static UniTask<TRecord> Parse<TRecord,TJson,TArg>(int setIdentity, TJson jsonModels,TArg arg1) where TRecord : RecordBase
        {
            return GetSet(setIdentity).Parse<TRecord,TJson,TArg>(jsonModels,arg1);
        }
    }
}