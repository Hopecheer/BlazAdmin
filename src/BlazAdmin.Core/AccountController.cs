﻿using BlazAdmin.Core.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazAdmin.Core
{
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Login([FromForm]LoginInfoModel model, [FromQuery]string callback)
        {
            var err = await userService.LoginAsync(model.Username, model.Password);
            if (!string.IsNullOrWhiteSpace(err))
            {
                return BadRequest(err);
            }
            return Redirect(callback);
        }

        public async Task<IActionResult> Logout([FromQuery]string callback)
        {
            await userService.LogoutAsync();
            return Redirect(callback);
        }
    }
}
