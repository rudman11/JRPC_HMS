#pragma checksum "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7929c8c1ccb9949e8b99097d63252701cf8d4813"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__TopCounts), @"mvc.1.0.view", @"/Views/Shared/_TopCounts.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_TopCounts.cshtml", typeof(AspNetCore.Views_Shared__TopCounts))]
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
#line 1 "D:\JRPC_HMS\JRPC_HMS\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "D:\JRPC_HMS\JRPC_HMS\Views\_ViewImports.cshtml"
using JRPC_HMS;

#line default
#line hidden
#line 3 "D:\JRPC_HMS\JRPC_HMS\Views\_ViewImports.cshtml"
using JRPC_HMS.Data;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7929c8c1ccb9949e8b99097d63252701cf8d4813", @"/Views/Shared/_TopCounts.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7708c60378eed77829083da78d87ef9846d972a", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__TopCounts : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<JRPC_HMS.Models.TopCountsViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/Sales/OrdersIndex", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("small-box-footer"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-orderStatus", "Incompleted", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-currentpage", "1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-pageSize", "10", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/Catalog/StockIndex", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-orderStatus", "Voided", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/QRServ/FeedbackIndex", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-status", "NEGATIVE", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-status", "POSITIVE", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/QRServ/Customers", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(43, 141, true);
            WriteLiteral("\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-aqua\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(185, 20, false);
#line 7 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.AllOrdersCount);

#line default
#line hidden
            EndContext();
            BeginContext(205, 158, true);
            WriteLiteral("</h3>\r\n            <p>Total Orders</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-shopping-cart\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(363, 112, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d48137364", async() => {
                BeginContext(421, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(475, 179, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-yellow\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(655, 26, false);
#line 21 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.IncompleteOrderCount);

#line default
#line hidden
            EndContext();
            BeginContext(681, 166, true);
            WriteLiteral("</h3>\r\n            <p>Incompleted Orders</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-cart-arrow-down\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(847, 198, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d48139556", async() => {
                BeginContext(991, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-orderStatus", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["orderStatus"] = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["currentpage"] = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["pageSize"] = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1045, 179, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-yellow\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(1225, 19, false);
#line 35 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.LowStockCount);

#line default
#line hidden
            EndContext();
            BeginContext(1244, 157, true);
            WriteLiteral("</h3>\r\n            <p>Total Low Stock</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-chart-pie\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(1401, 113, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481312705", async() => {
                BeginContext(1460, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1514, 176, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-red\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(1691, 23, false);
#line 49 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.VoidedOrdersCount);

#line default
#line hidden
            EndContext();
            BeginContext(1714, 166, true);
            WriteLiteral("</h3>\r\n            <p>Voided Orders</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-exclamation-triangle\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(1880, 193, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481314897", async() => {
                BeginContext(2019, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-orderStatus", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["orderStatus"] = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["currentpage"] = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["pageSize"] = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2073, 177, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-lime\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(2251, 15, false);
#line 63 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.Feedbacks);

#line default
#line hidden
            EndContext();
            BeginContext(2266, 156, true);
            WriteLiteral("</h3>\r\n            <p>Total Feedbacks</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-comments\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(2422, 115, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481318041", async() => {
                BeginContext(2483, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2537, 176, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-red\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(2714, 19, false);
#line 77 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.TotalNegative);

#line default
#line hidden
            EndContext();
            BeginContext(2733, 168, true);
            WriteLiteral("</h3>\r\n            <p>Total Negative Feedbacks</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-thumbs-down\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(2901, 193, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481320231", async() => {
                BeginContext(3040, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-status", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["status"] = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["currentpage"] = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["pageSize"] = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3094, 178, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-green\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(3273, 19, false);
#line 91 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.TotalPositive);

#line default
#line hidden
            EndContext();
            BeginContext(3292, 166, true);
            WriteLiteral("</h3>\r\n            <p>Total Positive Feedbacks</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-thumbs-up\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(3458, 193, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481323380", async() => {
                BeginContext(3597, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-status", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["status"] = (string)__tagHelperAttribute_9.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_9);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["currentpage"] = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["pageSize"] = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3651, 188, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->\r\n<div class=\"col-lg-3 col-xs-6\">\r\n    <!-- small box -->\r\n    <div class=\"small-box bg-maroon-gradient\">\r\n        <div class=\"inner\">\r\n            <h3>");
            EndContext();
            BeginContext(3840, 15, false);
#line 105 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_TopCounts.cshtml"
           Write(Model.Customers);

#line default
#line hidden
            EndContext();
            BeginContext(3855, 162, true);
            WriteLiteral("</h3>\r\n            <p>Total Feedback Customers</p>\r\n        </div>\r\n        <div class=\"icon\">\r\n            <i class=\"fas fa-users\"></i>\r\n        </div>\r\n        ");
            EndContext();
            BeginContext(4017, 111, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7929c8c1ccb9949e8b99097d63252701cf8d481326532", async() => {
                BeginContext(4074, 50, true);
                WriteLiteral("More info <i class=\"fa fa-arrow-circle-right\"></i>");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_10.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_10);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(4128, 36, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!-- ./col -->");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<JRPC_HMS.Models.TopCountsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591