using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.TagHelpers
{
    public class ColumnTagHelper : TagHelper
    {
        public int Size { get; set; }

        public int Offset { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var cssClass = $"col-md-{Size} col-md-offset-{Offset}";
            output.Attributes.SetAttribute("class", cssClass);
        }
    }
}
