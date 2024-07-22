using Game.Scripts.Core.Repository.Attribute;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
    public sealed class AddressableGameObjectComponent : AddressableAssetComponent<GameObject>
    {
        [PrimaryKey]
        public string Id => _id;
        public GameObject Asset => Handle.Result;
    }

    public sealed class AddressableSpriteAtlasComponent : AddressableAssetComponent<SpriteAtlas>
    {
        [PrimaryKey]
        public string Id => _id;
        
        public SpriteAtlas Asset => Handle.Result;
    }
    
    public sealed class AddressableAudioClipComponent : AddressableAssetComponent<AudioClip>
    {
        [PrimaryKey]
        public string Id => _id;
        
        public AudioClip Asset => Handle.Result;
    }
    
    public sealed class AddressableTransformComponent : AddressableAssetComponent<GameObject>
    {
        [PrimaryKey]
        public string Id => _id;

        public Transform Asset => Handle.Result.transform;
    }
    
    public sealed class AddressableTextAssetComponent : AddressableAssetComponent<TextAsset>
    {
        [PrimaryKey]
        public string Id => _id;

        public string Asset => Handle.Result.text;
    }

    public sealed class AddressableAnimatorComponent : AddressableAssetComponent<GameObject>
    {
        private Animator _animator;
        
        [PrimaryKey]
        public string Id => _id;

        public Animator Asset
        {
            get
            {
                if (_animator == null)
                {
                    _animator = Handle.Result.GetComponent<Animator>();
                }
                return _animator;
            }
        }

        /// <summary>
        /// 清理引用。(在删除后执行)
        /// </summary>
        public override void Clear()
        {
            _animator = null;
            base.Clear();
        }
    }
}