#pragma checksum "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ThematicPlan.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6b472cd6b0f419fb6be538c4348f2261f5a80887"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ThematicPlan), @"mvc.1.0.view", @"/Views/Home/ThematicPlan.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/ThematicPlan.cshtml", typeof(AspNetCore.Views_Home_ThematicPlan))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\_ViewImports.cshtml"
using IFNMUSiteCore;

#line default
#line hidden
#line 2 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\_ViewImports.cshtml"
using IFNMUSiteCore.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6b472cd6b0f419fb6be538c4348f2261f5a80887", @"/Views/Home/ThematicPlan.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68a1ec3d6fbaa4e724ec210c3c6ef4ee28b939e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ThematicPlan : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ThematicPlan.cshtml"
  
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(54, 218, true);
            WriteLiteral("<div class=\"frameblock\" style=\"width: 100%;height: 89%; position:absolute;  background-position:center; background-image:url(/images/cooltext339224809839651.png); background-repeat:no-repeat;\">\r\n    <iframe id=\"iFrame\"");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 272, "\"", 291, 1);
#line 5 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ThematicPlan.cshtml"
WriteAttributeValue("", 278, ViewBag.Path, 278, 13, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(292, 117, true);
            WriteLiteral(" align=\"middle\" frameborder=\"0\" style=\"width:100%;height:100%; position:absolute;margin-top:2px;\"></iframe>\r\n</div>\r\n");
            EndContext();
            DefineSection("Script", async() => {
                BeginContext(425, 485, true);
                WriteLiteral(@"
    <script>
        function reloadIFrame() {
            var iframe = document.getElementById(""iFrame"");
            if (iframe.contentDocument.URL == ""about:blank"") {
                iframe.src = iframe.src;
            }
        }
        var timerId = setInterval(""reloadIFrame();"", 1000);

        $(document).ready(function () {
            $('#iFrame').on('load', function () {
                clearInterval(timerId);
            });
        });
    </script>
");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
