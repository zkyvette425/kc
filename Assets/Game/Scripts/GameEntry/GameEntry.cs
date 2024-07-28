using System;
using KC;
using UnityEngine;
using UnityEngine.Profiling;
using Component = KC.Component;

namespace Game
{
    public class Test1Component : Component, IAwake<int>
    {
        public ComponentRef<Test1Component> test1;

        public int Number;
        
        public void Awake(int a)
        {
            Number = a;
        }
    }

    public class Test2Component : Component, IAwake<string>
    {
        public void Awake(string a)
        {
        }
    }
    
    public class Test3Component : Component, IAwake<bool>
    {
        public void Awake(bool a)
        {
        }
    }

    public class TestModule : Module
    {
        public TestModule()
        {
            Debug.Log($"创建Module:{Id}");
        }
    }
    
    public class GameEntry : MonoBehaviour
    {
        // public static InitModule InitModule { get; private set; }
        //
        // private void Awake()
        // {
        //     DontDestroyOnLoad(this);
        // }
        //
        // private async void Start()
        // {
        //     await UniTask.DelayFrame(1);
        //
        //     InitModule = await ModuleRepository.Install<InitModule>(nameof(InitModule));
        //     
        //     Debug.Log(InitModule.Tables.TbItem.Get(10002).ToString());
        // }
        //
        // private void Update()
        // {
        //     TimeInfo.Instance.Update();
        // }
        //
        // private void OnApplicationQuit()
        // {
        //     ModuleRepository.UninstallAll();
        // }

        private void Start()
        {
            TestModule testModule = new TestModule();
            var test1  =testModule.AddComponent<Test1Component, int>(666);
            var test2 = testModule.AddComponent<Test1Component, int>(777);
            test1.test1 = test2;
            Test1Component t = test1.test1;
            t.Number = 999;
            Debug.Log(t.Number +"...." + test2.Number);
            testModule.RemoveComponent(test2);
            
            
            for (int i = 0; i < 100000; i++)
            {
                test1.AddComponent<Test1Component, int>(123);
                if (i == 50000)
                {
                    test1.AddComponentWithId<Test2Component, string>(6, "test2");
                }
                if (i == 70000)
                {
                    test1.AddComponentWithId<Test2Component, string>(8, "test2");
                }
            }
           // test1.AddComponent<Test1Component, int>(123);
           // test1.AddComponentWithId<Test2Component, string>(6, "test2");
            
            Profiler.BeginSample("aaaaaaaaaa");
            var a = test1.GetComponent<Test2Component>();
            Profiler.EndSample();
            Debug.LogError(a.Id);
        }

        private void Update()
        {
            
        }
    }
}

