﻿using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using BlazAdmin.Docs.Model;

namespace BlazAdmin.Docs.Pages
{
    public class PageBase : ComponentBase
    {
        private IList<DemoModel> Code(string name)
        {
            var location = Path.Combine(Path.GetDirectoryName(typeof(Startup).Assembly.Location), "Demo");
            var demoInfos = JsonConvert.DeserializeObject<IEnumerable<DemoPageModel>>(System.IO.File.ReadAllText(Path.Combine(location, "demos.json")));
            var demoInfo = demoInfos.SingleOrDefault(x => x.Name == name);
            if (demoInfo == null)
            {
                return new List<DemoModel>();
            }
            var demos = new List<DemoModel>();
            foreach (var item in demoInfo.Demos)
            {
                var razorPath = Path.Combine(location, item.Name + ".razor");
                var demoModel = new DemoModel()
                {
                    Type = "BlazAdmin.Docs.Demo." + item.Name,
                    Title = item.Title
                };
                if (System.IO.File.Exists(razorPath))
                {
                    var code = System.IO.File.ReadAllText(razorPath);
                    demoModel.Options.Add(new TabOption()
                    {
                        Content = GetCode(WebUtility.HtmlEncode(code), "razor"),
                        Name = item.Name,
                        Title = item.Name + ".razor",
                        OnRenderCompletedAsync = TabCode_OnRenderCompleteAsync
                    });
                    demos.Add(demoModel);
                    continue;
                }
                var codeFiles = Directory.EnumerateFiles(Path.Combine(location, item.Name))
                    .Where(x => item.Files.Contains(Path.GetFileName(x)))
                    .OrderBy(x => item.Files.IndexOf(Path.GetFileName(x)));
                demoModel.Type += "." + Path.GetFileNameWithoutExtension(codeFiles.FirstOrDefault());
                foreach (var codeFile in codeFiles)
                {
                    var extension = codeFile.Split('.').LastOrDefault().ToLower();
                    var language = extension;
                    var code = System.IO.File.ReadAllText(codeFile);
                    switch (extension)
                    {
                        case "razor":
                            break;
                        case "css":
                            break;
                        case "cs":
                            language = "csharp";
                            break;
                    }
                    demoModel.Options.Add(new TabOption()
                    {
                        Content = GetCode(WebUtility.HtmlEncode(code), language),
                        Title = Path.GetFileName(codeFile),
                        Name = language,
                        OnRenderCompletedAsync = TabCode_OnRenderCompleteAsync
                    });
                }
                demos.Add(demoModel);
            }
            return demos;
        }
        [Inject]
        private IHttpClientFactory httpClientFactory { get; set; }
        [Inject]
        protected IJSRuntime jSRuntime { get; set; }

        protected IList<DemoModel> demos;

        protected string GetCode(string code, string language)
        {
            return $"<pre lang=\"{language}\">{code}</pre>";
        }

        protected string GetName(string fileName)
        {
            return fileName.Replace(".", string.Empty);
        }

        [Parameter]
        public string Name { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        protected override void OnInitialized()
        {
            demos = Code(Name);
            foreach (var item in demos)
            {
                item.Demo = Type.GetType(item.Type);
            }
        }

        protected async Task TabCode_OnRenderCompleteAsync(object tab)
        {
            await jSRuntime.InvokeVoidAsync("renderHightlight", ((BTabPanelBase)tab).TabContainer.Content);
        }
    }
}
