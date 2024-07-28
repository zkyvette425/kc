using System;

namespace KC
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(CreateInstance, isThreadSafe: true);

        public static T Instance => _instance.Value;

        protected Singleton()
        {
            
        }
        
        private static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}