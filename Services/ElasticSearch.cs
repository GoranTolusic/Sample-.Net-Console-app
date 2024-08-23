namespace ElasticSearch
{
    public class ElasticService
    {
        public ElasticService() {
            Console.WriteLine($"ElasticService Instantiated");
        }

        public static ElasticService GetInstance()
        {
            return new ElasticService();
        }
    }
}