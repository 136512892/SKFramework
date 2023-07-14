using SK.Framework.Refer;

namespace SK.Framework.Resource
{
    /// <summary>
    /// 资产引用
    /// </summary>
    public class ResourceReference : ReferBase
    {
        protected override void OnNotZero()
        {

        }

        protected override void OnZero()
        {
            //当该资产的引用数量变为0时 卸载该资产
            Main.Resource.UnloadAsset(Name, true);
        }
    }
}