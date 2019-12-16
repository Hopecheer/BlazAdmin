﻿using Blazui.Component;
using Blazui.Component.Table;
using BlazAdmin.Docs.Demo.Table;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazAdmin.Docs.Demo.Loading
{
    public class BasicLoadingBase : ComponentBase
    {
        protected List<TestData> Datas = new List<TestData>();

        [Inject]
        private LoadingService LoadingService { get; set; }
        protected IContainerComponent table;
        protected override void OnInitialized()
        {
            Datas.Add(new TestData()
            {
                Address = "地址1",
                Name = "张三",
                Time = DateTime.Now
            });
            Datas.Add(new TestData()
            {
                Address = "地址2",
                Name = "张三1",
                Time = DateTime.Now
            });
            Datas.Add(new TestData()
            {
                Address = "地址3",
                Name = "张三3",
                Time = DateTime.Now
            });
        }

        internal async Task<PagerResult> LoadDataSource(int currentPage)
        {
            var result = new PagerResult()
            {
                Rows = Datas,
                Total = Datas.Count
            };
            return await Task.FromResult(result);
        }
        protected void RenderCompleted()
        {
            table.Loading(LoadingService);
        }
        protected void CustomRenderCompleted()
        {
            table.Loading(LoadingService, "拼命加载中", "el-icon-loading", "rgba(0, 0, 0, 0.8)");
        }

        protected void ShowLoading()
        {
            table.Loading(LoadingService);
        }

        protected void CloseLoading()
        {
            table.Close(LoadingService);
        }
    }
}
