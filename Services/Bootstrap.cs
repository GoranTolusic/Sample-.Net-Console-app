namespace EFConsoleApp
{
    public class Bootstrap
    {
        public string[] args;
        public Bootstrap(string[] args = null)
        {
            this.args = args ?? new string[] { };
        }

        public static Bootstrap GetInstance(string[] args = null)
        {
            return new Bootstrap(args);
        }

        public void ReadArgs()
        {
            Console.WriteLine($"Read Args processed. Arguments:");
            foreach (var arg in this.args)
            {
                Console.WriteLine($"{arg}");
            }
        }
    }
}
