![image](https://github.com/user-attachments/assets/3fc44e76-5c30-4b12-869e-af9eb1cf8d3f)
<!-- <img src="https://sdk.weixin.senparc.com/images/senparc-logo-500.jpg" /> -->

Senparc.Weixin —— WeChat .NET SDK 
=================

[![Build status](https://mysenparc.visualstudio.com/Senparc%20SDK/_apis/build/status/Weixin%20SDK/Senparc.Weixin%20Dev-%E5%86%85%E9%83%A8-%E8%87%AA%E5%8A%A8-.Net6)](https://mysenparc.visualstudio.com/Senparc%20SDK/_build/latest?definitionId=36)
[![NuGet](https://img.shields.io/nuget/dt/Senparc.Weixin.svg)](https://www.nuget.org/packages/Senparc.Weixin)
[![GitHub commit activity the past week, 4 weeks, year](https://img.shields.io/github/commit-activity/4w/JeffreySu/WeiXinMPSDK.svg)](https://github.com/JeffreySu/WeiXinMPSDK/commits/master)
![Static Badge](https://img.shields.io/badge/.NET-10.0-blue)
[![Senparc.Weixin.All](https://img.shields.io/nuget/vpre/Senparc.Weixin.All?label=Senparc.Weixin.All)](https://www.nuget.org/packages/Senparc.Weixin.All/)
[![license](https://img.shields.io/github/license/JeffreySu/WeiXinMPSDK.svg)](http://www.apache.org/licenses/LICENSE-2.0)

[![Senparc.Weixin](https://img.shields.io/nuget/vpre/Senparc.Weixin?label=Senparc.Weixin)](https://www.nuget.org/packages/Senparc.Weixin/)
[![Senparc.Weixin.MP](https://img.shields.io/nuget/vpre/Senparc.Weixin.MP?label=Senparc.Weixin.MP)](https://www.nuget.org/packages/Senparc.Weixin.MP/)
[![Senparc.Weixin.MP.Middleware](https://img.shields.io/nuget/vpre/Senparc.Weixin.MP.Middleware?label=Senparc.Weixin.MP.Middleware)](https://www.nuget.org/packages/Senparc.Weixin.MP.Middleware/)
[![Senparc.Weixin.MP.Mvc](https://img.shields.io/nuget/vpre/Senparc.Weixin.MP.Mvc?label=Senparc.Weixin.MP.Mvc)](https://www.nuget.org/packages/Senparc.Weixin.MP.Mvc/)
[![Senparc.Weixin.WxOpen](https://img.shields.io/nuget/vpre/Senparc.Weixin.WxOpen?label=Senparc.Weixin.WxOpen)](https://www.nuget.org/packages/Senparc.Weixin.WxOpen/)
[![Senparc.Weixin.WxOpen.Middleware](https://img.shields.io/nuget/vpre/Senparc.Weixin.WxOpen.Middleware?label=Senparc.Weixin.WxOpen.Middleware)](https://www.nuget.org/packages/Senparc.Weixin.WxOpen.Middleware/)
[![Senparc.Weixin.Work](https://img.shields.io/nuget/vpre/Senparc.Weixin.Work?label=Senparc.Weixin.Work)](https://www.nuget.org/packages/Senparc.Weixin.Work/)
[![Senparc.Weixin.Work.Middleware](https://img.shields.io/nuget/vpre/Senparc.Weixin.Work.Middleware?label=Senparc.Weixin.Work.Middleware)](https://www.nuget.org/packages/Senparc.Weixin.Work.Middleware/)
[![Senparc.Weixin.TenPay](https://img.shields.io/nuget/vpre/Senparc.Weixin.TenPay?label=Senparc.Weixin.TenPay)](https://www.nuget.org/packages/Senparc.Weixin.TenPay/)
[![Senparc.Weixin.TenPayV3](https://img.shields.io/nuget/vpre/Senparc.Weixin.TenPayV3?label=Senparc.Weixin.TenPayV3)](https://www.nuget.org/packages/Senparc.Weixin.TenPayV3/)
[![Senparc.Weixin.Open](https://img.shields.io/nuget/vpre/Senparc.Weixin.Open?label=Senparc.Weixin.Open)](https://www.nuget.org/packages/Senparc.Weixin.Open/)
[![Senparc.Weixin.AspNet](https://img.shields.io/nuget/vpre/Senparc.Weixin.AspNet?label=Senparc.Weixin.AspNet)](https://www.nuget.org/packages/Senparc.Weixin.AspNet/)
[![Senparc.Weixin.Cache.Redis](https://img.shields.io/nuget/vpre/Senparc.Weixin.Cache.Redis?label=Senparc.Weixin.Cache.Redis)](https://www.nuget.org/packages/Senparc.Weixin.Cache.Redis/)
[![Senparc.Weixin.Cache.CsRedis](https://img.shields.io/nuget/vpre/Senparc.Weixin.Cache.CsRedis?label=Senparc.Weixin.Cache.CsRedis)](https://www.nuget.org/packages/Senparc.Weixin.Cache.CsRedis/)
[![Senparc.Weixin.Cache.Memcached](https://img.shields.io/nuget/vpre/Senparc.Weixin.Cache.Memcached?label=Senparc.Weixin.Cache.Memcached)](https://www.nuget.org/packages/Senparc.Weixin.Cache.Memcached/)
[![Senparc.Weixin.Cache.Dapr](https://img.shields.io/nuget/vpre/Senparc.Weixin.Cache.Dapr?label=Senparc.Weixin.Cache.Dapr)](https://www.nuget.org/packages/Senparc.Weixin.Cache.Dapr/)
[![Senparc.WebSocket](https://img.shields.io/nuget/vpre/Senparc.WebSocket?label=Senparc.WebSocket)](https://www.nuget.org/packages/Senparc.WebSocket/)




[[中文]](readme.md)

> 📈 **Senparc.Weixin SDK is currently the most widely used WeChat .NET SDK and one of the most popular .NET open-source projects in China. This project has been continuously maintained for over 12 years and has powered a large number of successful systems and applications. We will continue iterating, and are deeply integrating [AI](https://github.com/Senparc/Senparc.AI) scenarios with more samples coming online. Welcome to join our community 👐**

With Senparc.Weixin, you can quickly build applications across the entire WeChat ecosystem, including Official Accounts, Mini Programs, Mini Games, Enterprise WeChat, Open Platform, WeChat Pay, JS-SDK, WeChat hardware/Bluetooth, and more. The samples in this repository are also suitable for .NET beginners.
  
Currently, Senparc.Weixin supports almost all WeChat modules and APIs, and supports multiple frameworks including [.NET 3.5 / 4.0 / 4.5 / .NET Standard 2.x / .NET Core 2.x / .NET Core 3.x / .NET 6.0 / .NET 7.0 / .NET 8.0 / .NET 10.0](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer). It is compatible with MVC, Razor, WebApi, Console, desktop apps (.exe), Blazor, MAUI, background services, and more, while remaining fully decoupled from external frameworks.  
  
Since the project started in 2013, we have kept it continuously updated for over 12 years, and have shared complete source code and design ideas without reservation. We hope more developers can benefit from it, spread the open-source spirit, and help advance open source in China. Sincere thanks to everyone who has supported us along the way.  
  
If you like this project and want us to continue improving it, please give us a ★Star :)  
  
## 🔔 Announcement  

> [!TIP]
> 🥇 [Senparc Developer Community 2025-2026 Contributor Ranking](https://github.com/JeffreySu/WeiXinMPSDK/wiki/%E7%9B%9B%E6%B4%BE%E5%BC%80%E5%8F%91%E8%80%85%E7%A4%BE%E5%8C%BA-2025%E2%80%902026-%E5%B9%B4%E5%BA%A6%E8%B4%A1%E7%8C%AE%E8%80%85%E6%A6%9C%E5%8D%95)<br>
> ⚡ .NET 10 stable is now released. Latest [Sample for .NET 10](./Samples/All/net10-mvc/) (backward compatible), online demo: https://sdk.weixin.senparc.com/<br>
> 🔥 AI chatbot WeChat integration sample is now online! [View](/Samples%20with%20AI)<br>
> 🎬 `Scott Hanselman interview on AI topics` [Watch video](https://www.bilibili.com/video/BV1NE421K78D/)<br/>
> [!NOTE]
> 🔒 [WeChat Pay V3 module (V1.0)](https://github.com/JeffreySu/WeiXinMPSDK/tree/master/src/Senparc.Weixin.TenPay/Senparc.Weixin.TenPayV3) is online! [NuGet](https://www.nuget.org/packages/Senparc.Weixin.TenPayV3)<br>
> 🎠 Fully supports automatic long-text chunking and sending. More: [Auto-replying Extra-long Messages for GenAI Applications](https://www.cnblogs.com/szw/p/weixin-large-text-response.html)<br/>
  
<!-- _1. In order to isolate the demo from the source code and make it easier for everyone to find the demo, the Senparc.Weixin.MP.Sample and other folders have been moved to the [/Samples/](/Samples/) folder._<br>  
_2. The `Senparc.Weixin.Plugins` plan has been launched, details [click here](https://github.com/JeffreySu/WeiXinMPSDK/tree/master/Plugins)._ -->  
  
  
## 🌟 Start: Module-by-Module Docs + Runnable Samples  
  
Module | Link  
----|----  
Official Account | https://sdk.weixin.senparc.com/Docs/MP/  
Mini Program | https://sdk.weixin.senparc.com/Docs/WxOpen/  
Enterprise WeChat | https://sdk.weixin.senparc.com/Docs/Work/  
WeChat Pay V3 (recommended) | https://sdk.weixin.senparc.com/Docs/TenPayV3/  
WeChat Pay V2 (not recommended) | https://sdk.weixin.senparc.com/Docs/TenPayV2/  
  
> [!NOTE]
> 1. Each module page above includes both docs and immediately runnable code templates (you only need to fill WeChat settings, no code changes required).<br>
> 2. Configuration, registration, and API invocation patterns are consistent across modules. Learn one module and you can quickly apply the same approach to others.<br>
> 3. The [/docs](/docs/) directory provides more complete guidance for advanced development, [click here](/docs/). <br>
> 4. Senparc.Weixin SDK modules are fully decoupled and independently published. To simplify dependencies, you can directly use [Senparc.Weixin.All](https://www.nuget.org/packages/Senparc.Weixin.All) to reference all modules automatically.
  
## 🚀 Hello World: Start Your WeChat Development Journey in 3 Lines  
  
> [!NOTE]
> 1. The sample source code below is in [`/Samples/MP/Senparc.Weixin.Sample.MP`](/Samples/MP/Senparc.Weixin.Sample.MP), using Official Account as an example. Once you know this flow, you can apply the same pattern to Mini Program, Enterprise WeChat, WeChat Pay, etc.<br>
> 2. For other module or integrated demos, see standalone samples under [`/Samples/`](/Samples/) or integrated/advanced samples under [`/Samples/All/`](/Samples/All/).
  
  
### Startup code (only 2 lines):
1. <strong>Add configuration above `builder.Build()` in Program.cs:</strong>  
``` C#  
builder.Services.AddSenparcWeixinServices(builder.Configuration);  
```  
> If you are using legacy `Startup.cs`, this line belongs in `ConfigureServices()`.
  
2. <strong>Enable configuration below `builder.Build()` in Program.cs:</strong>  
``` C#  
var registerService = app.UseSenparcWeixin(app.Environment, null, null, register => { },  
    (register, weixinSetting) =>  
{  
    // Register Official Account information (can be executed multiple times to register multiple Official Accounts)  
    register.RegisterMpAccount(weixinSetting, "Senparc Network Assistant Official Account");  
});  
```  
> - If you are using legacy `Startup.cs`, this block belongs in `Configure()`.
> - If you want auto-registration for all configured accounts, append `autoRegisterAllPlatforms: true` (requires `Senparc.Weixin.All`):
> ```C#
> var registerService = app.UseSenparcWeixin(app.Environment, null, null, register => { },
>     (register, weixinSetting) => { /* no manual registration needed */ },
>     autoRegisterAllPlatforms: true /* auto-register all platforms */
> );
> ```
  
### Call advanced APIs (only 1 line):
You can call APIs anywhere in your program (customer service API as an example):
``` C#  
await CustomApi.SendTextAsync("AppId", "OpenId", "Hello World!");  
```  
> [!TIP]
> 1. Senparc.Weixin SDK automatically manages AccessToken through the full lifecycle. During development, you only need AppId and do not need to handle token expiration manually.<br>
> 2. Registration information such as AppId can be automatically obtained from `Senparc.Weixin.Config.SenparcWeixinSetting`, and the relevant parameters are configured in `appsettings.json`.  
> 3. A synchronous version is also available: `Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText()`.<br>
> 4. Namespace and parameter naming follow official API documentation conventions as closely as possible (especially return fields), making code lookup and testing faster while reducing bug risks.

> [!TIP]
> At this point, you can already apply the same pattern to all WeChat modules.
  
### How to use Official Account messaging?
Official Accounts provide a built-in chat window for text, image, voice, and other interactions.
  
The same pattern also applies to Enterprise WeChat and Mini Program customer service messaging. Only two steps:
  
#### Step 1: Create a custom MessageHandler to control message processing logic:  
  
<details>  
<summary>CustomMessageHandler.cs</summary>  
  
  
``` C#  
using Senparc.NeuChar.Entities;  
using Senparc.Weixin.MP.Entities;  
using Senparc.Weixin.MP.Entities.Request;  
using Senparc.Weixin.MP.MessageContexts;  
using Senparc.Weixin.MP.MessageHandlers;  
  
namespace Senparc.Weixin.Sample.MP  
{  
    /// <summary>  
    /// Custom MessageHandler  
    /// Inherits from MessageHandler and overrides the corresponding request handling methods  
    /// </summary>  
    public partial class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>  
    {  
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0,  
            bool onlyAllowEncryptMessage = false, IServiceProvider serviceProvider = null)  
            : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, null, serviceProvider)  
        {  
        }  
  
        /// <summary>  
        /// Default message for all unhandled types  
        /// </summary>  
        /// <returns></returns>  
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)  
        {  
            //ResponseMessageText can also be News or other types  
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();  
            responseMessage.Content = $"You sent a message, but the program did not specify a processing procedure";  
            return responseMessage;  
        }  
  
        public override Task<IResponseMessageBase> OnImageRequestAsync(RequestMessageImage requestMessage)  
        {  
            //Handle image requests...  
        }  
  
        public override Task<IResponseMessageBase> OnLocationRequestAsync(RequestMessageLocation requestMessage)  
        {  
            //Handle location requests...  
        }  
    }  
}  
```  
</details>  
  
  
#### Step 2: Request the CustomMessageHandler:  
  
We provide two ways to request the CustomMessageHandler: `Middleware` (recommended) and `Controller (or WebApi)`. You can choose either one. Taking Middleware as an example, after enabling the configuration in Program.cs, add the following code to register the MessageHandler:  
``` C#  
app.UseMessageHandlerForMp("/WeixinAsync",  
    (stream, postModel, maxRecordCount, serviceProvider)  
        => new CustomMessageHandler(stream, postModel, maxRecordCount, false, serviceProvider),  
    options   
        =>  
    {  
        options.AccountSettingFunc = context => Senparc.Weixin.Config.SenparcWeixinSetting;  
    });  
```  
  
At this point, you can use [https://YourDomain/WeixinAsync](https://sdk.weixin.senparc.com/WeixinAsync) to configure your WeChat Official Account backend in [Settings and Development] > [Basic Configuration] > [Server Address (URL)], and set the Token in [appsettings.json](/Samples/MP/Senparc.Weixin.Sample.MP/appsettings.json#L36) (also applies to Enterprise WeChat and Mini Program; see the corresponding [Samples](/Samples/)).  
  
In addition, you can also use the `Controller (or WebApi)` method to have more precise control over the entire message processing process (or use it in .NET Framework), [click here to view](https://github.com/JeffreySu/WeiXinMPSDK/wiki/%E5%A6%82%E4%BD%95%E4%BD%BF%E7%94%A8MessageHandler%E7%AE%80%E5%8C%96%E6%B6%88%E6%81%AF%E5%A4%84%E7%90%86%E6%B5%81%E7%A8%8B).  
  
Now you have mastered the basic skills required for WeChat platform development. Keep reading for more resources:  
  
<img src="https://sdk.weixin.senparc.com/images/SenparcRobotsnapshoot.jpg" width="300" align="right">  
  
## 📇 More Indexes

* [🏹 Libraries by Module](#-libraries-by-module)
* [💾 Explanation of Source Code Project Folders (under the src folder)](#-explanation-of-source-code-project-folders-under-the-src-folder)
* [🖥️ Explanation of Samples Folder (under the Samples folder)](#%EF%B8%8F-explanation-of-samples-folder-under-the-samples-folder)
* [🎨 Resources](#-resources)
* [📖 Senparc Official Book Tutorial](#-senparc-official-book-tutorial)
* [💻 Senparc Official Video Tutorial](#-senparc-official-video-tutorial)
* [🧪 Follow the test account to experience the functions (SenparcRobot)](#-follow-the-test-account-to-experience-the-functions-senparcrobot)
* [✋ Contribute Code](#-contribute-code)
* [👩‍🏫 How to develop with .NET Core](#-how-to-develop-with-net-core)
* [↕️ Install via NuGet](#%EF%B8%8F-install-via-nuget-to-the-project)
* [🏬 Deployment](#-deployment-guide)
* [🍴 Important Branches](#-important-branches)
* [🍟 Thanks to Contributors](#-thanks-to-contributors)
* [💰 Donate](#-donations)
* [⭐ Star Quantity Statistics](#-star-quantity-statistics)
* [📊 Project Operations Statistics](#-project-operations-statistics)
* [📎 License](#-license)

This repository includes source code for .NET Framework / .NET Standard 2.0+ / .NET Core 3.1 / .NET 6 / .NET 7 / .NET 8 / .NET 10 (same core logic):

* Open demo with Visual Studio 2022 (all versions): .NET 10.0 - [Senparc.Weixin.Sample.Net10.sln](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer-.NET10/Samples/All/net10-mvc) (recommended, with source references)
* Open demo with Visual Studio 2022 (all versions): .NET 8.0 - [Senparc.Weixin.Sample.Net8.sln](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/Samples/All/net8-mvc) (with source references)
* Open .NET Framework demo with Visual Studio 2019+ : [Senparc.Weixin.MP.Sample.Net45.sln](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/Samples/All/net45-mvc/) (package references only)
* Open command-line Console demo with Visual Studio 2019+ (.NET Core): [Senparc.Weixin.MP.Sample.Consoles.vs2019.sln](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/Samples/All/console)

## 🏹 Libraries by Module

| # | Module | DLL | NuGet | Supported .NET |
|---|---|---|---|---|
| 1 | Core library | Senparc.Weixin.dll | [![Senparc.Weixin][1.1]][1.2]<br>[![Senparc.Weixin][nuget-img-base]][nuget-url-base] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 2 | Official Account /<br>JSSDK / Shake Around / etc. | Senparc.Weixin.MP.dll | [![MP][2.1]][2.2]<br>[![MP][nuget-img-mp]][nuget-url-mp] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 3 | [Mini Program<br>(incl. Mini Games)<br>(independent project)](https://github.com/JeffreySu/WxOpen) | Senparc.Weixin.WxOpen.dll | [![WxOpen][9.1]][9.2]<br>[![WxOpen][nuget-img-wxopen]][nuget-url-wxopen] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 4 | WeChat Pay | Senparc.Weixin.TenPay.dll | [![TenPay][12.1]][12.2]<br>[![TenPay][nuget-img-tenpay]][nuget-url-tenpay] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 5 | WeChat Pay V3 | Senparc.Weixin.TenPayV3.dll | [![TenPayV3][13.1]][13.2]<br>[![TenPay][nuget-img-tenpayv3]][nuget-url-tenpayv3] | ![.NET 4.5][net46N] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 6 | ASP.NET MVC extension | Senparc.Weixin.MP.MVC.dll | [![MP.MVC][3.1]][3.2]<br>[![Mvc][nuget-img-mvc]][nuget-url-mvc] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 7 | Enterprise Account<br>(officially discontinued) | Senparc.Weixin.QY.dll | [![QY][4.1]][4.2]<br>[![QY][nuget-img-qy]][nuget-url-qy] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 9 | Enterprise WeChat | Senparc.Weixin.Work.dll | [![Work][5.1]][5.2]<br>[![Work][nuget-img-work]][nuget-url-work] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 9 | WeChat Open Platform | Senparc.Weixin.Open.dll | [![Open][6.1]][6.2]<br>[![Open][nuget-img-open]][nuget-url-open] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 10 | Redis distributed cache | Senparc.Weixin.Cache.<br>Redis.dll | [![Cache.Redis][7.1]][7.2]<br>[![Redis][nuget-img-redis]][nuget-url-redis] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 11 | Memcached<br>distributed cache | Senparc.Weixin.Cache.<br>Memcached.dll | [![Cache.Memcached][8.1]][8.2]<br>[![MC][nuget-img-mc]][nuget-url-mc] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 12 | [WebSocket<br>(independent project)](https://github.com/JeffreySu/Senparc.WebSocket) | Senparc.WebSocket.dll | [![Senparc.WebSocket][10.1]][10.2]<br>[![WebSocket][nuget-img-ws]][nuget-url-ws] | ![.NET 4.5][net46Y] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |
| 13 | All-in-One package | Senparc.Weixin.All.dll | [![Senparc.Weixin.All][14.1]][14.2]<br>[![Senparc.Weixin.All][nuget-img-base]][nuget-url-base] | ![.NET 4.5][net46N] ![.NET Standard 2.0/2.1][core20Y] ![.NET 10.0][net10] |

#### Legend
| ![.NET 4.6][net46Y] | ![.NET Standard 2.x][core20Y] | ![.NET 10.0][net10] |
|--|--|--|
| .NET Framework 4.6.2+ | .NET Standard 2.0 / 2.1 | .NET 10.0, backward compatible with .NET 5.0-9.0 |

> [!WARNING]
> 1. Since May 1, 2019, .NET Framework 3.5 and 4.0 are no longer updated. The last stable version for .NET Framework 3.5 + 4.0 is available [here](https://github.com/JeffreySu/WeiXinMPSDK/releases/tag/v16.6.15).<br>
> 2. Since April 3, 2022, .NET Framework 4.5 has been upgraded to 4.6.2. The last stable version for .NET Framework 4.5 is available [here](https://github.com/JeffreySu/WeiXinMPSDK/releases/tag/v16.17.9).<br>
> 3. If you still use .NET Framework, we recommend upgrading to .NET Framework 4.8+ by January 12, 2027. Official support for .NET Framework 4.6.2 ends then ([see details](https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-framework)).<br>
> 4. Use `Senparc.Weixin.All` to reference all modules at once.

[1.1]: https://img.shields.io/nuget/v/Senparc.Weixin.svg?style=flat
[1.2]: https://www.nuget.org/packages/Senparc.Weixin
[2.1]: https://img.shields.io/nuget/v/Senparc.Weixin.MP.svg?style=flat
[2.2]: https://www.nuget.org/packages/Senparc.Weixin.MP
[3.1]: https://img.shields.io/nuget/v/Senparc.Weixin.MP.MVC.svg?style=flat
[3.2]: https://www.nuget.org/packages/Senparc.Weixin.MP.MVC
[4.1]: https://img.shields.io/nuget/v/Senparc.Weixin.QY.svg?style=flat
[4.2]: https://www.nuget.org/packages/Senparc.Weixin.QY 
[5.1]: https://img.shields.io/nuget/v/Senparc.Weixin.Work.svg?style=flat
[5.2]: https://www.nuget.org/packages/Senparc.Weixin.Work
[6.1]: https://img.shields.io/nuget/v/Senparc.Weixin.Open.svg?style=flat
[6.2]: https://www.nuget.org/packages/Senparc.Weixin.Open
[7.1]: https://img.shields.io/nuget/v/Senparc.Weixin.Cache.Redis.svg?style=flat
[7.2]: https://www.nuget.org/packages/Senparc.Weixin.Cache.Redis
[8.1]: https://img.shields.io/nuget/v/Senparc.Weixin.Cache.Memcached.svg?style=flat
[8.2]: https://www.nuget.org/packages/Senparc.Weixin.Cache.Memcached
[9.1]: https://img.shields.io/nuget/v/Senparc.Weixin.WxOpen.svg?style=flat
[9.2]: https://www.nuget.org/packages/Senparc.Weixin.WxOpen
[10.1]: https://img.shields.io/nuget/v/Senparc.WebSocket.svg?style=flat
[10.2]: https://www.nuget.org/packages/Senparc.WebSocket
[11.1]: https://img.shields.io/nuget/v/Senparc.Weixin.MP.CoreMVC.svg?style=flat
[11.2]: https://www.nuget.org/packages/Senparc.Weixin.MP.CoreMVC
[12.1]: https://img.shields.io/nuget/v/Senparc.Weixin.TenPay.svg?style=flat
[12.2]: https://www.nuget.org/packages/Senparc.Weixin.TenPay
[13.1]: https://img.shields.io/nuget/v/Senparc.Weixin.TenPayV3.svg?style=flat
[13.2]: https://www.nuget.org/packages/Senparc.Weixin.TenPayV3
[14.1]: https://img.shields.io/nuget/v/Senparc.Weixin.All.svg?style=flat
[14.2]: https://www.nuget.org/packages/Senparc.Weixin.All

[net46Y]: https://img.shields.io/badge/.NET%20Framework%204.6+-Y-brightgreen.svg
[net46N]: https://img.shields.io/badge/.NET%20Framework%204.6+-N-lightgrey.svg
[net46N-]: https://img.shields.io/badge/.NET%20Framework%204.6+----lightgrey.svg
[core20Y]: https://img.shields.io/badge/.NET%20Standard2.x-Y-brightgreen.svg
[net8]: https://img.shields.io/badge/.NET%208.0-Y-brightgreen.svg
[net10]: https://img.shields.io/badge/.NET%2010.0-Y-brightgreen.svg


[nuget-img-base]: https://img.shields.io/nuget/dt/Senparc.Weixin.svg
[nuget-url-base]: https://www.nuget.org/packages/Senparc.Weixin
[nuget-img-mp]: https://img.shields.io/nuget/dt/Senparc.Weixin.MP.svg
[nuget-url-mp]: https://www.nuget.org/packages/Senparc.Weixin.MP
[nuget-img-mvc]: https://img.shields.io/nuget/dt/Senparc.Weixin.MP.Mvc.svg
[nuget-url-mvc]: https://www.nuget.org/packages/Senparc.Weixin.MP.Mvc
[nuget-img-tenpay]: https://img.shields.io/nuget/dt/Senparc.Weixin.TenPay.svg
[nuget-url-tenpay]: https://www.nuget.org/packages/Senparc.Weixin.TenPay
[nuget-img-tenpayv3]: https://img.shields.io/nuget/dt/Senparc.Weixin.TenPayV3.svg
[nuget-url-tenpayv3]: https://www.nuget.org/packages/Senparc.Weixin.TenPayV3
[nuget-img-qy]: https://img.shields.io/nuget/dt/Senparc.Weixin.QY.svg
[nuget-url-qy]: https://www.nuget.org/packages/Senparc.Weixin.QY
[nuget-img-work]: https://img.shields.io/nuget/dt/Senparc.Weixin.Work.svg
[nuget-url-work]: https://www.nuget.org/packages/Senparc.Weixin.Work
[nuget-img-open]: https://img.shields.io/nuget/dt/Senparc.Weixin.Open.svg
[nuget-url-open]: https://www.nuget.org/packages/Senparc.Weixin.Open
[nuget-img-redis]: https://img.shields.io/nuget/dt/Senparc.Weixin.Cache.Redis.svg
[nuget-url-redis]: https://www.nuget.org/packages/Senparc.Weixin.Cache.Redis
[nuget-img-mc]: https://img.shields.io/nuget/dt/Senparc.Weixin.Cache.Memcached.svg
[nuget-url-mc]: https://www.nuget.org/packages/Senparc.Weixin.Cache.Memcached
[nuget-img-wxopen]: https://img.shields.io/nuget/dt/Senparc.Weixin.WxOpen.svg
[nuget-url-wxopen]: https://www.nuget.org/packages/Senparc.Weixin.WxOpen
[nuget-img-ws]: https://img.shields.io/nuget/dt/Senparc.WebSocket.svg
[nuget-url-ws]: https://www.nuget.org/packages/Senparc.WebSocket


## Feature Support  
  
* Most of the WeChat 8.x APIs are supported, including WeChat Pay, custom menu/personalized menu, template message interface, material upload interface, mass message interface, customer service interface, payment interface, WeChat store interface, card interface, invoice interface, etc.  
* Support for WeChat Official Accounts, Mini Programs, Enterprise Accounts, Open Platforms, WeChat Pay, and other modules.  
* Support for user session context (solving the problem of using Session to process user information on the server).  
* Full support for the latest APIs of WeChat Official Accounts, Mini Programs, Enterprise Accounts (WeChat Work), WeChat Pay V2/V3, and Open Platforms.  
* Support for distributed caching and caching strategy extension (default support: local cache, Redis, Memcached, can be freely extended), no need to worry about the type of cache used during development, can be freely switched in the configuration file or during runtime.  
  
> 1. The official APIs are perfectly integrated, and all upgrades will try to ensure backward compatibility unless otherwise specified. So you can safely use or directly upgrade (overwrite) the latest DLLs. It is recommended to use [NuGet](https://www.nuget.org/) for updates.<br>  
> 2. You can also modify and compile the code yourself. Open the [Senparc.Weixin.Sample.Net8.sln](/Samples/All/net8-mvc/) solution to see all the source code. When the compilation mode is `Release`, a local NuGet package will be automatically generated (default generated in the `/src/BuildOutPut/` folder).  
  
## 💾 Explanation of Source Code Project Folders (under the src folder)  
  
<details>  
<summary>Expand</summary>  
  
  
| Folder | Description |  
|--------|--------|  
|[Senparc.WebSocket](src/Senparc.WebSocket/)|WebSocket module|  
|[Senparc.Weixin.Cache](src/Senparc.Weixin.Cache)							|Senparc.Weixin.Cache.Memcached.dll, Senparc.Weixin.Cache.Redis.dll, and other distributed cache extension solutions|  
|[Senparc.Weixin.AspNet](src/Senparc.Weixin.AspNet)							|Senparc.Weixin.AspNet.dll, a class library specifically for web support|  
|[Senparc.Weixin.MP.MvcExtension](src/Senparc.Weixin.MP.MvcExtension)		|Senparc.Weixin.MP.MvcExtension.dll source code, an extension package for MVC projects |  
|[Senparc.Weixin.MP](src/Senparc.Weixin.MP)									|Senparc.Weixin.MP.dll WeChat Official Account SDK source code|  
|[Senparc.Weixin.MP.Middleware](src/Senparc.Weixin.MP.Middleware)           |Senparc.Weixin.MP.Middleware.dll WeChat Official Account message middleware source code|  
|[Senparc.Weixin.Open](src/Senparc.Weixin.Open)								|Senparc.Weixin.Open.dll Third-party Open Platform SDK source code|  
|[Senparc.Weixin.TenPay](src/Senparc.Weixin.TenPay)							|Senparc.Weixin.TenPay.dll & Senparc.Weixin.TenPayV3.dll source code for WeChat Pay [V2](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/src/Senparc.Weixin.TenPay/Senparc.Weixin.TenPay) and [V3](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/src/Senparc.Weixin.TenPay/Senparc.Weixin.TenPayV3)|  
|[Senparc.Weixin.Work](src/Senparc.Weixin.Work)								|Senparc.Weixin.Work.dll Enterprise WeChat SDK source code|  
|[Senparc.Weixin.Work.Middleware](src/Senparc.Weixin.Work.Middleware)       |Senparc.Weixin.Work.Middleware.dll Enterprise WeChat message middleware source code|  
|[Senparc.Weixin.WxOpen](src/Senparc.Weixin.WxOpen)							|Senparc.Weixin.WxOpen.dll WeChat Mini Program SDK source code, including Mini Games|  
|[Senparc.Weixin.WxOpen.Middleware](src/Senparc.Weixin.WxOpen.Middleware)   |Senparc.Weixin.WxOpen.Middleware.dll WeChat Mini Program message middleware source code, including Mini Games|  
|[Senparc.Weixin](src/Senparc.Weixin)										|Source code for all Senparc.Weixin.[x].dll basic libraries|  
</details>  
  
[Enter Folder](/src/)  
  
## 🖥️ Explanation of Samples Folder (under the Samples folder)  
  
The usage of all modules in the Senparc.Weixin SDK is highly consistent, including the configuration process, AccessToken management, message processing, service messages, API calls, etc. You only need to refer to the usage of any module (it is recommended to start with `Official Accounts` or `Mini Programs`), and you can apply the same principles to other modules.  
  
From the following samples, you can learn about the configuration and usage of each independent module. Just open the `.sln` solution file in the corresponding folder to view the source code and run it to see the documentation. The `All` folder contains more comprehensive and advanced feature demonstrations.  
  
| Folder | Description | SDK Reference Method |  
|--------|--------|----|  
|[MP](/Samples/MP/)          |   Official Accounts | NuGet Package  
|[TenPayV2](/Samples/TenPayV2/)    |   WeChat Pay V1 and V2  | NuGet Package  
|[TenPayV3](/Samples/TenPayV3/)    |   WeChat Pay V3 (TenPay APIv3) | NuGet Package  
|[Work](/Samples/Work/)        |   Enterprise Accounts | NuGet Package  
|[WxOpen](/Samples/WxOpen/)      |   Mini Programs | NuGet Package  
|[Shared](/Samples/Shared)      |   Shared files required by all samples  
|[All](/Samples/All/)         |   A mixed scenario demonstration that includes all functions of WeChat Official Accounts, Mini Programs, WeChat Pay, Enterprise Accounts, etc., recommended for projects that integrate multiple platforms or require deep development (advanced) |   
| ┣ [All/console](/Samples/All/console)			|Command Line Console Demo (.NET Core)| NuGet Package  
| ┣ [All/net45-mvc](/Samples/All/net45-mvc)						|Demo that can be directly published and used (.NET Framework 4.5 + ASP.NET MVC)|  NuGet Package  
| ┗ [All/net10-mvc](/Samples/All/net10-mvc)			|Demo ready for production use (.NET 10.0), compatible with .NET 5.0, 6.0, 7.0, 8.0, and .NET Core | <strong>Source Code (Latest)</strong>  
| ┗ [All/net8-mvc](/Samples/All/net8-mvc)			|Demo ready for production use (.NET 8.0), compatible with .NET 5.0, 6.0, 7.0, and .NET Core | <strong>Source Code (Latest)</strong>  
  
[Enter Samples Folder](/Samples/)  
  
<!-- ## Customize Your WeChat Project Sample  
  
Web version: [Click here](https://www.cnblogs.com/szw/p/WeChatSampleBuilder-V2.html#Web-WeChatSampleBuilder) for the tutorial.  
  
<img src="https://sdk.weixin.senparc.com/images/WeChatSampleBuilder-v0.2.0-web.png?t=1" width="700" alt="WeChatSampleBuilder" /> -->  
  
<!--   
2. Desktop version: Log in to [https://weixin.senparc.com/User](https://weixin.senparc.com/User) to download the WeChatSampleBuilder tool and view the instructions.  
  
<img src="https://sdk.weixin.senparc.com/images/WeChatSampleBuilder-v0.2.0.png?t=1" width="700" alt="WeChatSampleBuilder" />  
-->  
<!-- > Note: Using the WeChatSampleBuilder tool is only for simplifying the sample code for testing and learning purposes. It cannot help you generate complete production projects with business logic. To build a production project, please refer to the complete demos or other tutorials. It is recommended to use existing system frameworks for project construction, such as [NeuCharFramework](https://github.com/NeuCharFramework/NCF). -->  
  
  
## 🎨 Resources  
  
1. Official website: https://weixin.senparc.com/  
2. Online demo (for .NET 10.0, backward compatible with .NET 6.0, 7.0, 8.0, and .NET Core): https://sdk.weixin.senparc.com/  
3. WeChat development tutorials: https://www.cnblogs.com/szw/p/weixin-course-index.html  
4. WeChat technical community: https://weixin.senparc.com/QA  
5. Custom menu online editor: https://sdk.weixin.senparc.com/Menu  
6. Online message testing tool: https://sdk.weixin.senparc.com/SimulateTool  
7. Cache testing tool: https://sdk.weixin.senparc.com/Cache/Test  
8. chm help document download: https://sdk.weixin.senparc.com/Document  
9. Source code and latest updates: https://github.com/JeffreySu/WeiXinMPSDK  
10. WeChat development resource collection: https://github.com/JeffreySu/WeixinResource  
11. Auxiliary system for reading "In-depth Analysis of WeChat Development": https://book.weixin.senparc.com  
12. Purchase "In-depth Analysis of WeChat Development": [https://item.jd.com/12220004.html](https://book.weixin.senparc.com/book/link?code=github-homepage-resource)  
13. "Rapid Development of WeChat Official Accounts and Mini Programs" video tutorial: [https://github.com/JeffreySu/WechatVideoCourse](https://github.com/JeffreySu/WechatVideoCourse)  
  
* Technical communication QQ groups:  
  
>Group 1 (Official Accounts): 300313885<br>  
>Group 14 (Video Course Students): 588231256<br>  
>Group 10 (Distributed Cache): 246860933<br>  
>Group 12 (Mini Programs): 108830388<br>  
>Group 16 (Open Platform): 860626938<br>  
>*`The following groups are full:`*<br>  
>`Group 2: 293958349 (Full), Group 3: 342319110 (Full)`<br>  
>`Group 4: 372212092 (Full), Group 5: 377815480 (Full), Group 6: 425898825 (Full)`<br>  
>`Group 7: 482942254 (Full), Group 8: 106230270 (Full), Group 9: 539061281 (Full)`<br>  
>`Group 11: 553198593 (Full), Group 13: 183424136 (Open Platform, Full), Group 15: 289181996 (Full)`<br>  
  
* Business contact QQ: 498977166  
  
<!-- * Sina Weibo: [@苏震巍](http://weibo.com/jeffreysu1984) -->  
  
If this project is helpful to you, we welcome any form of donation or participation in code updates and feedback. Thank you!  
  
Donation: [Enter](http://sdk.weixin.senparc.com#donate)

## 📖 Senparc Official Book Tutorial  
  
<img src="https://sdk.weixin.senparc.com/images/book-cover-front-small-3d.png" width="400" align="right">  
  
> The WeChat development book, titled "In-Depth Analysis of WeChat Development: Efficient Development Secrets for Official Accounts and Mini Programs," completed by Jeffrey Su and the Senparc team after 2 years of effort, has been published. The book comes with an auxiliary reading system: [BookHelper](http://book.weixin.senparc.com).<br>  
> Welcome to purchase the genuine book: [【Buy Genuine】](https://book.weixin.senparc.com/book/link?code=github-homepage)<br>  
> The code snapshot of the book's publication version is in the branch [BookVersion1](https://github.com/JeffreySu/WeiXinMPSDK/tree/BookVersion1).  
  
  
## 💻 Senparc Official Video Tutorial  
  
> In order to help everyone understand WeChat development details more intuitively and learn practical techniques in .NET development, we established the "Senparc Classroom" group and launched WeChat development video courses, covering the following two parts:<br>  
> 1. WeChat development fundamentals<br>  
> 2. Case study of official accounts and mini programs<br>  
>   
> The total course duration is 60 lessons, with additional episodes.<br>  
> Currently, the videos are available on NetEase Cloud Classroom, with well-produced content and abundant materials. The course has been selected as an "A" level course. [【Watch Videos】](https://book.weixin.senparc.com/book/videolinknetease?code=github-homepage), [【View Course Code and Slides】](https://github.com/JeffreySu/WechatVideoCourse).  
  
  
## 🧪 Follow the test account to experience the functions (SenparcRobot):  
  
|Senparc Network Assistant Official Account|Senparc Network Assistant Mini Program|BookHelper|  
|--|--|--|  
| <img width="258" height="258" alt="image" src="https://github.com/user-attachments/assets/35aa4952-13fd-4cf1-b81f-43f053ff631c" /> | <img width="258" height="430" alt="image" src="https://github.com/user-attachments/assets/07b5f2b2-54ed-455d-9e17-0ae67ce56038" />  | <img width="258" height="716" alt="image" src="https://github.com/user-attachments/assets/7a1b8231-5cca-4742-bc12-4cb2a83c7b40" /> |  
  
  
## ✋ Contribute Code  
  
> If you need to use or modify the source code of this project, it is recommended to Fork first. You are also welcome to submit a Pull Request for the general version you modified.  
  
1. Fork  
2. Create your feature branch (`git checkout -b my-new-feature`)  
3. Commit your changes (`git commit -am 'Added some feature'`)  
4. Push to the remote `git` repository (`git push origin my-new-feature`)  
5. Go to the `my-new-feature` branch of your `git` remote repository on the github website and submit a Pull Request  
(Submit to the `Developer` branch instead of the `master` branch directly)  
  
<!--  
### /Controllers/WeixinController.cs  
  
The Token below needs to be synchronized with the Token set in the Wechat official platform backend. If it is frequently changed, it is recommended to write it into the Web.config or other configuration files (In actual use, it is recommended to use numbers + English letters in different cases to rewrite the Token. Once the Token is cracked, Wechat requests will be easily forged!):  
```C#  
public readonly string Token = "weixin";  
```  
The following Action (Get) is used to receive and return the verification result of the Wechat backend URL. No modification is required. The address is like: http://domain/Weixin or http://domain/Weixin/Index  
```C#  
/// <summary>  
/// Wechat backend verification address (using Get), the "Interface Configuration Information" Url of Wechat backend is filled as: http://weixin.senparc.com/weixin  
/// </summary>  
[HttpGet]  
[ActionName("Index")]  
public ActionResult Get(PostModel postModel, string echostr)  
{  
    if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))  
    {  
        return Content(echostr); // If a random string is returned, it means the verification is passed  
    }  
    else  
    {  
        return Content("failed:" + postModel.Signature + ","   
            + MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +  
            "If you see this sentence in the browser, it means that this address can be used as the Url of the Wechat official account backend. Please keep the Token consistent.");  
    }  
}  
```  
The above method's PostModel is an entity class that includes Signature, Timestamp, Nonce (passed in by the Wechat server through the Url parameters when the request is made), as well as AppId, Token, EncodingAESKey and other sensitive information (needs to be passed in). It will also be used later.  
  
  
The following Action (Post) is used to receive Post requests from the Wechat server (usually initiated by the user). The if statement is essential here. The previous Get method only provides verification when the Wechat backend saves the Url. The verification must be re-verified for each Post, otherwise the request can be easily forged.  
```C#  
/// <summary>  
/// After the user sends a message, the Wechat platform automatically posts a request here and waits for the XML response  
/// </summary>  
[HttpPost]  
[ActionName("Index")]  
public ActionResult Post(PostModel postModel)  
{  
    if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))  
    {  
        return Content("Parameter error!");  
    }  
    ...  
}  
```  
  
### How to handle Wechat official account requests?  
  
Senparc.Weixin.MP provides two ways to handle requests, the [conventional method](https://github.com/JeffreySu/WeiXinMPSDK/wiki/Handling-Wechat-messages-in-the-usual-way) and the [MessageHandler](https://github.com/JeffreySu/WeiXinMPSDK/wiki/How-to-use-MessageHandler-to-simplify-message-processing). The wiki has detailed explanations of these two methods. Here is a simple example of using the MessageHandler processing method.  
  
The processing flow of the MessageHandler is very simple:  
``` C#  
[HttpPost]  
[ActionName("Index")]  
public ActionResult Post(PostModel postModel)  
{  
    if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))  
    {  
        return Content("Parameter error!");  
    }  
  
    postModel.Token = Token;  
    postModel.EncodingAESKey = EncodingAESKey;// Keep consistent with your own backend settings  
    postModel.AppId = AppId;// Keep consistent with your own backend settings  
  
    var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);// Receive the message (first step)  
  
    messageHandler.Execute();// Execute the Wechat processing process (second step)  
  
    return new FixWeixinBugWeixinResult(messageHandler);// Return (third step)  
}  
```  
Except for the assignment of postModel in the above code, the receiving (first step), processing (second step), and returning (third step) of the message only require one line of code each.  
  
The CustomMessageHandler in the above code is a custom class that inherits from Senparc.Weixin.MP.MessageHandler.cs. MessageHandler is an abstract class that contains abstract methods for executing various request types (such as text, voice, location, image, etc.). We only need to implement these methods one by one in the CustomMessageHandler we created. The CustomMessageHandler.cs just created is as follows:  
  
```C#  
using System;  
using System.IO;  
using Senparc.Weixin.MP.MessageHandlers;  
using Senparc.Weixin.MP.Entities;  
  
namespace Senparc.Weixin.MP.Sample.CustomerMessageHandler  
{  
    public class CustomMessageHandler : MessageHandler<MessageContext>  
    {  
        public public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)  
            : base(inputStream, postModel, maxRecordCount)  
        {  
  
        }  
  
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)  
        {  
            //ResponseMessageText can also be News or other types  
            var responseMessage = CreateResponseMessage<ResponseMessageText>();  
            responseMessage.Content = "This message is from DefaultResponseMessage.";  
            return responseMessage;  
        }  
  
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)  
        {  
            //...  
        }  
  
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)  
        {  
            //...  
        }  
  
        //More OnXX methods that are not overridden will return the results in DefaultResponseMessage by default.  
        ....  
    }  
}  
```  
  
Where OnTextRequest, OnVoiceRequest, etc. correspond to different types of requests such as receiving text, voice, etc.  
  
For example, if we need to respond to text type requests, we just need to improve the OnTextRequest method:  
```C#  
      public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)  
      {  
          //TODO: The logic here can be handed over to the Service to handle specific information. Refer to the OnLocationRequest method or /Service/LocationSercice.cs  
          var responseMessage = CreateResponseMessage<ResponseMessageText>();  
          responseMessage.Content = string.Format("You just sent a text message: {0}", requestMessage.Content);  
          return responseMessage;  
      }  
```  
When CustomMessageHandler executes messageHandler.Execute(), if it finds that the request information type is text, it will automatically call the above code and return the responseMessage in the code as the returned information. responseMessage can be any type under the IResponseMessageBase interface (including text, news, multimedia, etc.).  
  
Starting from v0.4.0, MessageHandler adds support for user session context to solve the problem that user sessions cannot be managed on the server. See: [User Context WeixinContext and MessageContext](https://github.com/JeffreySu/WeiXinMPSDK/wiki/User-Context-WeixinContext-and-MessageContext)  
  
-->  
  
  
## 👩‍🏫 How to develop with .NET Core  
  
> The current branch includes full code for .NET Framework 4.6.2+ and .NET 6.0/7.0/8.0/10.0 (for versions no longer updated, see [release](https://github.com/JeffreySu/WeiXinMPSDK/releases) snapshots).<br>  
> The Demo for .NET Framework is located in the `/src/Samples/All/net45-mvc` directory, and<br>  
> [Recommended] The Demo for .NET 10.0 (compatible with .NET 5.0, 6.0, 7.0, 8.0, and .NET Core 3.1 and lower versions) is located in `/Samples/All/net10-mvc`.<br><br>  
> Note: In the samples above, `net10-mvc` directly references each module's source code and can generate Senparc.Weixin SDK packages compatible with multiple versions when built in `Release`.
  
## ↕️ Install via Nuget to the project  
  
The Nuget installation methods for each module: [Installing the SDK into the project using Nuget](https://github.com/JeffreySu/WeiXinMPSDK/wiki/Installing-the-SDK-into-the-project-using-Nuget)  
  
## 🏬 Deployment guide  
  
### 1) Deploy to Azure App Service  
  
[App Service](https://docs.microsoft.com/zh-cn/azure/app-service/azure-web-sites-web-hosting-plans-in-depth-overview) is a Web service launched by Microsoft Azure, which has good support for .NET. The deployment steps are detailed in: [Deploy the Wechat site to Azure](https://github.com/JeffreySu/WeiXinMPSDK/wiki/Deploy-the-Wechat-site-to-Azure).  
  
### 2) Deploy to any server via FTP  
  
Install an FTP service on the web server (recommended: [FileZilla Server](https://filezilla-project.org/download.php?type=server)), then upload your locally compiled code directly. The corresponding sample in [Samples](https://github.com/JeffreySu/WeiXinMPSDK/tree/master/Samples) is [Senparc.Weixin.Sample.Net10](https://github.com/JeffreySu/WeiXinMPSDK/tree/Developer/Samples/All/net10-mvc). It can be used directly after compilation without code changes. If you use Azure App Service or other cloud services, FTP is usually enabled as well.  
  
  
<!--  
Implemented Functions  
-------------  
* Wechat Official Account  
>   - [x] Receive/Send Messages (Events)  
>   - [x] Custom Menu & Personalized Menu  
>   - [x] Message Management  
>   - [x] OAuth Authorization  
>   - [x] JSSDK  
>   - [x] Wechat Payment  
>   - [x] User Management  
>   - [x] Material Management  
>   - [x] Account Management  
>       - [x] Parameterized QR Code  
>       - [x] Long URL to Short URL Interface  
>       - [x] Wechat Authentication Event Push  
>   - [x] Data Statistics  
>   - [x] Wechat Store  
>   - [x] Wechat Card Coupon  
>       - [x] Card Coupon Event Push  
>           - [ ] Payment Event Push  
>           - [ ] Membership Card Content Update Event Push  
>           - [ ] Inventory Alert Event Push  
>           - [ ] Coupon Point Flow Detail Event Push  
>   - [x] Wechat Store  
>   - [x] Wechat Intelligence  
>   - [x] Wechat Device Function  
>   - [x] Customer Service Function  
>   - [x] Wechat Shake Around  
>   - [x] Wechat Wi-Fi (Incomplete)  
>   - [x] Wechat Scan QR Code (Merchant)  
>       - [ ] Scan QR Code Event Push  
>           - [ ] Open Product Homepage Event Push  
>           - [ ] Follow Official Account Event Push  
>           - [ ] Enter Official Account Event Push  
>           - [ ] Asynchronous Push of Geographic Location Information  
>           - [ ] Product Audit Result Push  
  
* Wechat Open Platform  
>   - [x] Website Application  
>   - [x] Official Account Third-Party Platform  
  
  
* Wechat Work Account  
>	- [x] Manage Address Book  
>	- [x] Manage Material Files  
>	- [x] Manage Enterprise Account Applications  
>	- [x] Receive Messages and Events  
>	- [x] Send Messages  
>	- [x] Custom Menu  
>	- [x] Identity Authentication Interface  
>	- [x] JSSDK  
>	- [x] Third-Party Application Authorization  
>	    - [x] Third-Party Callback Protocol  
>	        - [ ] Authorization Code Event Push  
>	        - [ ] Address Book Change Notification  
> 	- [x] Enterprise Account Authorization Login  
>	- [x] Enterprise Account Wechat Payment  
>	- [x] Enterprise Session Service  
>	    - [ ] Enterprise Session Callback  
>	- [x] Enterprise Shake Around  
>	- [ ] Enterprise Card Coupon Service  
>	    - [ ] Card Coupon Event Push  
>	- [x] Enterprise Customer Service  
>	    - [ ] Customer Service Reply Message Callback  
	      
  
  
* Cache Strategy  
>   - [x] Strategy Extension Interface  
>   - [x] Local Cache  
>   - [x] Redis Extension Package  
>   - [x] Memcached Extension Package  
  
 Welcome developers to submit Pull Requests for unfinished or to-be-supplemented modules!  
-->  
  
## 🍴 Important Branches  
  
|  Branch    |     Description           
|-----------|---------------  
| master    | The main branch for official releases. This branch is usually more stable and can be used in production environments.  
| Developer | 1. The development branch. This branch is usually the Beta version, and new versions are developed in this branch before being pushed to the master branch. If you want to get a sneak peek of new features, you can use this branch.<br>2. This branch is compatible with .NET 4.5 / .NET Core / .NET Core 2.0 versions at the same time. It is recommended to submit Pull Requests for code to this branch instead of the master branch.  
| BookVersion1 | This branch is a code snapshot of the book [In-Depth Analysis of Wechat Development: Efficient Development Secrets for Official Accounts and Mini Programs](https://book.weixin.senparc.com/book/link?code=github-homepage2) publication.  
| DotNET-Core_MySQL | This branch is a demonstration branch for integrating [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) framework in .NET Core environment.  
| NET4.0     | Branch that only supports .NET 4.0. This branch was stopped updating in 2017. The latest code for .NET 4.0 is synchronized with the master / Developer branch.  
| NET3.5     | Branch that only supports .NET 3.5. This branch was stopped updating in 2015. The latest code for .NET 3.5 is synchronized with the master / Developer branch.  
| Developer-Senparc.SDK | This branch is only used for internal testing of the Senparc team and can be ignored.  
  
  
## 🍟 Thanks To Contributors  
  
Thanks to the developers who contributed to this project. You have not only improved this project, but also made a contribution to the Chinese open source community. Thank you! The list can be found [here](https://github.com/JeffreySu/WeiXinMPSDK/blob/master/Contributors.md).  

<a href="https://github.com/JeffreySu/WeiXinMPSDK/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=JeffreySu/WeiXinMPSDK&max=200" />
</a>
  
## 💰 Donations  
  
If this project is useful to you, we welcome any form of donation, including participating in project code updates or providing feedback. Thank you!  
  
Donate:  
  
[![donate](http://sdk.weixin.senparc.com/Images/T1nAXdXb0jXXXXXXXX_s.png)](http://sdk.weixin.senparc.com#donate)


## ⭐ Star Quantity Statistics 

[![starcharts stargazers over time](https://starchart.cc/JeffreySu/WeiXinMPSDK.svg)](https://starchart.cc/JeffreySu/WeiXinMPSDK)

## 📊 Project Operations Statistics

### Star Growth Trend

[![Star History](https://next.ossinsight.io/widgets/official/analyze-repo-stars-history/thumbnail.png?repo_id=7560589&image_size=auto&color_scheme=light)](https://ossinsight.io/analyze/JeffreySu/WeiXinMPSDK)

### Issues Opened and Closed Trend

[![Issues Opened and Closed](https://next.ossinsight.io/widgets/official/analyze-repo-issue-opened-and-closed/thumbnail.png?repo_id=7560589&image_size=auto&color_scheme=light)](https://next.ossinsight.io/widgets/official/analyze-repo-issue-opened-and-closed?repo_id=7560589)

### Total NuGet Downloads

[![NuGet Downloads](https://img.shields.io/nuget/dt/Senparc.Weixin?style=for-the-badge&logo=nuget&label=NuGet%20Downloads)](https://www.nuget.org/packages/Senparc.Weixin)

## 📎 License

Apache License Version 2.0

```
Copyright 2025 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.
```
Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

> [!TIP]
> 100% open source, commercial use supported.
