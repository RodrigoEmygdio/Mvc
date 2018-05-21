// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DataAnnotationsWebSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataAnnotationsWebSite.Controllers
{
    public class EnumController : Controller
    {
        public IActionResult Enum()
        {
            return View(new Model {
                Id = ModelEnum.FirstOption
            });
        }
    }
}
