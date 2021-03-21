using StarFruit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCli
{
    public class DotnetCommand
    {
        [Description("Display help (new view).")]
        [Aliases("-h2")]
        public bool Help2 { get; set; }
   }
}
