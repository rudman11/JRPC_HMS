#pragma checksum "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b92f8db38839a7aa090b45b30efaa56a11f9ee13"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(JRPC_HMS.Pages.Pages_Index), @"mvc.1.0.razor-page", @"/Pages/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Index.cshtml", typeof(JRPC_HMS.Pages.Pages_Index), null)]
namespace JRPC_HMS.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\JRPC_HMS\JRPC_HMS\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "D:\JRPC_HMS\JRPC_HMS\Pages\_ViewImports.cshtml"
using JRPC_HMS;

#line default
#line hidden
#line 3 "D:\JRPC_HMS\JRPC_HMS\Pages\_ViewImports.cshtml"
using JRPC_HMS.Data;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b92f8db38839a7aa090b45b30efaa56a11f9ee13", @"/Pages/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7abe7b4b9681a4257d040f20b51282dfdde00424", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("submit"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-metrics", "Daily", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page-handler", "MetricsChange", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-metrics", "Weekly", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-metrics", "Monthly", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-metrics", "Overall", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page-handler", "HasRead", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-success"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_11 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
  
    ViewData["Title"] = "Dashboard";

#line default
#line hidden
            DefineSection("Scripts", async() => {
                BeginContext(89, 6725, true);
                WriteLiteral(@"
    <script type=""text/javascript"">
        var chart = null;
        var chart2 = null;
        var cats = null;
        var data = null;
        function createChart() {
            $('#loading').show();
            $('#topCounts').load('/Index?handler=TopCountsView');
            $('#latestOrders').load('/Index?handler=LatestOrdersView');
            $('#lowStockList').load('/Index?handler=LowStockView');
            $('#orderTotals').load('/Index?handler=OrderTotalsView');
            $('#bestsellers').load('/Index?handler=BestsellersView');
            $('#feedbacks').load('/Index?handler=FeedbacksView');
            $.getJSON('/Index?handler=OrdersBarChartView', function (indata) {
                cats = indata[0];
                data = indata[1];
                chart = new Highcharts.chart('container', {
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: ''
                    },");
                WriteLiteral(@"
                    subtitle: {
                        text: ''
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        categories: cats,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        tickInterval: 10,
                        lineWidth: 1,
                        allowDecimals: false,
                        title: {
                            text: 'Order Amounts'
                        }
                    },
                    tooltip: {
                        backgroundColor: '#DCEFFC',
                        borderColor: '#5F6670',
                        headerFormat: '<span style=""font-size:18px"">{point.key}</span><table>',
                        pointFormat: '<tr><td style=""color:#1779ba;padding:5px;background-color:#DCEFFC;"">{series.name}: </td>' +
       ");
                WriteLiteral(@"                     '<td style=""color:#1779ba;padding:5px;background-color:#DCEFFC;""><b>{point.y}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 0
                        },
                        series: {
                            dataLabels: {
                                inside: true,
                                enabled: true,
                                color: '#5F6670',
                                useHTML: true,
                                style: {
                                    fontSize: '15px'
                                }
                            }
                        }
        ");
                WriteLiteral(@"            },
                    series: [{
                        name: 'Orders',
                        data: data
                    }]
                });
            });
            $.getJSON('/Index?handler=FeedbackBarChartView', function (indata) {
                cats = indata[0];
                data = indata[1];
                chart2 = new Highcharts.chart('containerFeedback', {
                    chart: {
                        type: 'column'
                    },
                    credits: {
                        enabled: false
                    },
                    title: {
                        text: ''
                    },
                    subtitle: {
                        text: ''
                    },
                    xAxis: {
                        categories: cats,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        tickInterval: 10,
  ");
                WriteLiteral(@"                      lineWidth: 1,
                        allowDecimals: false,
                        title: {
                            text: 'No of Feedbacks'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        backgroundColor: '#DCEFFC',
                        borderColor: '#5F6670',
                        headerFormat: '<span style=""font-size:18px"">{point.key}</span><table>',
                        pointFormat: '<tr><td style=""color:#1779ba;padding:5px;background-color:#DCEFFC;"">{series.name}: </td>' +
                            '<td style=""color:#1779ba;padding:5px;background-color:#DCEFFC;""><b>{point.y}</b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    plotOptions: {
                        column: {");
                WriteLiteral(@"
                            borderColor: '#5F6670',
                            color: '#DCEFFC',
                            borderRadius: 5,
                            pointPadding: 0,
                            borderWidth: 2,
                            dataLabels: {
                                inside: true,
                                enabled: true,
                                color: '#5F6670',
                                useHTML: true,
                                style: {
                                    fontSize: '15px'
                                }
                            }
                        }
                    },
                    series: [{
                        name: 'Total Feedbacks',
                        data: data
                    }]
                });
            });
            $.getJSON('/Index?handler=UserReadWhatsNew', function (data) {
                if (data.hasread === ""Yes"") {
                    $('#newFeatu");
                WriteLiteral(@"res').modal('hide');
                }
                else {
                    $('#newFeatures').modal('show');
                }
            });
            $('#loading').hide();
        }

        $(function () {
            createChart(cats, data);
            setInterval(function () {
                chart.destroy();
                chart2.destroy();
                chart = null;
                chart2 = null;
                cats = null;
                data = null;
                createChart();
            }, 15000);
        });
    </script>
");
                EndContext();
            }
            );
            BeginContext(6817, 4417, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1314576", async() => {
                BeginContext(6855, 96, true);
                WriteLiteral("\r\n    <section class=\"content-header\">\r\n        <h1>\r\n            Dashboard\r\n            <small>");
                EndContext();
                BeginContext(6952, 13, false);
#line 180 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
              Write(Model.Metrics);

#line default
#line hidden
                EndContext();
                BeginContext(6965, 173, true);
                WriteLiteral(" Data</small>\r\n        </h1>\r\n        <ol class=\"breadcrumb\">\r\n            <li><a href=\"#\"><i class=\"fa fa-dashboard\"></i> Home</a></li>\r\n        </ol>\r\n    </section>\r\n    ");
                EndContext();
                BeginContext(7138, 41, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "b92f8db38839a7aa090b45b30efaa56a11f9ee1315568", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 186 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Metrics);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(7179, 182, true);
                WriteLiteral("\r\n    <section class=\"content\">\r\n        <div class=\"row\">\r\n            <div class=\"col-xs-12\">\r\n                <div class=\"btn-group pull-right\" role=\"group\">\r\n                    ");
                EndContext();
                BeginContext(7361, 119, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1317551", async() => {
                    BeginContext(7466, 5, true);
                    WriteLiteral("Daily");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-metrics", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["metrics"] = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(7480, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
                BeginContext(7502, 121, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1319789", async() => {
                    BeginContext(7608, 6, true);
                    WriteLiteral("Weekly");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-metrics", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["metrics"] = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(7623, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
                BeginContext(7645, 123, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1322028", async() => {
                    BeginContext(7752, 7, true);
                    WriteLiteral("Monthly");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-metrics", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["metrics"] = (string)__tagHelperAttribute_6.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(7768, 22, true);
                WriteLiteral("\r\n                    ");
                EndContext();
                BeginContext(7790, 123, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1324268", async() => {
                    BeginContext(7897, 7, true);
                    WriteLiteral("Overall");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-metrics", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
                }
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["metrics"] = (string)__tagHelperAttribute_7.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(7913, 1652, true);
                WriteLiteral(@"
                </div>
            </div>
        </div>
        <div class=""row"">&nbsp;</div>
        <div class=""row"">
            <!-- Top Counts -->
            <div id=""topCounts""></div>
        </div>
        <!-- /.row -->
        <div class=""row"">
            <!-- Bar Chart: Orders -->
            <div class=""col-xs-12"">
                <div class=""box box-primary"">
                    <div class=""box-header with-border"">
                        <h3 class=""box-title"">Orders</h3>
                    </div>
                    <div class=""box-body"">
                        <div id=""container"" style=""width:100%; height:300px;""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-8"">
                <!-- TABLE: LATEST ORDERS -->
                <div id=""latestOrders""></div>
            </div>
            <div class=""col-md-4"">
                <!-- Low Stock List -->
               ");
                WriteLiteral(@" <div id=""lowStockList""></div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-md-8"">
                <!-- TABLE: Order Totals -->
                <div id=""orderTotals""></div>
            </div>
            <div class=""col-md-4"">
                <!-- Bestsellers -->
                <div id=""bestsellers""></div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-xs-12"">
                <div class=""box box-primary"">
                    <div class=""box-header with-border"">
                        <h3 class=""box-title"">Feedbacks for ");
                EndContext();
                BeginContext(9566, 13, false);
#line 241 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
                                                       Write(Model.Metrics);

#line default
#line hidden
                EndContext();
                BeginContext(9579, 896, true);
                WriteLiteral(@"</h3>
                    </div>
                    <div class=""box-body"">
                        <div id=""containerFeedback"" style=""width:100%; height:300px;""></div>
                    </div>
                </div>
            </div>
        </div>
        <div class=""row"">
            <div class=""col-xs-12"">
                <div id=""feedbacks""></div>
            </div>
        </div>

        <!-- Modal -->
        <div class=""modal fade"" id=""newFeatures"" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"">
            <div class=""modal-dialog"" role=""document"">
                <div class=""modal-content"">
                    <div class=""modal-header"">
                        <h4 class=""modal-title"" id=""myModalLabel"">What's New</h4>
                    </div>
                    <div class=""modal-body"">
                        <ul class=""list-group"">
");
                EndContext();
#line 264 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
                             foreach (var feature in Model.ChangeLog)
                            {

#line default
#line hidden
                BeginContext(10577, 134, true);
                WriteLiteral("                                <li class=\"list-group-item\">\r\n                                    <h5 class=\"list-group-item-heading\">");
                EndContext();
                BeginContext(10712, 18, false);
#line 267 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
                                                                   Write(feature.ChangeName);

#line default
#line hidden
                EndContext();
                BeginContext(10730, 75, true);
                WriteLiteral("</h5>\r\n                                    <p class=\"list-group-item-text\">");
                EndContext();
                BeginContext(10806, 19, false);
#line 268 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
                                                               Write(feature.Description);

#line default
#line hidden
                EndContext();
                BeginContext(10825, 45, true);
                WriteLiteral("</p>\r\n                                </li>\r\n");
                EndContext();
#line 270 "D:\JRPC_HMS\JRPC_HMS\Pages\Index.cshtml"
                            }

#line default
#line hidden
                BeginContext(10901, 131, true);
                WriteLiteral("                        </ul>\r\n                    </div>\r\n                    <div class=\"modal-footer\">\r\n                        ");
                EndContext();
                BeginContext(11032, 89, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b92f8db38839a7aa090b45b30efaa56a11f9ee1331198", async() => {
                    BeginContext(11105, 7, true);
                    WriteLiteral("Dismiss");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.PageHandler = (string)__tagHelperAttribute_8.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(11121, 106, true);
                WriteLiteral("\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </section>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_10.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_10);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Page = (string)__tagHelperAttribute_11.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_11);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
