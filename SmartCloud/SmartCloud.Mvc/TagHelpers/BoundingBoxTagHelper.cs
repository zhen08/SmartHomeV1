using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using SmartCloud.Mvc.Models.HomeViewModels;
using SmartVideo.Model.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCloud.Mvc.TagHelpers
{
    [HtmlTargetElement("boundingbox")]
    public class BoundingBoxTagHelper : TagHelper
    {
        public VideoIndexViewModel Data { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string guid = Guid.NewGuid().ToString();
            output.TagName = "section";
            output.Content.AppendHtml($@"<canvas style=""background-image: url('/Image?blob={Data.ThumbnailBlob}'); background-size: 100% 100%;"" id=""{guid}"" width=""320"" height=""240""></canvas>");
            if (Data.BoundingBoxs != null)
            {
                output.Content.AppendHtml($@"<script>");
                output.Content.AppendHtml($@"     drawBoundingBox(""{guid}"", {JsonConvert.SerializeObject(Data.BoundingBoxs)})");
                output.Content.AppendHtml($@"</script>");
            }
            output.TagMode = TagMode.StartTagAndEndTag;
        }

    }
}
