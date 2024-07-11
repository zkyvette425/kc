using Game.Scripts.Core.Repository;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public abstract class AddressableAssetComponent<TObject> : RecordBase
    {
        protected AsyncOperationHandle<TObject> Handle;
        
        protected string _id { get; private set; }
        
    }
}

