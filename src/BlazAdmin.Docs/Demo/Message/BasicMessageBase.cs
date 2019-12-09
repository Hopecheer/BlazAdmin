﻿using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazAdmin.Docs.Demo.Message
{
    public class BasicMessageBase : ComponentBase
    {
        [Inject]
        public MessageService Message { get; set; }
        
        public void ShowMessage()
        {
            Message.Show("普通消息");
        }

    }
}
