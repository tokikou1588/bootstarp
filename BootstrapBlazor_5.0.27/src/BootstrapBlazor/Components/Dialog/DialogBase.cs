﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DialogBase<TModel> : ComponentBase
    {
        /// <summary>
        /// 获得/设置 EditModel 实例
        /// </summary>
        [Parameter]
        [NotNull]
        public TModel? Model { get; set; }

        /// <summary>
        /// 获得/设置 BodyTemplate 实例
        /// </summary>
        [Parameter]
        public RenderFragment<TModel>? BodyTemplate { get; set; }

        /// <summary>
        /// 获得 数据项集合
        /// </summary>
        [Parameter]
        public IEnumerable<IEditorItem>? Items { get; set; }

        /// <summary>
        /// 获得/设置 是否显示标签
        /// </summary>
        [Parameter]
        public bool ShowLabel { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Model == null)
            {
                throw new InvalidOperationException("Model value not set to null");
            }
        }
    }
}
