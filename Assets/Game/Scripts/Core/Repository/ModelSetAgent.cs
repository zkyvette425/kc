using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Framework.core;
using Framework.core.ReferencePool;
using Game.Scripts.Core.Repository.External;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 数据库代理
    /// </summary>
    public class ModelSetAgent : IReference
    {
        private int _moduleId;
        
        /// <summary>
        /// 获取模块ID
        /// </summary>
        public int ModuleId => _moduleId;

        /// <summary>
        /// 获取所属模块
        /// </summary>
        public ModuleBase Module => ModuleRepository.GetModule(_moduleId);

        /// <summary>
        /// 构建数据库代理
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        internal void Ctor(int moduleId)
        {
            _moduleId = moduleId;
            Repository.Alloc(_moduleId);
        }

        /// <summary>
        /// 获取当前模块的库
        /// </summary>
        /// <returns></returns>
        public ISet GetSet()
        {
            return Repository.GetSet(_moduleId);
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public ITable GetTable(Type scheme)
        {
            return GetSet().GetTable(scheme);
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="record">表记录</param>
        /// <returns></returns>
        public ITable GetTable<TRecord>(RecordBase record) where TRecord : RecordBase
        {
            return GetTable(typeof(TRecord));
        }

        /// <summary>
        /// 分配表
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Alloc(Type scheme)
        {
            Repository.Alloc(_moduleId, scheme);
        }

        /// <summary>
        /// 分配表
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        public void Alloc<TRecord>() where TRecord : RecordBase
        {
            Alloc(typeof(TRecord));
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            Repository.Free(_moduleId);
        }

        /// <summary>
        /// 释放表
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Free(Type scheme)
        {
            Repository.Free(_moduleId, scheme);
        }

        /// <summary>
        /// 释放表
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        public void Free<TRecord>() where TRecord : RecordBase
        {
            Free(typeof(TRecord));
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public RecordBase Create(Type scheme)
        {
            return Repository.Create(_moduleId, scheme);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <returns></returns>
        public TRecord Create<TRecord>() where TRecord : RecordBase
        {
            return Repository.Create<TRecord>(_moduleId);
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        public void Attach(RecordBase record)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"附加表记录:{record.SetId}于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Attach(record);
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="records">表记录集</param>
        public void Attach(ICollection<RecordBase> records)
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"附加表记录集于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Attach(records);
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public void Attach<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"附加表记录集于数据库于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Attach(records);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        public void Delete(RecordBase record)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"删除表记录:{record.SetId}于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Delete(record);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="records">表记录集</param>
        public void Delete(ICollection<RecordBase> records)
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"删除表记录集于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Delete(records);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public void Delete<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"删除表记录集于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.Delete(records);
        }

        /// <summary>
        /// 删除某张表数据
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Delete(Type scheme)
        {
            Repository.Delete(_moduleId, scheme);
        }

        /// <summary>
        /// 删除某张表数据
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        public void Delete<TRecord>() where TRecord : RecordBase
        {
            Repository.Delete<TRecord>(_moduleId);
        }

        /// <summary>
        /// 删除当前库
        /// </summary>
        public void Delete()
        {
            Repository.Delete(_moduleId);
        }

        /// <summary>
        /// 重置表记录
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetData(RecordBase record)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"重置表记录:{record.SetId}于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetData(record);
        }

        /// <summary>
        /// 重置表记录集
        /// </summary>
        /// <param name="records">表记录集</param>
        public void ResetData(ICollection<RecordBase> records)
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"重置表记录集于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetData(records);
        }

        /// <summary>
        /// 重置表记录集
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public void ResetData<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"重置表记录集于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetData(records);
        }

        /// <summary>
        /// 重置某张表数据
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void ResetData(Type scheme)
        {
            Repository.ResetData(_moduleId, scheme);
        }

        /// <summary>
        /// 重置某张表数据
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        public void ResetData<TRecord>() where TRecord : RecordBase
        {
            Repository.ResetData<TRecord>(_moduleId);
        }

        /// <summary>
        /// 重置当前库
        /// </summary>
        public void ResetData()
        {
            Repository.ResetData(_moduleId);
        }

        /// <summary>
        /// 重置某条记录的状态
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetState(RecordBase record)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"重置记录:{record.SetId}的状态于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetState(record);
        }

        /// <summary>
        /// 重置记录集的状态
        /// </summary>
        /// <param name="records">表记录集</param>
        public void ResetState(ICollection<RecordBase> records)
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"重置记录集的状态于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetState(records);
        }

        /// <summary>
        /// 重置记录集的状态
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="records">表记录集</param>
        public void ResetState<TRecord>(ICollection<TRecord> records) where TRecord : RecordBase
        {
            if (records == null)
            {
                return;
            }

            if (records.Any(p => p.SetId != _moduleId))
            {
                throw new FrameworkException($"重置记录集的状态于数据库:{_moduleId}失败,原因:ID不一致");
            }

            Repository.ResetState(records);
        }

        /// <summary>
        /// 重置表状态
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void ResetState(Type scheme)
        {
            Repository.ResetState(_moduleId, scheme);
        }

        /// <summary>
        /// 删除某张表数据
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        public void ResetState<TRecord>() where TRecord : RecordBase
        {
            Repository.ResetState<TRecord>(_moduleId);
        }

        /// <summary>
        /// 删除当前库
        /// </summary>
        public void ResetState()
        {
            Repository.ResetState(_moduleId);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist(RecordBase record, bool isIncludeDeleted = false)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"判断是否存在表记录:{record.SetId}于数据库:{_moduleId}失败,原因:ID不一致");
            }

            return Repository.IsExist(record, isIncludeDeleted);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return Repository.IsExist(_moduleId, scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist<TRecord>(Func<TRecord, bool> where, bool isIncludeDeleted = false)
            where TRecord : RecordBase
        {
            return Repository.IsExist(_moduleId, where, isIncludeDeleted);
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public int Count(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return Repository.Count(_moduleId, scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public int Count<TRecord>(Func<TRecord, bool> where, bool isIncludeDeleted = false) where TRecord : RecordBase
        {
            return Repository.Count<TRecord>(_moduleId, where, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<RecordBase> Find(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted)
        {
            return Repository.Find(_moduleId, scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<TRecord> Find<TRecord>(Func<TRecord, bool> where, bool isIncludeDeleted) where TRecord : RecordBase
        {
            return Repository.Find<TRecord>(_moduleId, where, isIncludeDeleted);
        }
        
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(Type scheme,bool isIncludeDeleted = false)
        {
            return Repository.FirstOrDefault(_moduleId, scheme, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return Repository.FirstOrDefault(_moduleId, scheme, where, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="where">条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public TRecord FirstOrDefault<TRecord>(Func<TRecord, bool> where, bool isIncludeDeleted = false)
            where TRecord : RecordBase
        {
            return Repository.FirstOrDefault<TRecord>(_moduleId, where, isIncludeDeleted);
        }
        
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public TRecord FirstOrDefault<TRecord>(bool isIncludeDeleted = false)
            where TRecord : RecordBase
        {
            return Repository.FirstOrDefault<TRecord>(_moduleId, isIncludeDeleted);
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public UniTask<object> Package(RecordBase record, params object[] extraObjects)
        {
            if (record == null)
            {
                throw new ArgumentNullException();
            }

            if (record.SetId != _moduleId)
            {
                throw new FrameworkException($"附加表记录:{record.SetId}于数据库:{_moduleId}失败,原因:ID不一致");
            }

            return Repository.Package(record, extraObjects);
        }

        // /// <summary>
        // /// 解析
        // /// </summary>
        // /// <param name="scheme">表结构</param>
        // /// <param name="jsonModels">Json数据</param>
        // /// <param name="extraObjects">额外数据</param>
        // /// <returns></returns>
        // public UniTask Parse(Type scheme, object jsonModels, params object[] extraObjects)
        // {
        //     return Repository.Parse(_moduleId, scheme, jsonModels, extraObjects);
        // }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <typeparam name="TJson"></typeparam>
        /// <param name="jsonModels">Json数据</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public UniTask<TRecord> Parse<TRecord,TJson>(TJson jsonModels) where TRecord : RecordBase
        {
            return Repository.Parse<TRecord,TJson>(_moduleId, jsonModels);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="TRecord">表记录类型</typeparam>
        /// <typeparam name="TJson"></typeparam>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="jsonModels">Json数据</param>
        /// <param name="arg1"></param>
        /// <returns></returns>
        public UniTask<TRecord> Parse<TRecord,TJson,TArg>(TJson jsonModels,TArg arg1) where TRecord : RecordBase
        {
            return Repository.Parse<TRecord,TJson,TArg>(_moduleId, jsonModels,arg1);
        }

        /// <summary>
        /// 清理引用。
        /// </summary>
        public void Clear()
        {
            Delete();
        }
    }
}