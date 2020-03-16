#pragma checksum "D:\JRPC_HMS\JRPC_HMS\Pages\Documentation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "437617459cf9df6d0539ab2752b12ef87cab5f6c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(JRPC_HMS.Pages.Pages_Documentation), @"mvc.1.0.razor-page", @"/Pages/Documentation.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/Documentation.cshtml", typeof(JRPC_HMS.Pages.Pages_Documentation), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"437617459cf9df6d0539ab2752b12ef87cab5f6c", @"/Pages/Documentation.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7abe7b4b9681a4257d040f20b51282dfdde00424", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Documentation : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "D:\JRPC_HMS\JRPC_HMS\Pages\Documentation.cshtml"
  
    ViewData["Title"] = "Documentation";

#line default
#line hidden
            BeginContext(98, 2903, true);
            WriteLiteral(@"
<style>
    .tabs-left > .nav-tabs {
        border-bottom: 0;
    }

    .tab-content > .tab-pane,
    .pill-content > .pill-pane {
        display: none;
    }

    .tab-content > .active,
    .pill-content > .active {
        display: block;        
    }
        
    .tabs-left > .nav-tabs > li {
        float: none;
    }

        .tabs-left > .nav-tabs > li > a {
            min-width: 100px;
            margin-right: 0;
            margin-bottom: 3px;
        }

    .tabs-left > .nav-tabs {
        float: left;
        margin-right: 19px;
        border-right: 1px solid #ddd;
    }

        .tabs-left > .nav-tabs > li > a {
            margin-right: -1px;
            -webkit-border-radius: 4px 0 0 4px;
            -moz-border-radius: 4px 0 0 4px;
            border-radius: 4px 0 0 4px;
        }

            .tabs-left > .nav-tabs > li > a:hover,
            .tabs-left > .nav-tabs > li > a:focus {
                border-color: #eeeeee #dddddd #eeeeee #eeeeee;
");
            WriteLiteral(@"                background-color: #337AB7;
                color: #FFFFFF;
            }

        .tabs-left > .nav-tabs .active > a,
        .tabs-left > .nav-tabs .active > a:hover,
        .tabs-left > .nav-tabs .active > a:focus {
            border-color: #ddd transparent #ddd #ddd;
            *border-right-color: #ffffff;
            background-color: #337AB7;
            color: #FFFFFF;
        }
</style>

<section class=""content-header"">
    <h1>
        Documentation
    </h1>
    <ol class=""breadcrumb"">
        <li><a href=""#""><i class=""fa fa-dashboard""></i> Home</a></li>
        <li class=""active"">Documentation</li>
    </ol>
</section>

<section class=""content"">
    <div class=""row"">
        <div class=""col-xs-12"">
            <div class=""box box-primary"">
                <div class=""box-body no-padding"">
                    <div class=""tabbable tabs-left"">
                        <ul class=""nav nav-tabs"">
                            <li class=""active""><a href=""#a"" ");
            WriteLiteral(@"data-toggle=""tab"">Tutorial 1</a></li>
                            <li><a href=""#b"" data-toggle=""tab"">Tutorial 2</a></li>
                            <li><a href=""#c"" data-toggle=""tab"">Tutorial 3</a></li>
                        </ul>
                        <div class=""tab-content"">
                            <div class=""tab-pane active"" id=""a"">
                                Tutorial 1
                            </div>
                            <div class=""tab-pane"" id=""b"">
                                Tutorial 2
                            </div>
                            <div class=""tab-pane"" id=""c"">
                                Tutorial 3
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<JRPC_HMS.Pages.DocumentationModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<JRPC_HMS.Pages.DocumentationModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<JRPC_HMS.Pages.DocumentationModel>)PageContext?.ViewData;
        public JRPC_HMS.Pages.DocumentationModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
