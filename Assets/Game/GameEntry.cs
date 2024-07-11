
using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Repository;
using UnityEngine;

namespace Game
{
    public class GameEntry : MonoBehaviour
    {
        public static InitModule InitModule { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            await UniTask.DelayFrame(1);

            InitModule = await ModuleRepository.Install<InitModule>(nameof(InitModule));
            
            Debug.Log(InitModule.Tables.TbItem.Get(10002).ToString());
        }

        private void Update()
        {
            TimeInfo.Instance.Update();
        }

        private void OnApplicationQuit()
        {
            ModuleRepository.UninstallAll();
        }
    }
}

