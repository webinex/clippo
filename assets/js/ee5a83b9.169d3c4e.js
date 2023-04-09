"use strict";(self.webpackChunkdocs=self.webpackChunkdocs||[]).push([[187],{3905:(e,t,n)=>{n.d(t,{Zo:()=>c,kt:()=>f});var r=n(7294);function i(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function a(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function o(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?a(Object(n),!0).forEach((function(t){i(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):a(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function s(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var l=r.createContext({}),p=function(e){var t=r.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):o(o({},t),e)),n},c=function(e){var t=p(e.components);return r.createElement(l.Provider,{value:t},e.children)},d="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},m=r.forwardRef((function(e,t){var n=e.components,i=e.mdxType,a=e.originalType,l=e.parentName,c=s(e,["components","mdxType","originalType","parentName"]),d=p(n),m=i,f=d["".concat(l,".").concat(m)]||d[m]||u[m]||a;return n?r.createElement(f,o(o({ref:t},c),{},{components:n})):r.createElement(f,o({ref:t},c))}));function f(e,t){var n=arguments,i=t&&t.mdxType;if("string"==typeof e||i){var a=n.length,o=new Array(a);o[0]=m;var s={};for(var l in t)hasOwnProperty.call(t,l)&&(s[l]=t[l]);s.originalType=e,s[d]="string"==typeof e?e:i,o[1]=s;for(var p=2;p<a;p++)o[p]=n[p];return r.createElement.apply(null,o)}return r.createElement.apply(null,n)}m.displayName="MDXCreateElement"},4859:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>l,contentTitle:()=>o,default:()=>u,frontMatter:()=>a,metadata:()=>s,toc:()=>p});var r=n(7462),i=(n(7294),n(3905));const a={title:"Interceptors",sidebar_position:2},o="Interceptors",s={unversionedId:"extensions/extensions-interceptors",id:"extensions/extensions-interceptors",title:"Interceptors",description:"Interceptors behavior similar to middlewares. They would be executed",source:"@site/docs/extensions/extensions-interceptors.md",sourceDirName:"extensions",slug:"/extensions/extensions-interceptors",permalink:"/clippo/docs/extensions/extensions-interceptors",draft:!1,editUrl:"https://github.com/webinex/clippo/tree/main/docs/docs/extensions/extensions-interceptors.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{title:"Interceptors",sidebar_position:2},sidebar:"tutorialSidebar",previous:{title:"Intro",permalink:"/clippo/docs/extensions/extensions-intro"},next:{title:"Actions",permalink:"/clippo/docs/extensions/extensions-actions"}},l={},p=[{value:"Built-in Interceptors",id:"built-in-interceptors",level:2},{value:"Example",id:"example",level:2}],c={toc:p},d="wrapper";function u(e){let{components:t,...n}=e;return(0,i.kt)(d,(0,r.Z)({},c,n,{components:t,mdxType:"MDXLayout"}),(0,i.kt)("h1",{id:"interceptors"},"Interceptors"),(0,i.kt)("p",null,"Interceptors behavior similar to middlewares. They would be executed\nin reverse order they added and same order after calling ",(0,i.kt)("inlineCode",{parentName:"p"},"next.InvokeAsync(args)"),".\nIf interceptor returns failed CodedResult, than request pipeline interrupted and CodedResult returned.",(0,i.kt)("br",{parentName:"p"}),"\n","Default ClippoController would return failed status code with serialized ",(0,i.kt)("inlineCode",{parentName:"p"},"x-coded-failure")," header.  "),(0,i.kt)("h2",{id:"built-in-interceptors"},"Built-in Interceptors"),(0,i.kt)("ul",null,(0,i.kt)("li",{parentName:"ul"},(0,i.kt)("inlineCode",{parentName:"li"},".AddMaxSize(int sizeInBytes)"),": validates store file content size. Returns ",(0,i.kt)("inlineCode",{parentName:"li"},"INV.CLIPPO.MAX"),"\ncoded failure if content exceed max size"),(0,i.kt)("li",{parentName:"ul"},(0,i.kt)("inlineCode",{parentName:"li"},".AddNoEmpty()"),": validates store file not empty. Returns ",(0,i.kt)("inlineCode",{parentName:"li"},"INV.CLIPPO.NO_EMPTY")," coded\nfailure if content exceed max size"),(0,i.kt)("li",{parentName:"ul"},"EF ",(0,i.kt)("inlineCode",{parentName:"li"},".AddSaveChanges"),": calls ",(0,i.kt)("inlineCode",{parentName:"li"},"dbContext.SaveChangesAsync")," on ",(0,i.kt)("inlineCode",{parentName:"li"},"ApplyAsync")," and ",(0,i.kt)("inlineCode",{parentName:"li"},"StoreAsync")," successful results"),(0,i.kt)("li",{parentName:"ul"},"EF ",(0,i.kt)("inlineCode",{parentName:"li"},".AddSaveChangesWhenUrlPathStartsWith(string path)"),": calls ",(0,i.kt)("inlineCode",{parentName:"li"},"dbContext.SaveChangesAsync")," on ",(0,i.kt)("inlineCode",{parentName:"li"},"ApplyAsync"),"\nand ",(0,i.kt)("inlineCode",{parentName:"li"},"StoreAsync")," successful results when request path starts with ",(0,i.kt)("inlineCode",{parentName:"li"},"path")," segment")),(0,i.kt)("h2",{id:"example"},"Example"),(0,i.kt)("p",null,"For example, interceptor to validate content not exceed max size  "),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-csharp"},"internal class MaxSizeInterceptor<Attachment> : ClippoInterceptor<Attachment>\n{\n    private readonly MaxSizeSettings _maxSizeSettings;\n\n    public MaxSizeInterceptor(MaxSizeSettings maxSizeSettings)\n    {\n        _maxSizeSettings = maxSizeSettings;\n    }\n\n    public override async Task<CodedResult<Attachment[]>> OnStoreAsync(\n        IEnumerable<StoreClipArgs> args,\n        INext<IEnumerable<StoreClipArgs>, CodedResult<Attachment[]>> next)\n    {\n        args = args.ToArray();\n\n        foreach (var arg in args)\n        {\n            if (arg.Content.Value.Length > _maxSizeSettings.Value)\n                return ClippoCodes.Results.MaxSize<Attachment[]>(arg.Content);\n        }\n\n        return await base.OnStoreAsync(args, next);\n    }\n}\n")),(0,i.kt)("p",null,"Register your interceptor"),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-csharp"},"    return services.AddClippo<Attachment>(x =>\n    {\n        x\n            ...\n            .Interceptors\n            .Add<MaxSizeInterceptor>();\n    });\n")))}u.isMDXComponent=!0}}]);