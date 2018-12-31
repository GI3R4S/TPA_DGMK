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
            ParameterExpression parametersOfData = Expression.Parameter(typeof(object), "data");
            BlockExpression blockExpression = Expression.Block(Expression.Convert(Expression.Convert(parametersOfData, data.GetType()), Type));
            Delegate Run = Expression.Lambda(blockExpression, parametersOfData).Compile();
            object ret = Run.DynamicInvoke(data);
            return ret;
        }

        public static IList ConvertList(Type type, IList source)
        {
            var typeOfList = typeof(List<>);
            Type[] typeArguments = { type };
            var genericArgumentsListType = typeOfList.MakeGenericType(typeArguments);
            var listOfTypes = (IList)Activator.CreateInstance(genericArgumentsListType);
            foreach (var item in source)
            {
                listOfTypes.Add(type.Cast(item));
            }
            return listOfTypes;
        }
    }
}
