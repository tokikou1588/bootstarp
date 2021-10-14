﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// Display 组件
    /// </summary>
    public partial class Display<TValue>
    {
        /// <summary>
        /// 获得 显示文本
        /// </summary>
        protected string? CurrentTextAsString { get; set; }

        /// <summary>
        /// 获得/设置 异步格式化字符串
        /// </summary>
        [Parameter]
        public Func<TValue, Task<string>>? FormatterAsync { get; set; }

        /// <summary>
        /// 获得/设置 格式化字符串 如时间类型设置 yyyy-MM-dd
        /// </summary>
        [Parameter]
        public string? FormatString { get; set; }

        /// <summary>
        /// 获得/设置 数据集用于 CheckboxList Select 组件 通过 Value 显示 Text 使用 默认 null
        /// </summary>
        [Parameter]
        public IEnumerable<SelectedItem>? Data { get; set; }

        /// <summary>
        /// OnParametersSetAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            CurrentTextAsString = await FormatTextAsString(Value);
        }

        /// <summary>
        /// 数值格式化委托方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual async Task<string?> FormatTextAsString(TValue value) => FormatterAsync != null
            ? await FormatterAsync(value)
            : (!string.IsNullOrEmpty(FormatString) && value != null
                ? Utility.Format(value, FormatString)
                : value == null
                    ? FormatValueString()
                    : FormatText(value));

        private string FormatText(TValue value)
        {
            string ret;
            var type = typeof(TValue);
            if (type.IsEnum())
            {
                ret = type.ToEnumDisplayName(value!.ToString());
            }
            else if (type.IsArray)
            {
                ret = ConvertArrayToString(value);
            }
            else if (type.IsGenericType && type.IsAssignableTo(typeof(IEnumerable)))
            {
                // 泛型集合 IEnumerable<TValue>
                ret = ConvertEnumerableToString(value);
            }
            else
            {
                ret = FormatValueString();
            }
            return ret;
        }

        private string FormatValueString()
        {
            string? ret = null;

            // 检查 数据源
            var valueString = Value?.ToString();
            if (Data != null)
            {
                ret = Data.FirstOrDefault(i => i.Value.Equals(valueString ?? "", StringComparison.OrdinalIgnoreCase))?.Text;
            }
            return ret ?? valueString ?? string.Empty;
        }

        private static Func<TValue, string>? _converterArray;
        /// <summary>
        /// 获取属性方法 Lambda 表达式
        /// </summary>
        /// <returns></returns>
        private static string ConvertArrayToString(TValue value)
        {
            return (_converterArray ??= ConvertArrayToStringLambda())(value);

            static Func<TValue, string> ConvertArrayToStringLambda()
            {
                Func<TValue, string> ret = _ => "";
                var param_p1 = Expression.Parameter(typeof(Array));
                var target_type = typeof(TValue).UnderlyingSystemType;
                var methodType = Type.GetType(target_type.FullName!.Replace("[]", ""));
                if (methodType != null)
                {
                    var method = typeof(string).GetMethods().Where(m => m.Name == "Join" && m.IsGenericMethod && m.GetParameters()[0].ParameterType == typeof(string)).FirstOrDefault()?.MakeGenericMethod(methodType);
                    if (method != null)
                    {
                        var body = Expression.Call(method, Expression.Constant(","), Expression.Convert(param_p1, target_type));
                        ret = Expression.Lambda<Func<TValue, string>>(body, param_p1).Compile();
                    }
                }
                return ret;
            }
        }

        private static Func<TValue, string>? _convertEnumerableToString;
        private static Func<TValue, IEnumerable<string>>? _convertToEnumerableString;
        /// <summary>
        /// 获取属性方法 Lambda 表达式
        /// </summary>
        /// <returns></returns>
        private string ConvertEnumerableToString(TValue value)
        {
            return Data == null
                ? (_convertEnumerableToString ??= ConvertEnumerableToStringLambda())(value)
                : GetTextByValue((_convertToEnumerableString ??= ConvertToEnumerableStringLambda())(value));

            static Func<TValue, string> ConvertEnumerableToStringLambda()
            {
                Func<TValue, string> ret = _ => "";
                var typeArguments = typeof(TValue).GenericTypeArguments;
                var param_p1 = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(typeArguments));
                var method = typeof(string).GetMethods().Where(m => m.Name == "Join" && m.IsGenericMethod && m.GetParameters()[0].ParameterType == typeof(string)).FirstOrDefault()?.MakeGenericMethod(typeArguments);
                if (method != null)
                {
                    var body = Expression.Call(method, Expression.Constant(","), param_p1);
                    ret = Expression.Lambda<Func<TValue, string>>(body, param_p1).Compile();
                }
                return ret;
            }

            static Func<TValue, IEnumerable<string>> ConvertToEnumerableStringLambda()
            {
                Func<TValue, IEnumerable<string>> ret = _ => Enumerable.Empty<string>();
                var typeArguments = typeof(TValue).GenericTypeArguments;
                var param_p1 = Expression.Parameter(typeof(IEnumerable<>).MakeGenericType(typeArguments));

                var method = typeof(Display<>).MakeGenericType(typeof(TValue))
                    .GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Static)?
                    .MakeGenericMethod(typeArguments);
                if (method != null)
                {
                    var body = Expression.Call(method, param_p1);
                    ret = Expression.Lambda<Func<TValue, IEnumerable<string>>>(body, param_p1).Compile();
                }
                return ret;
            }
        }

        private static IEnumerable<string?> Cast<TType>(IEnumerable<TType> source) => source.Select(i => i?.ToString());

        private string GetTextByValue(IEnumerable<string> source) => Data == null
            ? ""
            : string.Join(",", source.Aggregate(new List<string>(), (s, i) =>
            {
                var text = Data.FirstOrDefault(d => d.Value.Equals(i, StringComparison.OrdinalIgnoreCase))?.Text;
                if (text != null)
                {
                    s.Add(text);
                }
                return s;
            }));
    }
}
