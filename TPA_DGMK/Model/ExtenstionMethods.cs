using System;
using System.Reflection;

namespace Model
{
    public static class ExtensionMethods
    {
        public static bool GetVisible(this Type type)
        {
            return type.IsPublic || type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamANDAssem;
        }
        public static bool GetVisible(this MethodBase method)
        {
            return method != null && (method.IsPublic || method.IsFamily || method.IsFamilyAndAssembly);
        }
        public static bool GetVisible(this FieldInfo field)
        {
            return field != null && (field.IsPublic || field.IsFamily || field.IsFamilyAndAssembly);
        }
        public static string GetNamespace(this Type type)
        {
            string ns = type.Namespace;
            return ns ?? string.Empty;
        }
    }
}