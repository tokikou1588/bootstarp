﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// Console 组件
    /// </summary>
    public abstract class ConsoleBase : BootstrapComponentBase
    {
        /// <summary>
        /// 获得/设置 组件绑定数据源
        /// </summary>
        [Parameter]
        public IEnumerable<ConsoleMessageItem> Items { get; set; } = Enumerable.Empty<ConsoleMessageItem>();

        /// <summary>
        /// 获得/设置 Header 显示文字 默认值为 系统监控
        /// </summary>
        [Parameter]
        public string? HeaderText { get; set; }

        /// <summary>
        /// 获得/设置 指示灯 Title 显示文字
        /// </summary>
        [Parameter]
        public string? LightTitle { get; set; }

        /// <summary>
        /// 获得/设置 自动滚屏显示文字
        /// </summary>
        [Parameter]
        public string? AutoScrollText { get; set; }

        /// <summary>
        /// 获得/设置 是否显示自动滚屏选项 默认 false
        /// </summary>
        [Parameter]
        public bool ShowAutoScroll { get; set; }

        /// <summary>
        /// 获得/设置 是否自动滚屏默认 true
        /// </summary>
        [Parameter]
        public bool IsAutoScroll { get; set; } = true;

        /// <summary>
        /// 获得/设置 按钮 显示文字 默认值为 清屏
        /// </summary>
        [Parameter]
        public string? ClearButtonText { get; set; }

        /// <summary>
        /// 获得/设置 按钮 显示图标 默认值为 fa-times
        /// </summary>
        [Parameter]
        public string ClearButtonIcon { get; set; } = "fa fa-times";

        /// <summary>
        /// 获得/设置 按钮 显示图标 默认值为 fa-times
        /// </summary>
        [Parameter]
        public Color ClearButtonColor { get; set; } = Color.Secondary;

        /// <summary>
        /// 获得/设置 清空委托方法
        /// </summary>
        [Parameter]
        public Action? OnClear { get; set; }

        /// <summary>
        /// 获得/设置 组件高度 默认为 126px;
        /// </summary>
        [Parameter]
        public int Height { get; set; }
    }
}
