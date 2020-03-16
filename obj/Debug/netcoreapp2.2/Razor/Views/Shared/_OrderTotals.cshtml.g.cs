#pragma checksum "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2ca2e32d20a853cd3b399b29f38adab415451cbc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__OrderTotals), @"mvc.1.0.view", @"/Views/Shared/_OrderTotals.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_OrderTotals.cshtml", typeof(AspNetCore.Views_Shared__OrderTotals))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ca2e32d20a853cd3b399b29f38adab415451cbc", @"/Views/Shared/_OrderTotals.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7708c60378eed77829083da78d87ef9846d972a", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__OrderTotals : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<JRPC_HMS.Models.OrderTotal>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(35, 979, true);
            WriteLiteral(@"
<div class=""box box-info"">
    <div class=""box-header with-border"">
        <h3 class=""box-title"">Order Totals</h3>
    </div>
    <!-- /.box-header -->
    <div class=""box-body"">
        <div class=""table-responsive"">
            <table class=""table no-margin"">
                <thead>
                    <tr>
                        <th style=""text-align:center;"">Order Status</th>
                        <th style=""text-align:center;"">Today</th>
                        <th style=""text-align:center;"">This Week</th>
                        <th style=""text-align:center;"">This Month</th>
                        <th style=""text-align:center;"">This Year</th>
                        <th style=""text-align:center;"">All Time</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style=""text-align:center;"">Pending</td>
                        <td style=""text-align:center;"">R ");
            EndContext();
            BeginContext(1015, 18, false);
#line 24 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.PendingToday);

#line default
#line hidden
            EndContext();
            BeginContext(1033, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1098, 17, false);
#line 25 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.PendingWeek);

#line default
#line hidden
            EndContext();
            BeginContext(1115, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1180, 18, false);
#line 26 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.PendingMonth);

#line default
#line hidden
            EndContext();
            BeginContext(1198, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1263, 17, false);
#line 27 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.PendingYear);

#line default
#line hidden
            EndContext();
            BeginContext(1280, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1345, 20, false);
#line 28 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.PendingAllTime);

#line default
#line hidden
            EndContext();
            BeginContext(1365, 188, true);
            WriteLiteral("</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">Completed</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1554, 20, false);
#line 32 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.CompletedToday);

#line default
#line hidden
            EndContext();
            BeginContext(1574, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1639, 19, false);
#line 33 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.CompletedWeek);

#line default
#line hidden
            EndContext();
            BeginContext(1658, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1723, 20, false);
#line 34 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.CompletedMonth);

#line default
#line hidden
            EndContext();
            BeginContext(1743, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1808, 19, false);
#line 35 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.CompletedYear);

#line default
#line hidden
            EndContext();
            BeginContext(1827, 64, true);
            WriteLiteral("</td>\r\n                        <td style=\"text-align:center;\">R ");
            EndContext();
            BeginContext(1892, 22, false);
#line 36 "D:\JRPC_HMS\JRPC_HMS\Views\Shared\_OrderTotals.cshtml"
                                                    Write(Model.CompletedAllTime);

#line default
#line hidden
            EndContext();
            BeginContext(1914, 169, true);
            WriteLiteral("</td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>\r\n        </div>\r\n        <!-- /.table-responsive -->\r\n    </div>\r\n</div>\r\n<!-- /.box -->");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<JRPC_HMS.Models.OrderTotal> Html { get; private set; }
    }
}
#pragma warning restore 1591
