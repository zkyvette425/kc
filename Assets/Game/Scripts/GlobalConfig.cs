using UnityEngine;
using YooAsset;

namespace KC
{
    public enum BuildType
    {
        Debug,
        Release,
    }

    [CreateAssetMenu(menuName = "Game/CreateGlobalConfig", fileName = "GlobalConfig", order = 1)]
    public class GlobalConfig : ScriptableObject
    {
        public BuildType BuildType;

        public EPlayMode EPlayMode;
    }
}