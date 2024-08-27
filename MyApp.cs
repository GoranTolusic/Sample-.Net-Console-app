//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap bootInstance = Bootstrap.GetInstance(args);
            bootInstance.StartProcess();
        }
    }
}
