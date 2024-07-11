using System.Collections.Generic;
using System.Linq;
using Framework.core;
using UnityEngine;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace Game.Scripts.Core.Repository
{
    /// <summary>
    /// 模块仓储
    /// </summary>
    public static class ModuleRepository
    {
        private static Dictionary<int, ModuleBase> _modules;
        
        
        /// <summary>
        /// 构建模块仓储
        /// </summary>
        static ModuleRepository()
        {
            _modules = new Dictionary<int, ModuleBase>();
        }

        /// <summary>
        /// 是否存在模块
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static bool IsExist(int moduleId)
        {
            return _modules.ContainsKey(moduleId);
        }
        
        /// <summary>
        /// 是否存在模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static bool IsExist(string moduleName)
        {
            return _modules.Values.Any(p => p.Name == moduleName);   
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static ModuleBase GetModule(int moduleId)
        {
            return !IsExist(moduleId) ? null : _modules[moduleId];
        }
        
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns></returns>
        public static TModule GetModule<TModule>(int moduleId) where TModule : ModuleBase
        {
            return !IsExist(moduleId) ? null : (TModule)_modules[moduleId];
        }
        
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static ModuleBase GetModule(string moduleName)
        {
            return _modules.Values.FirstOrDefault(module => module.Name == moduleName);
        }
        
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns></returns>
        public static TModule GetModule<TModule>(string moduleName) where TModule : ModuleBase
        {
            return (TModule)GetModule(moduleName);
        }

        /// <summary>
        /// 获取子模块集
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        internal static List<ModuleBase> GetChildModules(int moduleId)
        {
            return _modules.Values.Where(p => p.ParentId == moduleId).ToList();
        }

        /// <summary>
        /// 获取全部后裔模块集(所有子孙模块)
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        internal static List<ModuleBase> GetDescendants(int moduleId)
        {
            return _modules.Values.Where(p => p.RootId == moduleId && p.Id != moduleId).ToList();
        }

        /// <summary>
        /// 安装新模块
        /// </summary>
        /// <param name="name">模块名称</param>
        /// <param name="args">模块初始化参数</param>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns></returns>
        public static async Cysharp.Threading.Tasks.UniTask<TModule> Install<TModule>(string name,params object[] args) where TModule : ModuleBase,new()
        {
            TModule module = Framework.core.ReferencePool.ReferencePool.Acquire<TModule>();
            module.Id = module.GetHashCode();
            module.Name = name;
            if (IsExist(module.Id))
            {
                throw new FrameworkException($"创建模块:{name} - {module.Name}失败,原因:模块已经存在");
            }
            _modules[module.Id] = module;
            await module.Init(args);
            return module;
        }

        /// <summary>
        /// 卸载模块
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        public static UniTask Uninstall(int moduleId)
        {
            var module = GetModule(moduleId);
            return Uninstall(module);
        }
        
        /// <summary>
        /// 卸载模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        public static UniTask Uninstall(string moduleName)
        {
            var module = GetModule(moduleName);
            return Uninstall(module);
        }
        
        /// <summary>
        /// 卸载模块
        /// </summary>
        /// <param name="module">模块</param>
        public static async UniTask Uninstall(ModuleBase module)
        {
            if (module == null)
            {
                return;
            }
            int moduleId = module.Id;
            await module.Destroy();
            Framework.core.ReferencePool.ReferencePool.Release(module);
            _modules.Remove(moduleId);
        }
        
        /// <summary>
        /// 卸载所有模块
        /// </summary>
        public static async UniTask UninstallAll()
        {
            async UniTask SingleDelete(ModuleBase module)
            {
                await module.Destroy();
                Framework.core.ReferencePool.ReferencePool.Release(module);
            }

            var modules = _modules.Values.OrderByDescending(p => p.Priority);
            await UniTask.WhenAll(modules.Select(SingleDelete));
            _modules.Clear();
            Debug.Log("所有模块卸载完成");
        }

        /// <summary>
        /// 设置父模块
        /// </summary>
        /// <param name="module">待设置模块</param>
        /// <param name="parentModuleId">父模块ID</param>
        public static void SetParent(this ModuleBase module, int parentModuleId)
        {
            if (!IsExist(parentModuleId))
            {
                throw new FrameworkException($"模块:{module.Name} - {module.Id}设置父模块失败,原因:父模块:{parentModuleId}不存在");
            }
            module.ParentId = parentModuleId;
        }
        
        /// <summary>
        /// 设置父模块
        /// </summary>
        /// <param name="module">待设置模块</param>
        /// <param name="parentModuleName">父模块名称</param>
        public static void SetParent(this ModuleBase module, string parentModuleName)
        {
            var parent = GetModule(parentModuleName);
            if (parent == null)
            {
                throw new FrameworkException($"模块:{module.Name} - {module.Id}设置父模块失败,原因:父模块:{parentModuleName}不存在");
            }
            module.ParentId = parent.Id;
        }
        
        /// <summary>
        /// 设置父模块
        /// </summary>
        /// <param name="module">待设置模块</param>
        /// <param name="parentModule">父模块</param>
        public static void SetParent(this ModuleBase module, ModuleBase parentModule)
        {
            if (parentModule == null)
            {
                throw new FrameworkException($"模块:{module.Name} - {module.Id}设置父模块失败,原因:指定的父模块不存在");
            }
            module.ParentId = parentModule.Id;
        }
        
    }
}