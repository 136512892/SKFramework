using System;
using System.Reflection;

namespace SK.Framework
{
    public static class ReflectionExtension 
    {
        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="self">实例</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>字段值</returns>
        public static object GetFieldValue(this object self, string fieldName)
        {
            Type type = self.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName);
            return fieldInfo?.GetValue(self);
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="self">实例</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="index"></param>
        /// <returns>属性值</returns>
        public static object GetPropertyValue(this object self, string propertyName, object[] index = null)
        {
            Type type = self.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            return propertyInfo?.GetValue(self, index);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="self">实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object ExecuteMethod(this object self, string methodName, params object[] args)
        {
            Type type = self.GetType();
            MethodInfo methodInfo = type.GetMethod(methodName);
            return methodInfo?.Invoke(self, args);
        } 
    }
}