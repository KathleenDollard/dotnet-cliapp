using Common;
using StarFruit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCli
{
    [Description("Access commands in support of .NET")]
    public class DotnetCommand
    {
        [Description("Sets the UI appearance. 0 for none -> 5 for richest.")]
        public int UxLevel { get; set; }

        [Description("Terminal or Json. Terminal is default.")]
        public RenderContext RenderTo { get; set; }

        [Description("Display help (new view).")]
        [Aliases("-h2")]
        public bool Help2 { get; set; }
   }
}
