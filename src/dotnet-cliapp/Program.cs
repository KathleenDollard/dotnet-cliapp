using StarFruit2;
using System;

namespace DotnetCli
{
    class Program
    {
        static void Main(string[] args)
        {
             CommandSource.Run<DotnetCommand>(args);
        }
    }
}
