//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bootstrap bootInstance = Bootstrap.GetInstance(args);
            await bootInstance.StartProcess();
        }
    }
}
