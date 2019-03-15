//
//  CustomAddHeaderAttribute.cs
//
//  Copyright (c) Wiregrass Code Technology 2018-2019
//          
using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IndustryCodes
{
    public class CustomAddHeaderAttribute : ResultFilterAttribute
    {
        private readonly string headerName;
        private readonly string headerValue;

        public CustomAddHeaderAttribute(string name, string value)
        {
            headerName = name;
            headerValue = value;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Headers.Add(headerName, new[] { headerValue });

            base.OnResultExecuting(context);
        }
    }
}