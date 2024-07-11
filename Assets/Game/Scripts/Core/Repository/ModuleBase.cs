using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework.core;
using Framework.core.ReferencePool;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class ModuleBase: IReference
    {
        protected int _id;
        protected int _parentId;
        protected ModelSetAgent _setAgent;
        protected bool _isCheckLoading;        //是否开启加载检查,如果开启,则加载各环节中一个环节返回false,就不会进入后续环节
        protected int _priority;

        /// <summary>
        /// 设置或获取模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取模块ID
        /// </summary>
        public int Id 
        {
            get => _id;
            internal set => _id = value;
        }

        /// <summary>
        /// 获取父模块ID
        /// </summary>
        public int ParentId
        {
            get => _parentId;
            internal set => _parentId = value;
        }

        /// <summary>
        /// 获取根模块ID
        /// </summary>
        public int RootId => Root.Id;

        /// <summary>
        /// 获取父模块
        /// </summary>
        public ModuleBase Parent => ModuleRepository.GetModule(_parentId);

        /// <summary>
        /// 获取根模块
        /// </summary>
        public ModuleBase Root => Parent != null ? Parent.Root : this;

        /// <summary>
        /// 获取子模块集
        /// </summary>
        public List<ModuleBase> Children => ModuleRepository.GetChildModules(_id);

        /// <summary>
        /// 获取全部后裔模块集(所有子孙模块)
        /// </summary>
        public List<ModuleBase> Descendants => ModuleRepository.GetDescendants(_id);

        /// <summary>
        /// 获取数据库代理
        /// </summary>
        public ModelSetAgent SetAgent => _setAgent;

        /// <summary>
        /// 获取优先级(优先级数字越高,则优先级越低,销毁越靠前)
        /// </summary>
        public abstract int Priority { get; protected set; }

        /// <summary>
        /// 模块初始化时调用
        /// </summary>
        /// <param name="args">初始化参数集</param>
        /// <returns></returns>
        internal virtual async UniTask Init(params object[] args)
        {
            try
            {
                _setAgent = Framework.core.ReferencePool.ReferencePool.Acquire<ModelSetAgent>();
                _setAgent.Ctor(_id);
                await OnInit(args);
                if (_isCheckLoading)
                {
                    var isOk = await OnPreparingLoad();
                    if (!isOk)
                    {
                        return;
                    }
                    isOk = await OnLoading();
                    if (!isOk)
                    {
                        return;
                    }
                    await OnLoadCompleted();
                }
                else
                {
                    await OnPreparingLoad();
                    await OnLoading();
                    await OnLoadCompleted();
                }
            }
            catch (Exception e)
            {
                throw new FrameworkException($"模块:{Name} - {_id} 执行异常,原因:{e}");
            }
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="args">初始化参数集</param>
        /// <returns></returns>
        protected virtual UniTask OnInit(params object[] args)
        {
            return UniTask.FromResult(true);
        }

        /// <summary>
        /// 模块准备加载
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask<bool> OnPreparingLoad()
        {
            return UniTask.FromResult(true);
        }

        /// <summary>
        /// 模块加载中
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask<bool> OnLoading()
        {
            return UniTask.FromResult(true);
        }

        /// <summary>
        /// 模块加载完成
        /// </summary>
        /// <returns></returns>
        protected virtual UniTask OnLoadCompleted()
        {
            return UniTask.FromResult(true);
        }

        protected virtual void OnDestroyed()
        {
        }

        /// <summary>
        /// 销毁模块
        /// </summary>
        /// <returns></returns>
        internal async UniTask Destroy()
        {
            await UniTask.WhenAll(Children.Select(p => p.Destroy()));
            Framework.core.ReferencePool.ReferencePool.Release(_setAgent);
            OnDestroyed();
        }
            
        /// <summary>
        /// 清理引用。
        /// </summary>
        public void Clear()
        {
            _id = 0;
            _parentId = 0;
            _setAgent = null;
            Name = null;
        }
    }
}