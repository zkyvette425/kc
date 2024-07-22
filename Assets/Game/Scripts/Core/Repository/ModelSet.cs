using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Framework.core.ReferencePool;
using Game.Scripts.Core.Repository.External;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 数据库
    /// </summary>
    internal class ModelSet : ISet,IReference
    {
        private Dictionary<int, ModelTable> _tables;
        
        /// <summary>
        /// 获取库ID
        /// </summary>
        public int SetIdentity { get; private set; }

        /// <summary>
        /// 构建数据库
        /// </summary>
        /// <param name="id">数据库ID</param>
        internal void Ctor(int id)
        {
            _tables = new Dictionary<int, ModelTable>();
            SetIdentity = id;
        }

        /// <summary>
        /// 获取表结构集
        /// </summary>
        public List<Type> Schemes => _tables.Values.Select(p => p.Scheme).ToList();


        /// <summary>
        /// 判断指定表是否存在
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public bool IsExist(Type scheme)
        {
            return _tables.ContainsKey(scheme.GetHashCode());
        }

        /// <summary>
        /// 判断指定表是否存在
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public bool IsExist<TTable>() where TTable : RecordBase
        {
            return IsExist(typeof(TTable));
        }

        /// <summary>
        /// 分配表
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Alloc(Type scheme)
        {
            var id = scheme.GetHashCode();
            if (!_tables.ContainsKey(id))
            {
                var table = Framework.core.ReferencePool.ReferencePool.Acquire<ModelTable>();
                table.SetCtor(this,scheme);
                _tables.Add(id, table);
            }
        }

        /// <summary>
        /// 分配表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void Alloc<TTable>() where TTable : RecordBase
        {
            Alloc(typeof(TTable));
        }

        /// <summary>
        /// 释放表
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Free(Type scheme)
        {
            int id = scheme.GetHashCode();
            if (_tables.TryGetValue(id,out var table))
            {
                Framework.core.ReferencePool.ReferencePool.Release(table);
                _tables.Remove(id);
            }
        }

        /// <summary>
        /// 释放表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void Free<TTable>() where TTable : RecordBase
        {
            Free(typeof(TTable));
        }
        
        /// <summary>
        /// 清空所有表，暴力清空，不触发任何事件
        /// </summary>
        public void ViolenceClear()
        {
            foreach (var table in _tables.Values)
            {
                Framework.core.ReferencePool.ReferencePool.Release(table);
            }
            _tables.Clear();
        }

        /// <summary>
        /// 清空某张表，暴力清空，不触发任何事件
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void ViolenceClear(Type scheme)
        {
            var id = scheme.GetHashCode();
            if (_tables.TryGetValue(id,out var table))
            {
                Framework.core.ReferencePool.ReferencePool.Release(table);
                _tables.Remove(id);
            }
        }

        /// <summary>
        /// 清空某张表，暴力清空，不触发任何事件
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void ViolenceClear<TTable>() where TTable : RecordBase
        {
            ViolenceClear(typeof(TTable));
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public ITable GetTable(Type scheme)
        {
            var id = scheme.GetHashCode();
            if (!_tables.ContainsKey(id))
            {
                Alloc(scheme);
            }
            return _tables[id];
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public ITable GetTable<TTable>() where TTable : RecordBase
        {
            return GetTable(typeof(TTable));
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="record">表记录</param>
        /// <returns></returns>
        public ITable GetTable(RecordBase record)
        {
            return GetTable(record.GetType());
        }
        

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="record">表记录</param>
        public void Delete(RecordBase record)
        {
            if (IsExist(record.GetType()))
            {
                GetTable(record).Delete(record);
            }
        }

        /// <summary>
        /// 删除某张表
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void Delete(Type scheme)
        {
            if (IsExist(scheme))
            {
                var table =GetTable(scheme);
                table.Delete();
                Free(scheme);
            }
        }

        /// <summary>
        /// 删除某张表
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void Delete<TTable>() where TTable : RecordBase
        {
            Delete(typeof(TTable));
        }

        /// <summary>
        /// 删除整个库
        /// </summary>
        public void Delete()
        {
            foreach (var scheme in Schemes)
            {
                Delete(scheme);
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="type"></param>
        public RecordBase Create(Type type)
        {
            return GetTable(type).Create();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public TTable Create<TTable>() where TTable : RecordBase
        {
            return Create(typeof(TTable)) as TTable;
        }

        /// <summary>
        /// 创建并附加
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RecordBase CreateAndAttach(Type type)
        {
            var record = Create(type);
            Attach(record);
            return record;
        }

        /// <summary>
        /// 创建并附加
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public TTable CreateAndAttach<TTable>() where TTable : RecordBase
        {
            return (TTable)CreateAndAttach(typeof(TTable));
        }

        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="record">表记录</param>
        public void Attach(RecordBase record)
        {
            GetTable(record.GetType()).Attach(record);
        }

        /// <summary>
        /// 附加(为了性能,此处人为约束。集合项类型必须一致)
        /// </summary>
        /// <param name="records">表记录集</param>
        public void Attach(ICollection<RecordBase> records)
        {
            if (records == null || records.Count == 0)
            {
                return;
            }
            var table = GetTable(records.First());
            table.Attach(records);
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetData(RecordBase record)
        {
            GetTable(record.GetType()).ResetData(record);
        }

        /// <summary>
        /// 重置某张表的数据
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void ResetData(Type scheme)
        {
            GetTable(scheme).ResetData();
        }

        /// <summary>
        /// 重置某张表的数据
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void ResetData<TTable>() where TTable : RecordBase
        {
            ResetData(typeof(TTable));
        }

        /// <summary>
        /// 重置整个库的数据
        /// </summary>
        public void ResetData()
        {
            foreach (var scheme in Schemes)
            {
                ResetData(scheme);
            }
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        /// <param name="record">表记录</param>
        public void ResetState(RecordBase record)
        {
            GetTable(record).ResetState(record);
        }

        /// <summary>
        /// 重置某张表的状态
        /// </summary>
        /// <param name="scheme">表结构</param>
        public void ResetState(Type scheme)
        {
            GetTable(scheme).ResetState();
        }

        /// <summary>
        /// 重置某张表的状态
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        public void ResetState<TTable>() where TTable : RecordBase
        {
            ResetState(typeof(TTable));
        }

        /// <summary>
        /// 重置整个库的状态
        /// </summary>
        public void ResetState()
        {
            foreach (var scheme in Schemes)
            {
                ResetState(scheme);
            }
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist(RecordBase record, bool isIncludeDeleted = false)
        {
            if (!IsExist(record.GetType()))
            {
                return false;
            }
            return GetTable(record).IsExist(record, isIncludeDeleted);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return Count(scheme, where, isIncludeDeleted) > 0;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public bool IsExist<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return Count(where, isIncludeDeleted) > 0;
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public int Count(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            if (!IsExist(scheme))
            {
                return 0;
            }
            return GetTable(scheme).Count(where, isIncludeDeleted);
        }

        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public int Count<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return Count(typeof(TTable), p => where(p as TTable), isIncludeDeleted);
        }

        /// <summary>
        /// 获取记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<RecordBase> GetAssembly(Type scheme, bool isIncludeDeleted = false)
        {
            return isIncludeDeleted ? GetTable(scheme).TotalAssembly : GetTable(scheme).Assembly;
        }

        /// <summary>
        /// 获取记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<TTable> GetAssembly<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return GetAssembly(typeof(TTable), isIncludeDeleted).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 获取未污染记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public List<RecordBase> GetCleanAssembly(Type scheme)
        {
            return GetTable(scheme).CleanAssembly;
        }

        /// <summary>
        /// 获取未污染记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public List<TTable> GetCleanAssembly<TTable>() where TTable : RecordBase
        {
            return GetCleanAssembly(typeof(TTable)).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 获取更新记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public List<RecordBase> GetDirtyAssembly(Type scheme)
        {
            return GetTable(scheme).DirtyAssembly;
        }

        /// <summary>
        /// 获取更新记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public List<TTable> GetDirtyAssembly<TTable>() where TTable : RecordBase
        {
            return GetDirtyAssembly(typeof(TTable)).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 获取新增记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public List<RecordBase> GetNewAssembly(Type scheme)
        {
            return GetTable(scheme).NewAssembly;
        }

        /// <summary>
        /// 获取新增记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public List<TTable> GetNewAssembly<TTable>() where TTable : RecordBase
        {
            return GetNewAssembly(typeof(TTable)).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 获取软删除记录集
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public List<RecordBase> GetDeleteAssembly(Type scheme)
        {
            return GetTable(scheme).DeletedAssembly;
        }

        /// <summary>
        /// 获取软删除记录集
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public List<TTable> GetDeleteAssembly<TTable>() where TTable : RecordBase
        {
            return GetDeleteAssembly(typeof(TTable)).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="record">表记录</param>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        public UniTask<object> Package(RecordBase record, params object[] extraObjects)
        {
            return GetTable(record.GetType()).Package(record, extraObjects);
        }  

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<RecordBase> Find(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return GetTable(scheme).Find(where, isIncludeDeleted);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<TTable> Find<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return Find(typeof(TTable), p => where((TTable)p), isIncludeDeleted).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public List<TTable> Find<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return Find(typeof(TTable), _ => true, isIncludeDeleted).Select(p => (TTable)p).ToList();
        }

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(Type scheme, bool isIncludeDeleted = false)
        {
            return GetTable(scheme).FirstOrDefault(isIncludeDeleted);
        }

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public RecordBase FirstOrDefault(Type scheme, Func<RecordBase, bool> where, bool isIncludeDeleted = false)
        {
            return GetTable(scheme).FirstOrDefault(where, isIncludeDeleted);
        }

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="where">指定的查询条件</param>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public TTable FirstOrDefault<TTable>(Func<TTable, bool> where, bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return (TTable)FirstOrDefault(typeof(TTable), p => where((TTable)p), isIncludeDeleted);
        }

        /// <summary>
        /// 查找第一条
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="isIncludeDeleted">是否包含已标记删除的记录</param>
        /// <returns></returns>
        public TTable FirstOrDefault<TTable>(bool isIncludeDeleted = false) where TTable : RecordBase
        {
            return (TTable)FirstOrDefault(typeof(TTable), _ => true, isIncludeDeleted);
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <typeparam name="TRecord">表类型</typeparam>
        /// <typeparam name="TJson">json数据类型</typeparam>
        /// <returns></returns>
        public UniTask<TRecord> Parse<TRecord, TJson>(TJson jsonModels) where TRecord : RecordBase
        {
            return GetTable<TRecord>().Parse<TRecord,TJson>(jsonModels);
        }
        
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="jsonModels">json数据</param>
        /// <param name="arg1">参数</param>
        /// <typeparam name="TRecord">表类型</typeparam>
        /// <typeparam name="TJson">json数据类型</typeparam>
        /// <typeparam name="TArg">参数类型</typeparam>
        /// <returns></returns>
        public UniTask<TRecord> Parse<TRecord, TJson, TArg>(TJson jsonModels, TArg arg1) where TRecord : RecordBase
        {
            return GetTable<TRecord>().Parse<TRecord, TJson, TArg>(jsonModels, arg1);
        }


        /// <summary>
        /// Cross
        /// </summary>
        /// <param name="value"></param>
        public UniTask Cross(RecordBase value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cross
        /// </summary>
        public UniTask Cross()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cross
        /// </summary>
        /// <param name="scheme">表结构</param>
        /// <returns></returns>
        public UniTask Cross(Type scheme)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// cross
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <returns></returns>
        public UniTask Cross<TTable>() where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        public void ForkFromBase(int baseSetIdentity, Type scheme, Func<RecordBase, bool> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        public void ForkFromBase<TTable>(int baseSetIdentity, Func<TTable, bool> where) where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="baseScheme"></param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        public void ForkFromBase(int baseSetIdentity, Type baseScheme, Type scheme, Func<RecordBase, bool> where, Action<RecordBase, RecordBase> convert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        public void ForkFromBase<TBase, TTable>(int baseSetIdentity, Func<TBase, bool> where, Action<TBase, TTable> convert) where TBase : RecordBase where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        public void CopyFromBase(int baseSetIdentity, Type scheme, Func<RecordBase, bool> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        public void CopyFromBase<TTable>(int baseSetIdentity, Func<TTable, bool> where) where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="baseScheme"></param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        public void CopyFromBase(int baseSetIdentity, Type baseScheme, Type scheme, Func<RecordBase, bool> where, Action<RecordBase, RecordBase> convert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <typeparam name="TRecordBase"></typeparam>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        public void CopyFromBase<TRecordBase, TTable>(int baseSetIdentity, Func<TRecordBase, bool> where, Action<TRecordBase, TTable> convert) where TRecordBase : RecordBase where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="ignoreException"></param>
        public void CommitToBase(int baseSetIdentity, Type scheme, Func<RecordBase, bool> where, bool ignoreException = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="ignoreException"></param>
        public void CommitToBase<TTable>(int baseSetIdentity, Func<TTable, bool> where, bool ignoreException = false) where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="baseScheme"></param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="ignoreException"></param>
        /// <param name="convert"></param>
        public void CommitToBase(int baseSetIdentity, Type baseScheme, Type scheme, Func<RecordBase, bool> where, bool ignoreException, Action<RecordBase, RecordBase> convert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TTable">表类型</typeparam>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="ignoreException"></param>
        /// <param name="convert"></param>
        public void CommitToBase<TBase, TTable>(int baseSetIdentity, Func<TTable, bool> where, bool ignoreException, Action<TBase, TTable> convert) where TBase : RecordBase where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// /询问提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <returns></returns>
        public bool QueryToBase(int baseSetIdentity, Type scheme, Func<RecordBase, bool> where)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// /询问提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <returns></returns>
        public bool QueryToBase<TTable>(int baseSetIdentity, Func<TTable, bool> where) where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 询问提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="baseScheme"></param>
        /// <param name="scheme">表结构</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public bool QueryToBase(int baseSetIdentity, Type baseScheme, Type scheme, Func<RecordBase, bool> where, Action<RecordBase, RecordBase> convert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 询问提交
        /// </summary>
        /// <param name="baseSetIdentity">数据库ID</param>
        /// <param name="where">指定的查询条件</param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public bool QueryToBase<TBase, TTable>(int baseSetIdentity, Func<TTable, bool> where, Action<TBase, TTable> convert) where TBase : RecordBase where TTable : RecordBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清理引用。
        /// </summary>
        public void Clear()
        {
            foreach (var scheme in Schemes)
            {
                Free(scheme);
            }
        }
    }
}