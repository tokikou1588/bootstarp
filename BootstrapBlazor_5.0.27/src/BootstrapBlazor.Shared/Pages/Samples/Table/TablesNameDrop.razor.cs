﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Pages.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Shared.Pages.Table
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TablesNameDrop
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public Foo? Model { get; set; }

        private readonly List<SelectedItem> items = new()
        {
            new SelectedItem { Text = "自定义姓名1", Value = "自定义姓名1" },
            new SelectedItem { Text = "自定义姓名2", Value = "自定义姓名2" },
            new SelectedItem { Text = "自定义姓名3", Value = "自定义姓名3" },
            new SelectedItem { Text = "自定义姓名4", Value = "自定义姓名4" },
        };
    }
}
