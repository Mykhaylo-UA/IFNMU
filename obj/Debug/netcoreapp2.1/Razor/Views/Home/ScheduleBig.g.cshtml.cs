#pragma checksum "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c16f1280be42ffe6d13601717f4856c106e5a409"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ScheduleBig), @"mvc.1.0.view", @"/Views/Home/ScheduleBig.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/ScheduleBig.cshtml", typeof(AspNetCore.Views_Home_ScheduleBig))]
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
#line 2 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
using System.Globalization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c16f1280be42ffe6d13601717f4856c106e5a409", @"/Views/Home/ScheduleBig.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68a1ec3d6fbaa4e724ec210c3c6ef4ee28b939e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ScheduleBig : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IFNMUSiteCore.Models.Week>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("margin:0;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("/Home/ScheduleBig"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-ajax", new global::Microsoft.AspNetCore.Html.HtmlString("true"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-ajax-loading", new global::Microsoft.AspNetCore.Html.HtmlString("#loading"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-ajax-mode", new global::Microsoft.AspNetCore.Html.HtmlString("replace"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-ajax-update", new global::Microsoft.AspNetCore.Html.HtmlString("#results2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
  Layout = null;

#line default
#line hidden
            BeginContext(83, 36, true);
            WriteLiteral("<div class=\"titlePage\"><span>Курс - ");
            EndContext();
            BeginContext(120, 14, false);
#line 4 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                               Write(ViewBag.Course);

#line default
#line hidden
            EndContext();
            BeginContext(134, 11, true);
            WriteLiteral(" | Група - ");
            EndContext();
            BeginContext(146, 13, false);
#line 4 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                                         Write(ViewBag.Group);

#line default
#line hidden
            EndContext();
            BeginContext(159, 100, true);
            WriteLiteral("</span></div>\r\n<div style=\"text-align: center;margin: 1%;font-size: 1.2em;\">Поточний тиждень: <br/> ");
            EndContext();
            BeginContext(260, 80, false);
#line 5 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                                                                Write(Convert.ToDateTime(Model.From).ToString("d MMMM yyyy", new CultureInfo("uk-UA")));

#line default
#line hidden
            EndContext();
            BeginContext(340, 3, true);
            WriteLiteral(" - ");
            EndContext();
            BeginContext(344, 78, false);
#line 5 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                                                                                                                                                    Write(Convert.ToDateTime(Model.To).ToString("d MMMM yyyy", new CultureInfo("uk-UA")));

#line default
#line hidden
            EndContext();
            BeginContext(422, 8, true);
            WriteLiteral("</div>\r\n");
            EndContext();
            BeginContext(430, 768, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eb6ba23906084fb0bd595eaa932ff83a", async() => {
                BeginContext(595, 173, true);
                WriteLiteral("\r\n    <select style=\"background-color: azure;border: 0;border-radius: 4px; margin:0.5%; font-size: 16px;\" name=\"WeekId\" id=\"WeekId\" onchange=\"$(\'#sbm\').trigger(\'click\');\">\r\n");
                EndContext();
#line 8 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
         foreach (var w in ViewBag.Weeks)
        {

#line default
#line hidden
                BeginContext(822, 12, true);
                WriteLiteral("            ");
                EndContext();
                BeginContext(834, 186, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "aa037f918601417595a9c29f57f92945", async() => {
                    BeginContext(857, 76, false);
#line 10 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                             Write(Convert.ToDateTime(w.From).ToString("d MMMM yyyy", new CultureInfo("uk-UA")));

#line default
#line hidden
                    EndContext();
                    BeginContext(933, 3, true);
                    WriteLiteral(" - ");
                    EndContext();
                    BeginContext(937, 74, false);
#line 10 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                                                                                             Write(Convert.ToDateTime(w.To).ToString("d MMMM yyyy", new CultureInfo("uk-UA")));

#line default
#line hidden
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#line 10 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
               WriteLiteral(w.Id);

#line default
#line hidden
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1020, 2, true);
                WriteLiteral("\r\n");
                EndContext();
#line 11 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
        }

#line default
#line hidden
                BeginContext(1033, 39, true);
                WriteLiteral("    </select>\r\n    <input type=\"hidden\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1072, "\"", 1094, 1);
#line 13 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
WriteAttributeValue("", 1080, ViewBag.Group, 1080, 14, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1095, 96, true);
                WriteLiteral(" name=\"group\" id=\"group\"/>\r\n    <input id=\"sbm\" style=\"display:none;\" type=\"submit\" value=\"\"/>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1198, 176, true);
            WriteLiteral("\r\n<div id=\"results2\">\r\n    <table class=\"tableModal\">\r\n        <tr>\r\n            <th>№</th>\r\n            <th>Name</th>\r\n            <th>Time</th>\r\n        </tr>\r\n    </table>\r\n");
            EndContext();
#line 24 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
     foreach (var day in Model.ScheduleDays.OrderBy(day => day.DayNumber))
    {

#line default
#line hidden
            BeginContext(1457, 59, true);
            WriteLiteral("        <table class=\"tableModal\">\r\n            <caption>\r\n");
            EndContext();
#line 28 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                 switch (@day.DayNumber)
                {
                    case 1:

#line default
#line hidden
            BeginContext(1606, 48, true);
            WriteLiteral("                        <span>Понеділок</span>\r\n");
            EndContext();
#line 32 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                        break;
                    case 2:

#line default
#line hidden
            BeginContext(1715, 47, true);
            WriteLiteral("                        <span>Вівторок</span>\r\n");
            EndContext();
#line 35 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                        break;
                    case 3:

#line default
#line hidden
            BeginContext(1823, 45, true);
            WriteLiteral("                        <span>Середа</span>\r\n");
            EndContext();
#line 38 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                        break;
                    case 4:

#line default
#line hidden
            BeginContext(1929, 45, true);
            WriteLiteral("                        <span>Четвер</span>\r\n");
            EndContext();
#line 41 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                        break;
                    case 5:

#line default
#line hidden
            BeginContext(2035, 47, true);
            WriteLiteral("                        <span>П\'ятниця</span>\r\n");
            EndContext();
#line 44 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                        break;
                }

#line default
#line hidden
            BeginContext(2133, 24, true);
            WriteLiteral("            </caption>\r\n");
            EndContext();
#line 47 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
             foreach (var lessons in day.Lessons.OrderBy(les => les.LessonNumber))
            {

#line default
#line hidden
            BeginContext(2256, 46, true);
            WriteLiteral("                <tr>\r\n                    <td>");
            EndContext();
            BeginContext(2303, 20, false);
#line 50 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                   Write(lessons.LessonNumber);

#line default
#line hidden
            EndContext();
            BeginContext(2323, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(2355, 81, false);
#line 51 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                   Write(Html.ActionLink(lessons.Name, "Documents", "Home", new { id = lessons.Id }, null));

#line default
#line hidden
            EndContext();
            BeginContext(2436, 33, true);
            WriteLiteral("</td>\r\n                    <td>\r\n");
            EndContext();
#line 53 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                         switch (lessons.LessonNumber)
                        {
                            case 1:

#line default
#line hidden
            BeginContext(2589, 51, true);
            WriteLiteral("                                <span>8:00</span>\r\n");
            EndContext();
#line 57 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                break;
                            case 2:

#line default
#line hidden
            BeginContext(2717, 52, true);
            WriteLiteral("                                <span>10:05</span>\r\n");
            EndContext();
#line 60 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                break;
                            case 3:

#line default
#line hidden
            BeginContext(2846, 52, true);
            WriteLiteral("                                <span>12:10</span>\r\n");
            EndContext();
#line 63 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                break;
                            case 4:

#line default
#line hidden
            BeginContext(2975, 52, true);
            WriteLiteral("                                <span>14:15</span>\r\n");
            EndContext();
#line 66 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
                                break;
                        }

#line default
#line hidden
            BeginContext(3094, 50, true);
            WriteLiteral("                    </td>\r\n                </tr>\r\n");
            EndContext();
#line 70 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
            }

#line default
#line hidden
            BeginContext(3159, 18, true);
            WriteLiteral("        </table>\r\n");
            EndContext();
#line 72 "C:\Users\asus\Desktop\Project\IFNMUSIte\IFNMUSiteCore\Views\Home\ScheduleBig.cshtml"
    }

#line default
#line hidden
            BeginContext(3184, 6, true);
            WriteLiteral("</div>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IFNMUSiteCore.Models.Week> Html { get; private set; }
    }
}
#pragma warning restore 1591
