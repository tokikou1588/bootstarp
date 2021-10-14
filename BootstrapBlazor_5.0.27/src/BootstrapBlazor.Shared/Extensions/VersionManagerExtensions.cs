﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 版本获取服务
    /// </summary>
    public static class VesionManagerExtensions
    {
        /// <summary>
        /// 注入版本获取服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVersionManager(this IServiceCollection services)
        {
            services.AddTransient<NugetVersionService>();
            return services;
        }
    }

    internal class NugetVersionService
    {
        private HttpClient Client { get; set; }

        private static string Version { get; set; } = "latest";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="client"></param>
        public NugetVersionService(HttpClient client)
        {
            Client = client;
            Client.Timeout = TimeSpan.FromSeconds(5);

            Task.Run(async () =>
            {
                do
                {
                    await FetchVersionAsync();

                    await Task.Delay(300000);
                    Version = "latest";
                }
                while (true);
            });
        }

        /// <summary>
        /// 获得组件版本号方法
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetVersionAsync(string packageName = "bootstrapblazor")
        {
            Version = "latest";
            await FetchVersionAsync(packageName);
            return Version;
        }

        private async Task FetchVersionAsync(string packageName = "bootstrapblazor")
        {
            if (Version == "latest")
            {
                try
                {
                    var url = $"https://azuresearch-usnc.nuget.org/query?q={packageName}&prerelease=true&semVerLevel=2.0.0";
                    var package = await Client.GetFromJsonAsync<NugetPackage>(url);
                    if (package != null) Version = package.GetVersion();
                }
                catch { }
            }
        }

        private class NugetPackage
        {
            /// <summary>
            /// Data 数据集合
            /// </summary>
            public IEnumerable<NugetPackageData> Data { get; set; } = Array.Empty<NugetPackageData>();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetVersion() => Data.FirstOrDefault()?.Version ?? "";
        }

        private class NugetPackageData
        {
            /// <summary>
            /// 版本号
            /// </summary>
            public string Version { get; set; } = "";
        }
    }
}
