using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessLogic.Mapping
{
    public static class ConvertionUtilities
    {
        public static object Cast(this Type Type, object data)
        {
            ParameterExpression DataParam = Expression.Parameter(typeof(object), "data");
            BlockExpression Body = Expression.Block(Expression.Convert(Expression.Convert(DataParam, data.GetType()), Type));

            Delegate Run = Expression.Lambda(Body, DataParam).Compile();
            object ret = Run.DynamicInvoke(data);
            return ret;
        }

        public static IList ConvertList(Type type, IList source)
        {
            var listType = typeof(List<>);
            Type[] typeArgs = { type };
            var genericListType = listType.MakeGenericType(typeArgs);
            var typedList = (IList)Activator.CreateInstance(genericListType);
            foreach (var item in source)
            {
                typedList.Add(type.Cast(item));
            }

            return typedList;

        }
    }
}
