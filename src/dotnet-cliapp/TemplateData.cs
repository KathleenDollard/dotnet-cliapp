using System.Collections.Generic;

namespace DotnetCli
{
    public record TemplateData()
    {
        public string TemplateName { get; init; }
        public string ShortName { get; init; }
        public string Language { get; init; }
        public string Tags { get; init; }
        public string Author { get; init; }
        public TemplateType Type { get; init; }
        public static List<TemplateData> SampleData
                => new List<TemplateData>
                {
                      new TemplateData{TemplateName="Console Application", ShortName="console", Language="[C#], F#, VB", Tags="Common/Console" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Class library", ShortName="classlib", Language="[C#], F#, VB", Tags="Common/Library" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="WPF Application", ShortName="wpf", Language="[C#], VB", Tags="Common/WPF",Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="WPF Class library", ShortName="wpflib", Language="[C#], VB", Tags="Common/WPF" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="WPF Custom Control Library", ShortName="wpfcustomcontrollib", Language="[C#], VB", Tags="Common/WPF" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="WPF User Control Library", ShortName="wpfusercontrollib", Language="[C#], VB", Tags="Common/WPF" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Windows Forms App", ShortName="winforms", Language="[C#], VB", Tags="Common/WinForms" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Windows Forms Control Library", ShortName="winformscontrollib", Language="[C#], VB", Tags="Common/WinForms" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Windows Forms Class Library", ShortName="winformslib", Language="[C#], VB", Tags="Common/WinForms" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Worker Service", ShortName="worker", Language="[C#], F#", Tags="Common/Worker/Web" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Unit Test Project", ShortName="mstest", Language="[C#], F#, VB", Tags="Test/MSTest" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="NUnit 3 Test Project", ShortName="nunit", Language="[C#], F#, VB", Tags="Test/NUnit" , Type=TemplateType.Project, Author="Aleksei Kharlov aka halex2005 (codeofclimber.ru)"},
                      new TemplateData{TemplateName="NUnit 3 Test Item", ShortName="nunit-test", Language="[C#], F#, VB", Tags="Test/NUnit" , Type=TemplateType.Item, Author="Aleksei Kharlov aka halex2005 (codeofclimber.ru)"},
                      new TemplateData{TemplateName="xUnit Test Project", ShortName="xunit", Language="[C#], F#, VB", Tags="Test/xUnit" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Razor Component", ShortName="razorcomponent", Language="[C#]", Tags="Web/ASP.NET" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="Razor Page", ShortName="page", Language="[C#]", Tags="Web/ASP.NET" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="MVC ViewImports", ShortName="viewimports", Language="[C#]", Tags="Web/ASP.NET" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="MVC ViewStart", ShortName="viewstart", Language="[C#]", Tags="Web/ASP.NET" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="Blazor Server App", ShortName="blazorserver", Language="[C#]", Tags="Web/Blazor" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Blazor WebAssembly App", ShortName="blazorwasm", Language="[C#]", Tags="Web/Blazor/WebAssembly" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core Empty", ShortName="web", Language="[C#], F#", Tags="Web/Empty" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core Web App (Model-View-Controller)", ShortName="mvc", Language="[C#], F#", Tags="Web/MVC" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core Web App", ShortName="webapp", Language="[C#]", Tags="Web/MVC/Razor Pages" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core with Angular", ShortName="angular", Language="[C#]", Tags="Web/MVC/SPA" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core with React.js", ShortName="react", Language="[C#]", Tags="Web/MVC/SPA" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core with React.js and Redux", ShortName="reactredux", Language="[C#]", Tags="Web/MVC/SPA" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="Razor Class Library", ShortName="razorclasslib", Language="[C#]", Tags="Web/Razor/Library" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core Web API", ShortName="webapi", Language="[C#], F#", Tags="Web/WebAPI" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="ASP.NET Core gRPC Service", ShortName="grpc", Language="[C#]", Tags="Web/gRPC" , Type=TemplateType.Project, Author="Microsoft"},
                      new TemplateData{TemplateName="dotnet gitignore file", ShortName="gitignore", Language="", Tags="Config" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="global.json file", ShortName="globaljson", Language="", Tags="Config" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="NuGet Config", ShortName="nugetconfig", Language="", Tags="Config" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="Dotnet local tool manifest file", ShortName="tool-manifest", Language="", Tags="Config" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="Web Config", ShortName="webconfig", Language="", Tags="Config" , Type=TemplateType.Item, Author="Microsoft"},
                      new TemplateData{TemplateName="Solution File", ShortName="sln", Language="", Tags="Solution" , Type=TemplateType.Solution, Author="Microsoft"},
                      new TemplateData{TemplateName="Protocol Buffer File", ShortName="proto", Language="", Tags="Web/gRPC" , Type=TemplateType.Item, Author="Microsoft"}

                };
    }

}
