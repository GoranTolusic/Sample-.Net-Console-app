//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ElasticSearch;

namespace EFConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticService elasticInstance = ElasticService.GetInstance();
            Bootstrap bootInstance = Bootstrap.GetInstance(args);
            bootInstance.StartProcess();
        }
    }
}
