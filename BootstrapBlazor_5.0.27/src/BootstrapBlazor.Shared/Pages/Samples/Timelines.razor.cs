﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared.Common;
using BootstrapBlazor.Shared.Pages.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BootstrapBlazor.Shared.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Timelines
    {
        private readonly ConcurrentQueue<ConsoleMessageItem> _messages = new();

        private readonly CancellationTokenSource _cancelTokenSource = new();

        private readonly AutoResetEvent _locker = new(true);

        private IEnumerable<ConsoleMessageItem> Messages => _messages;

        private bool IsReverse { get; set; }

        private static Color GetColor()
        {
            var second = DateTime.Now.Second;
            return (second % 3) switch
            {
                1 => Color.Danger,
                2 => Color.Info,
                _ => Color.None
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var _ = Task.Run(async () =>
            {
                do
                {
                    _locker.WaitOne();
                    _messages.Enqueue(new ConsoleMessageItem { Message = $"{DateTimeOffset.Now}: Dispatch Message", Color = GetColor() });

                    if (_messages.Count > 8)
                    {
                        _messages.TryDequeue(out var _);
                    }
                    await InvokeAsync(StateHasChanged);
                    _locker.Set();
                    await Task.Delay(2000, _cancelTokenSource.Token);
                }
                while (!_cancelTokenSource.IsCancellationRequested);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="value"></param>

        public Task OnStateChanged(CheckboxState state, SelectedItem value)
        {
            IsReverse = value.Text == "倒序";
            StateHasChanged();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<SelectedItem> Items { get; set; } = new SelectedItem[]
        {
            new SelectedItem("1","正序"){ Active=true },
            new SelectedItem("2","倒序")
        };

        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumerable<TimelineItem> TimelineItems = new TimelineItem[]
        {
            new TimelineItem {
                Content = "创建时间",
                Description = DateTime.Now.ToString("yyyy-MM-dd")
            },
            new TimelineItem{
                Content = "通过审核",
                Description = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")
            },
            new TimelineItem
            {
                Content = "活动按期开始",
                Description = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")
            }
        };

        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumerable<TimelineItem> CustomerTimelineItems = new TimelineItem[]
        {
            new TimelineItem
            {
                Content = "默认样式的节点",
                Description = DateTime.Now.ToString("yyyy-MM-dd")
            },
            new TimelineItem
            {
                Color = Color.Success,
                Content = "支持自定义颜色",
                Description = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")
            },
            new TimelineItem
            {
                Color = Color.Danger,
                Icon = "fa fa-fw fa-fa",
                Content = "支持使用图标",
                Description = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd")
            }
        };

        /// <summary>
        /// 
        /// </summary>
        private IEnumerable<TimelineItem> GetCustomerComponentTimelineItems() => new TimelineItem[]
        {
            new TimelineItem
            {
                Color = Color.Success,
                Component = BootstrapDynamicComponent.CreateComponent<BootstrapBlazor.Components.Console>(new KeyValuePair<string, object?>[]
                {
                    new(nameof(BootstrapBlazor.Components.Console.Items), Messages)
                }),
                Description = "实时输出"
            },
            new TimelineItem
            {
                Color = Color.Info,
                Component = BootstrapDynamicComponent.CreateComponent<Counter>(),
                Description = "计数器"
            },
            new TimelineItem
            {
                Color = Color.Warning,
                Component = BootstrapDynamicComponent.CreateComponent<FetchData>(),
                Description = "天气预报信息"
            }
        };

        /// <summary>
        /// 
        /// </summary>
        private readonly IEnumerable<TimelineItem> AlternateTimelineItems = new TimelineItem[]
        {
            new TimelineItem
            {
                Content = "Create a services site 2015-09-01",
            },
            new TimelineItem
            {
                Color = Color.Success,
                Content = "Solve initial network problems 2015-09-01",
            },
            new TimelineItem
            {
                Color = Color.Danger,
                Content = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.",
            },
            new TimelineItem
            {
                Color = Color.Warning,
                Content = "Network problems being solved 2015-09-01",
            },
            new TimelineItem
            {
                Color = Color.Info,
                Content = "Create a services site 2015-09-01",
            }
        };

        /// <summary>
        /// 获得属性方法
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
        {
            // TODO: 移动到数据库中
            new AttributeItem() {
                Name = "Items",
                Description = "数据集合",
                Type = "IEnumerable<TimelineItem>",
                ValueList = "—",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = "IsReverse",
                Description = "是否倒序显示",
                Type = "boolean",
                ValueList = "true|false",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsLeft",
                Description = "是否左侧展现内容",
                Type = "boolean",
                ValueList = "true|false",
                DefaultValue = "false"
            },
            new AttributeItem() {
                Name = "IsAlternate",
                Description = "是否交替展现内容",
                Type = "boolean",
                ValueList = "true|false",
                DefaultValue = "false"
            },
        };

        /// <summary>
        /// 获得属性方法
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<AttributeItem> GetTimelineItemAttributes() => new AttributeItem[]
        {
            // TODO: 移动到数据库中
            new AttributeItem() {
                Name = nameof(TimelineItem.Color),
                Description = "节点颜色",
                Type = "Color",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = nameof(TimelineItem.Content),
                Description = "内容正文",
                Type = "string",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = nameof(TimelineItem.Icon),
                Description = "节点图标",
                Type = "string",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = nameof(TimelineItem.Description),
                Description = "节点描述文字",
                Type = "string",
                ValueList = " — ",
                DefaultValue = " — "
            },
            new AttributeItem() {
                Name = nameof(TimelineItem.Component),
                Description = "节点自定义组件",
                Type = nameof(BootstrapDynamicComponent),
                ValueList = " — ",
                DefaultValue = " — "
            }
        };
    }
}
