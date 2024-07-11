using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Framework.core;
using Framework.core.ReferencePool;
using Game.Scripts.Core.Repository.External;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 表记录基类
    /// </summary>
    public abstract class RecordBase : IReference
    {
        protected int _setId;
        protected RecordState _recordState;
        protected Dictionary<string, object> _caches;
        

        /// <summary>
        /// 设置(程序集内)或获取表记录状态
        /// </summary>
        protected internal RecordState RecordState
        {
            get => _recordState;
            internal set => _recordState = value;
        }

        /// <summary>
        /// 获取所在数据库ID
        /// </summary>
        public int SetId
        {
            get => _setId;
            internal set => _setId = value;
        }

        /// <summary>
        /// 获取所在数据库
        /// </summary>
        public ISet Set => Repository.GetSet(_setId);

        /// <summary>
        /// 清理引用。(在删除后执行)
        /// </summary>
        public virtual void Clear()
        {
            _setId = 0;
            _recordState = RecordState.Clean;
            _caches.Clear();
        }

        /// <summary>
        /// 构造表记录
        /// </summary>
        protected internal virtual void Ctor()
        {
            _caches = new Dictionary<string, object>();
        }

        /// <summary>
        /// 删除时触发
        /// </summary>
        protected internal virtual void OnDeleted()
        {
            
        }

        /// <summary>
        /// 打包为Json数据
        /// </summary>
        /// <param name="extraObjects">额外数据</param>
        /// <returns></returns>
        protected internal virtual UniTask<object> PackageToJson(params object[] extraObjects)
        {
            return default;
        }

        /// <summary>
        /// 使用Json数据填充
        /// </summary>
        /// <param name="json">json数据</param>
        /// <param name="arg1">参数</param>
        /// <returns></returns>
        protected internal virtual UniTask<bool> ParseFromJson<TJson, TArg>(TJson json, TArg arg1)
        {
            _recordState = RecordState.Clean;
            return UniTask.FromResult(true);
        }
        
        /// <summary>
        /// 回滚属性
        /// </summary>
        protected internal void RollbackProperties()
        {
            var properties = new Dictionary<string, object>(_caches);
            foreach (var key in _caches.Keys)
            {
                var property = GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    throw new FrameworkException($"回滚属性失败,属性:{key}不是公开属性");
                }
                if (property.GetSetMethod() != null)
                {
                    property.SetValue(this,properties[key]);
                }
            }
            _recordState = RecordState.Clean;
            _caches.Clear();
        }

        /// <summary>
        /// 清空属性缓存
        /// </summary>
        protected internal void ClearPropertyCaches()
        {
            _caches.Clear();
        }
    }
}