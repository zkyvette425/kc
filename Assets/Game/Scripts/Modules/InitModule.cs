// using cfg;
// using Cysharp.Threading.Tasks;
// using Game.Scripts.Core.Repository;
// using SimpleJSON;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
//
// namespace Game
// {
//     public class InitModule : ModuleBase
//     {
//         /// <summary>
//         /// 获取优先级(优先级数字越高,则优先级越低,销毁越靠前)
//         /// </summary>
//         public override int Priority { get; protected set; } = 2;
//
//         public static Tables Tables { get; private set; }
//
//         /// <summary>
//         /// 模块初始化
//         /// </summary>
//         /// <param name="args">初始化参数集</param>
//         /// <returns></returns>
//         protected override UniTask OnInit(params object[] args)
//         {
//             Tables = new Tables(t =>
//             {
//                 var handler = Addressables.LoadAssetAsync<TextAsset>(t);
//                 handler.WaitForCompletion();
//                 return JSONNode.Parse(handler.Result.text);
//             });
//
//             
//             
//             return UniTask.CompletedTask;
//         }
//     }
// }