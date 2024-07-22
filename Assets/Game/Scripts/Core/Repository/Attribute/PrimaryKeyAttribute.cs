using System;

namespace Game.Scripts.Core.Repository.Attribute
{
    /// <summary>
    /// 主键标记,不支持联合主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : System.Attribute
    {
        
    }
}