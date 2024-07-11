using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Framework.core;
using Framework.core.ReferencePool;
using Game.Scripts.Core.Repository.Attribute;
using Game.Scripts.Core.Repository.External;
using UnityEngine;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 数据表
    /// </summary>
    internal class ModelTable : ITable,IReference
    {
        private Dictionary<int, RecordBase> _records;
        private PropertyInfo _primaryKey;
        
        /// <summary>
        /// 获取数据库
        /// </summary>
        public ModelSet ModelSet { get; private set; }
        
        /// <summary>
        /// 获取表结构
        /// </summary>
        public Type Scheme { get; private set; }
        
        /// <summary>
        /// 获取ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 构造数据表
        /// </summary>
        /// <param name="modelSet">数据库</param>
        /// <param name="scheme">表结构</param>
        internal void SetCtor(ModelSet modelSet, Type scheme)
        {
            Id = Guid.NewGuid().ToString();
            ModelSet = modelSet;
            Scheme = scheme;
            _records = new Dictionary<int, RecordBase>();
            ParsePrimaryKey();
        }
        

        /// <summary>
        /// 获取比较
        /// </summary>
        public Func<RecordBase, RecordBase, bool> CompareHandle { get; }

        /// <summary>
        /// 获取有效的记录集
        /// </summary>
        public List<RecordBase> Assembly
        {
            get
            {
                return _records.Values.Where(p => p.RecordState != RecordState.Delete).ToList();
            }
        }

        /// <summary>
        /// 获取标记删除的记录集
        /// </summary>
        public List<RecordBase> DeletedAssembly
        {
            get
            {
                return _records.Values.Where(p => p.RecordState == RecordState.Delete).ToList();
            }
        }

        /// <summary>
        /// 获取被污染的记录集
        /// </summary>
        public List<RecordBase> DirtyAssembly
        {
            get
            {
                return _records.Values.Where(p => p.RecordState == RecordState.Dirty).ToList();
            }
        }

        /// <summary>
        /// 获取新增的记录集
        /// </summary>
        public List<RecordBase> NewAssembly
        {
            get
            {
                return _records.Values.Where(p => p.RecordState == RecordState.New).ToList();
            }
        }

        /// <summary>
        /// 获取未变化的记录集
        /// </summary>
        public List<RecordBase> CleanAssembly
        {
            get
            {
                return _records.Values.Where(p => p.RecordState == RecordState.Clean).ToList();
            }
        }

        /// <summary>
        /// 获取所有记录集
        /// </summary>
        public List<RecordBase> TotalAssembly => _records.Values.ToList();

        /// <summary>
        /// 清空记录，暴力清空，不触发任何事件
        /// </summary>
        public void ViolenceClear()
        {
            foreach (var record in _records.Values)
            {
                    
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public RecordBase Create()
        {             
            RecordBase record = Framework.core.ReferencePool.ReferencePool.Acquire(Scheme) as RecordBase;
            record!.SetId = ModelSet.SetIdentity;
            record.Ctor();
            return record;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        public void Delete(RecordBase record)
        {
            if (record.RecordState == RecordState.New)
            {
                record.OnDeleted();
                Dispose(record);
            }
            else
            {
                record.RecordState = RecordState.Delete;
                record.OnDeleted();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            foreach (var record in Assembly)
            {
                try
                {
                    Delete(record);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetData(RecordBase record)
        {
            if (record.RecordState == RecordState.New)
            {
                Delete(record);
            }
            else
            {
                record.RollbackProperties();
                record.RecordState = RecordState.Clean;
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void ResetData()
        {
            foreach (var record in TotalAssembly)
            {
                ResetData(record);
            }
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetState(RecordBase record)
        {
            if (record.RecordState == RecordState.Delete)
            {
                Dispose(record);
            }
            else
            {
                record.ClearPropertyCaches();
                record.RecordState = RecordState.Clean;
            }
        }

        /// <summary>
        /// 重置整张表的状态
        /// </summary>
        public void ResetState()
        {
            foreach (var record in TotalAssembly)
            {
                ResetState(record);
            }
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        public void Attach(RecordBase record)
        {
            if (IsExist(record,true))
            {
                if (record.RecordState == RecordState.Delete)
                {
                    record.RecordState = RecordState.Dirty;
                }
            }
            else
            {
                record.RecordState = RecordState.New;
                _records[GetPrimaryKey(record).GetHashCode()] = record;
            }
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="records">表记录集</param>
        public void Attach(IEnumerable<RecordBase> records)
        {
            foreach (var record in records)
            {
                Attach(record);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist(RecordBase record, bool isIncludeDeleted = false)
        {
            if (_records.ContainsKey(GetPrimaryKey(record).GetHashCode()))
            {
                return isIncludeDeleted || record.RecordState != RecordState.Delete;
            }
            return false;
        }

        /// <summary>
        /// 统计符合条件的记录数
        /// </summary>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public int Count(Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            if (where == null)
            {
                throw new FrameworkException($"数据库:{ModelSet}  数据表:{Scheme}统计符合条件的记录数失败,原因:查询条件为空");
            }

            return _records.Values.Count(p =>
                where(p) && (isIncludeDeleted || p.RecordState != RecordState.Delete));
        }

        /// <summary>
        /// 筛选符合条件的所有记录
        /// </summary>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<RecordBase> Find(Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            if (where  == null)
            {
                throw new FrameworkException($"数据库:{ModelSet}  数据表:{Scheme}筛选符合条件的所有记录失败,原因:查询条件为空");
            }

            return _records.Values.Where(p => where(p) && (isIncludeDeleted || p.RecordState != RecordState.Delete))
                .ToList();
        }

        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(bool isIncludeDeleted = false)
        {
            return _records.Values.FirstOrDefault(p => isIncludeDeleted || p.RecordState != RecordState.Delete);
        }

        /// <summary>
        /// 获取第一个
        /// </summary>
        /// <param name="match">指定的匹配条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(Func<RecordBase, bool> match, bool isIncludeDeleted = false)
        {
            if (match  == null)
            {
                throw new FrameworkException($"数据库:{ModelSet}  数据表:{Scheme}筛选符合条件的第一个记录失败,原因:匹配条件为空");
            }

            return _records.Values.FirstOrDefault(p => match(p) && (isIncludeDeleted || p.RecordState != RecordState.Delete));
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public UniTask<object> Package(RecordBase record, params object[] extraObjects)
        {
            return record.PackageToJson(extraObjects);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public async UniTask<TRecord> Parse<TRecord, TJson, TArg>(TJson jsonModels, TArg extraObjects)
            where TRecord : RecordBase
        {
            if (jsonModels == null)
            {
                return default;
            }

            RecordBase record = Create();
            record.RecordState = RecordState.Clean;
            var t = await record.ParseFromJson(jsonModels, extraObjects);
            
            if (!t)
            {
                throw new FrameworkException($"表记录{GetPrimaryKey(record)}解析失败");
            }

            if (IsExist(record))
            {
                throw new FrameworkException($"表记录{GetPrimaryKey(record)}解析失败,原因:表记录已存在,无需重复填充");
            }
            Attach(record);
            return (TRecord)record;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <returns></returns>
        public async UniTask<TRecord> Parse<TRecord, TJson>(TJson jsonModels)
            where TRecord : RecordBase
        {
            if (jsonModels == null)
            {
                return default;
            }

            RecordBase record = Create();
            record.RecordState = RecordState.Clean;
            var t = await record.ParseFromJson<TJson, object>(jsonModels, null);
            if (!t)
            {
                throw new FrameworkException($"表记录{GetPrimaryKey(record)}解析失败");
            }

            if (IsExist(record))
            {
                throw new FrameworkException($"表记录{GetPrimaryKey(record)}解析失败,原因:表记录已存在,无需重复填充");
            }

            Attach(record);
            return (TRecord)record;
        }

        /// <summary>
        /// 获取主键值
        /// </summary>
        /// <param name="record">表记录</param>
        /// <returns></returns>
        public object GetPrimaryKey(RecordBase record)
        {
            return _primaryKey.GetValue(record, null);
        }
        
        /// <summary>
        /// 释放记录
        /// </summary>
        /// <param name="record">表记录</param>
        public void Dispose(RecordBase record)
        {
            _records.Remove(GetPrimaryKey(record).GetHashCode());
            Framework.core.ReferencePool.ReferencePool.Release(record);
        }
        
        /// <summary>
        /// 清理引用。
        /// </summary>
        public void Clear()
        {
            Id = null;

            try
            {
                for (int i = 0; i < _records.Values.Count; i++)
                {
                    Dispose(_records.Values.ElementAt(i));
                }
                // foreach (var record in _records.Values)
                // {
                //     Dispose(record);
                // }
            }
            catch (Exception e)
            {
                Debug.LogError($"ModelTable Clear error:{e}");
            }
        
            _records.Clear();
            _records = null;
            _primaryKey = null;
            ModelSet = null;
            Scheme = null;
        }
        
        /// <summary>
        /// 解析主键
        /// </summary>
        private void ParsePrimaryKey()
        {
            var properties = Scheme
                .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>(false) != null).ToArray();
            
            if (properties.Length == 0)
            {
                throw new FrameworkException($"数据表{Scheme}解析主键失败,原因:主键未设置");
            }
            if (properties.Length > 1)
            {
                throw new FrameworkException($"数据表{Scheme}解析主键失败,原因:主键仅能设置一个");
            }
            
            _primaryKey = properties[0];
        }
    }
}