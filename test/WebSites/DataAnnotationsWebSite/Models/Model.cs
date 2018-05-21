// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsWebSite.Models
{
    public class Model
    {
        [Display(Name = "ModelEnum")]
        public ModelEnum Id { get; set; }
    }

    public enum ModelEnum
    {
        [Display(Name = "FirstOptionDisplay")]
        FirstOption,
        SecondOptions
    }
}
