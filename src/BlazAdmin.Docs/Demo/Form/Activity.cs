﻿using Blazui.Component.Select;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazAdmin.Docs.Demo.Form
{
    public class Activity
    {
        public string Name { get; set; }
        public Area Area { get; set; }
        public DateTime? Time { get; set; }
        public bool Delivery { get; set; }
        public List<string> Type { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"名称：{Name},区域：{Area},日期：{Time?.ToString()}，即时配送：{Delivery}，性质：{string.Join(",", Type)}，特殊资源：{Resource}，活动形式：{Description}";
        }
    }

    public enum Area
    {
        [Description("上海")]
        Shanghai = 0,

        [Description("北京")]
        Beijing
    }
}
