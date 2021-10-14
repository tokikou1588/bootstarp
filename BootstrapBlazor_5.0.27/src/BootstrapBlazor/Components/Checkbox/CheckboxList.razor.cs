﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Localization.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// CheckboxList 组件基类
    /// </summary>
    public sealed partial class CheckboxList<TValue>
    {
        /// <summary>
        /// 获得 组件样式
        /// </summary>
        private string? ClassString => CssBuilder.Default("checkbox-list form-control")
            .AddClass("no-border", !ShowBorder && ValidCss != "is-invalid")
            .AddClass(CssClass)
            .AddClass("is-invalid", IsValid.HasValue && !IsValid.Value)
            .Build();

        /// <summary>
        /// 获得 组件内部 Checkbox 项目样式
        /// </summary>
        private string? CheckboxItemClassString => CssBuilder.Default("checkbox-item")
            .AddClass(CheckboxItemClass)
            .Build();

        /// <summary>
        /// 获得/设置 数据源
        /// </summary>
        [Parameter]
        [NotNull]
        public IEnumerable<SelectedItem>? Items { get; set; }

        /// <summary>
        /// 获得/设置 Checkbox 组件布局样式
        /// </summary>
        [Parameter]
        public string? CheckboxItemClass { get; set; }

        /// <summary>
        /// 获得/设置 是否显示边框 默认为 true
        /// </summary>
        [Parameter]
        public bool ShowBorder { get; set; } = true;

        /// <summary>
        /// 获得/设置 SelectedItemChanged 方法
        /// </summary>
        [Parameter]
        public Func<IEnumerable<SelectedItem>, TValue, Task>? OnSelectedChanged { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizerFactory? LocalizerFactory { get; set; }

        [Inject]
        [NotNull]
        private IOptions<JsonLocalizationOptions>? Options { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Items == null)
            {
                Type? innerType = null;
                if (typeof(IEnumerable).IsAssignableFrom(typeof(TValue)))
                {
                    innerType = typeof(TValue).GetGenericArguments()[0];
                }
                if (innerType != null && innerType.IsEnum)
                {
                    Items = innerType.ToSelectList();
                }
                else
                {
                    Items = Enumerable.Empty<SelectedItem>();
                }
            }

            InitValue();

            // 处理 Required 标签
            if (EditContext != null && FieldIdentifier != null)
            {
                var pi = FieldIdentifier.Value.Model.GetType().GetProperty(FieldIdentifier.Value.FieldName);
                if (pi != null)
                {
                    var required = pi.GetCustomAttribute<RequiredAttribute>();
                    if (required != null)
                    {
                        Rules.Add(new RequiredValidator() { LocalizerFactory = LocalizerFactory, ErrorMessage = required.ErrorMessage, AllowEmptyString = required.AllowEmptyStrings });
                    }
                }
            }
        }

        private void InitValue()
        {
            // 通过 Value 对集合进行赋值
            if (Value != null)
            {
                var typeValue = typeof(TValue);
                IEnumerable? list = null;
                if (typeValue == typeof(string))
                {
                    var values = CurrentValueAsString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in Items)
                    {
                        item.Active = values.Any(v => v.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
                    }
                    list = values;
                }
                else if (typeValue.IsGenericType)
                {
                    var t = typeValue.GenericTypeArguments;
                    var instance = Activator.CreateInstance(typeof(List<>).MakeGenericType(t));
                    if (instance != null)
                    {
                        var mi = instance.GetType().GetMethod("AddRange");
                        if (mi != null)
                        {
                            mi.Invoke(instance, new object[] { Value });
                        }
                        list = instance as IEnumerable;
                        foreach (var item in Items)
                        {
                            item.Active = false;
                            foreach (var v in list!)
                            {
                                item.Active = item.Value.Equals(v!.ToString(), StringComparison.OrdinalIgnoreCase);
                                if (item.Active)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checkbox 组件选项状态改变时触发此方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="v"></param>
        private async Task OnStateChanged(SelectedItem item, bool v)
        {
            item.Active = v;

            var typeValue = typeof(TValue);
            if (typeValue == typeof(string))
            {
                CurrentValueAsString = string.Join(",", Items.Where(i => i.Active).Select(i => i.Value));
            }
            else if (typeValue.IsGenericType)
            {
                var t = typeValue.GenericTypeArguments;
                if (Activator.CreateInstance(typeof(List<>).MakeGenericType(t)) is IList instance)
                {
                    foreach (var sl in Items.Where(i => i.Active))
                    {
                        var val = sl.Value;
                        if (t[0].IsEnum && val != null)
                        {
                            instance.Add(Enum.Parse(t[0], val.ToString()));
                        }
                        else
                        {
                            instance.Add(Convert.ChangeType(val, t[0]));
                        }

                    }
                    CurrentValue = (TValue)instance;
                }
            }

            if (OnSelectedChanged != null)
            {
                await OnSelectedChanged.Invoke(Items, Value);
            }
        }
    }
}
