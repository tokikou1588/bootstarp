﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.Generic;

namespace BootstrapBlazor.Shared.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Navs
    {
        private IEnumerable<NavLink> Items => GetItems();

        private IEnumerable<NavLink> GetItems()
        {
            var ret = new List<NavLink>();
            var link = new NavLink();
            link.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>()
            {
                ["href"] = WebsiteOption.Value.AdminUrl,
                ["class"] = "nav-link nav-item",
                ["target"] = "_blank",
                ["ChildContent"] = new RenderFragment(builder =>
                {
                    builder.AddContent(0, "BootstrapAdmin");
                })
            }));
            ret.Add(link);
            return ret;
        }

        /// <summary>
        /// 获得属性方法
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
        {
            // TODO: 移动到数据库中
            new AttributeItem() {
                Name = "ChildContent",
                Description = "内容",
                Type = "RenderFragment",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = "Alignment",
                Description = "组件对齐方式",
                Type = "Alignment",
                ValueList = "Left|Center|Right",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = "IsVertical",
                Description = "垂直分布",
                Type = "bool",
                ValueList = "true|false",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsPills",
                Description = "胶囊",
                Type = "bool",
                ValueList = "true|false",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsFill",
                Description = "填充",
                Type = "bool",
                ValueList = "true|false",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsJustified",
                Description = "等宽",
                Type = "bool",
                ValueList = "true|false",
                DefaultValue = "false"
            }
        };
    }
}
