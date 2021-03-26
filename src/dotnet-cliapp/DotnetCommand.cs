using Common;
using StarFruit2;

namespace DotnetCli
{
    [Description("Access commands in support of .NET")]
    public class DotnetCommand
    {
        /// <summary>
        /// Sets the UI appearance. 0 for none -> 5 for richest."
        /// </summary>       
        public int UxLevel { get; set; }


        /// <summary>
        /// Terminal or Json. Terminal is default.
        /// </summary>
        public RenderContext RenderTo { get; set; }

        /// <summary>
        /// File for output
        /// </summary>
        public string? RenderToFileName { get; set; }


        /// <summary>
        /// Display help (new view).
        /// </summary>
        [Aliases("-h2")]
        public bool Help2 { get; set; }
    }
}
