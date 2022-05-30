namespace SK.Framework
{
    /// <summary>
    /// 过滤类型
    /// </summary>
    public enum GameObjectFilterMode
    {
        Name,       //根据名称过滤
        Component,  //根据组件过滤
        Layer,      //根据层级过滤
        Tag,        //根据标签过滤
        Active,     //根据物体是否显示过滤
        Missing,    //根据丢失材质、脚本等过滤
    }

    /// <summary>
    /// 丢失类型
    /// </summary>
    public enum MissingMode
    {
        Material,   //材质丢失
        Mesh,       //网格丢失
        Script      //脚本丢失
    }
}