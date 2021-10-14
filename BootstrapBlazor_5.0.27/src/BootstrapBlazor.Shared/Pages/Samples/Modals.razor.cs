﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Shared.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Modals
    {
        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? Modal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? BackdropModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? SmailModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? LargeModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? ExtraLargeModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? CenterModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? LongContentModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        private Modal? ScrollModal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AttributeItem> GetAttributes()
        {
            return new AttributeItem[]
            {
                new AttributeItem() {
                    Name = "BodyTemplate",
                    Description = "模态主体 ModalBody 组件",
                    Type = "RenderFragment",
                    ValueList = " — ",
                    DefaultValue = " — "
                },
                new AttributeItem() {
                    Name = "ChildContent",
                    Description = "内容",
                    Type = "RenderFragment",
                    ValueList = " — ",
                    DefaultValue = " — "
                },
                new AttributeItem() {
                    Name = "FooterTemplate",
                    Description = "模态底部 ModalFooter 组件",
                    Type = "RenderFragment",
                    ValueList = " — ",
                    DefaultValue = " — "
                },
                new AttributeItem() {
                    Name = "IsBackdrop",
                    Description = "是否后台关闭弹窗",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "false"
                },
                new AttributeItem() {
                    Name = "IsCentered",
                    Description = "是否垂直居中",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "true"
                },
                new AttributeItem() {
                    Name = "IsScrolling",
                    Description = "是否弹窗正文超长时滚动",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "false"
                },
                new AttributeItem() {
                    Name = "IsFade",
                    Description = "是否开启淡入淡出动画效果",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "true"
                },
                new AttributeItem() {
                    Name = "IsDraggable",
                    Description = "是否开启可拖拽效果",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "false"
                },
                new AttributeItem() {
                    Name = "ShowCloseButton",
                    Description = "是否显示关闭按钮",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "true"
                },
                new AttributeItem() {
                    Name = "ShowFooter",
                    Description = "是否显示 Footer",
                    Type = "boolean",
                    ValueList = " — ",
                    DefaultValue = "true"
                },
                new AttributeItem() {
                    Name = "Size",
                    Description = "尺寸",
                    Type = "Size",
                    ValueList = "None / ExtraSmall / Small / Medium / Large / ExtraLarge",
                    DefaultValue = "Large"
                },
                new AttributeItem() {
                    Name = "Title",
                    Description = "弹窗标题",
                    Type = "string",
                    ValueList = " — ",
                    DefaultValue = " 未设置 "
                },
            };
        }
    }
}
