// <auto-generated/>
#pragma warning disable 1591
namespace Test
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "x:\dir\subdir\Test\UseTestComponent.cshtml"
using Test;

#line default
#line hidden
#nullable disable
    public partial class UseTestComponent : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 219
        private void __RazorDirectiveTokenHelpers__() {
        }
        #pragma warning restore 219
        #pragma warning disable 0414
        private static System.Object __o = null;
        #pragma warning restore 0414
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            var __typeInference_CreateTestComponent_0 = global::__Blazor.Test.UseTestComponent.TypeInference.CreateTestComponent_0(__builder, -1, -1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                      item1

#line default
#line hidden
#nullable disable
            , -1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                                    items

#line default
#line hidden
#nullable disable
            , -1, 
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                                                 item1

#line default
#line hidden
#nullable disable
            , -1, (context) => (__builder2) => {
#nullable restore
#line 3 "x:\dir\subdir\Test\UseTestComponent.cshtml"
  __o = context;

#line default
#line hidden
#nullable disable
            }
            );
            __o = __typeInference_CreateTestComponent_0.
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
               Item1

#line default
#line hidden
#nullable disable
            ;
            __o = __typeInference_CreateTestComponent_0.
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                            Items2

#line default
#line hidden
#nullable disable
            ;
            __o = __typeInference_CreateTestComponent_0.
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
                                          Item3

#line default
#line hidden
#nullable disable
            ;
#nullable restore
#line 2 "x:\dir\subdir\Test\UseTestComponent.cshtml"
__o = typeof(global::Test.TestComponent<,,>);

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 6 "x:\dir\subdir\Test\UseTestComponent.cshtml"
       
    Image item1 = new Image() { id = 1, url="https://example.com"};
    static Tag tag1 = new Tag() { description = "A description."};
    static Tag tag2 = new Tag() { description = "Another description."};
    List<Tag> items = new List<Tag>() { tag1, tag2 };

#line default
#line hidden
#nullable disable
    }
}
namespace __Blazor.Test.UseTestComponent
{
    #line hidden
    internal static class TypeInference
    {
        public static global::Test.TestComponent<TItem1, TItem2, TItem3> CreateTestComponent_0<TItem1, TItem2, TItem3>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, TItem1 __arg0, int __seq1, global::System.Collections.Generic.List<TItem2> __arg1, int __seq2, TItem3 __arg2, int __seq3, global::Microsoft.AspNetCore.Components.RenderFragment<TItem2> __arg3)
            where TItem1 : global::Image
            where TItem2 : global::ITag
            where TItem3 : global::Image, new()
        {
        __builder.OpenComponent<global::Test.TestComponent<TItem1, TItem2, TItem3>>(seq);
        __builder.AddAttribute(__seq0, "Item1", __arg0);
        __builder.AddAttribute(__seq1, "Items2", __arg1);
        __builder.AddAttribute(__seq2, "Item3", __arg2);
        __builder.AddAttribute(__seq3, "ChildContent", __arg3);
        __builder.CloseComponent();
        return default;
        }
    }
}
#pragma warning restore 1591
