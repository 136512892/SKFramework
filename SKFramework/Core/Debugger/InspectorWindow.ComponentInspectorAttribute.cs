using System;

namespace SK.Framework.Debugger
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentInspectorAttribute : Attribute
    {
        public Type ComponentType { get; private set; }
        
        public ComponentInspectorAttribute(Type type)
        {
            ComponentType = type;
        }
    }
}