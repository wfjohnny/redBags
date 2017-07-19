using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.IoC
{
    /// <summary>
    /// 表示用于整个IoC系统的工具类。
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// autofac参数组合
        /// </summary>
        /// <param name="overridedArguments"></param>
        /// <returns></returns>
        public static IEnumerable<Parameter> GetParameter(object overridedArguments)
        {
            List<NamedParameter> overrides = new List<NamedParameter>();

            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new NamedParameter(propertyName, propertyValue));
                });

            return overrides;
        }
    }
}

