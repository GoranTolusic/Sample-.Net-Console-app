using EFConsoleApp.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Dynamic;
using System.Linq;


namespace ElasticSearch
{
    public class ElasticSearchService
    {
        public ElasticSearchService()
        {
            Console.WriteLine($"ElasticService Instantiated");
        }

        public static ElasticSearchService GetInstance()
        {
            return new ElasticSearchService();
        }

        public async Task ElasticRequest()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:9200/");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }

        public async Task ElasticCreateIndex(string indexName, string bodyObject)
        {

            // Strip namespaces and convert to lower cases
            int lastDotIndex = indexName.LastIndexOf('.');
            string actualIndexName = indexName.Substring(lastDotIndex + 1).ToLower();

            using (HttpClient client = new HttpClient())
            {
                string url = $"http://127.0.0.1:9200/{actualIndexName}";
                Console.WriteLine($"url sa put metodom i indexom: {url}");

                // Kreiramo HttpContent sa postavljenim Content-Type headerom
                HttpContent content = new StringContent(bodyObject, Encoding.UTF8, "application/json");

                // Slanje PUT zahtjeva na url
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseBody}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
        }

        public async Task CreateIndexes(SchoolContext context)
        {
            await this.ElasticRequest();

            var model = context.Model;
            foreach (var entityType in model.GetEntityTypes())
            {
                Console.WriteLine($"Entity: {entityType.Name}");
                // Dinamično kreiranje JSON strukture za ElasticSearch
                dynamic elasticJson = new ExpandoObject();
                elasticJson.settings = new ExpandoObject();
                elasticJson.settings.number_of_shards = 1;
                elasticJson.settings.number_of_replicas = 1;

                elasticJson.mappings = new ExpandoObject();
                elasticJson.mappings.properties = new ExpandoObject();

                Console.WriteLine("Properties:");
                foreach (var property in entityType.GetProperties())
                {
                    Console.WriteLine($" - {property.Name} ({property.ClrType.Name})");
                    string elasticType = GetElasticSearchType(property.ClrType);

                    // Dodajemo property u mappings koristeći IDictionary interface za ExpandoObject
                    var propertyDict = (IDictionary<string, object>)elasticJson.mappings.properties;
                    propertyDict[property.Name] = new ExpandoObject();
                    var typeDict = (IDictionary<string, object>)propertyDict[property.Name];
                    typeDict["type"] = elasticType; // Postavljanje tipa                    
                }

                // Serijalizacija dinamičkog objekta u JSON i poziv na elastic API
                string jsonBody = JsonSerializer.Serialize(elasticJson, new JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(jsonBody);
                await this.ElasticCreateIndex(entityType.Name, jsonBody);



                Console.WriteLine("Keys:");
                foreach (var key in entityType.GetKeys())
                {
                    Console.WriteLine($" - {string.Join(", ", key.Properties.Select(p => p.Name))}");
                }

                Console.WriteLine("Foreign Keys:");
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    Console.WriteLine($" - {foreignKey.DependentToPrincipal.Name}");
                }

                Console.WriteLine();
            }

        }

        static string GetElasticSearchType(Type clrType)
        {
            if (clrType == typeof(string))
            {
                return "text"; // ili "keyword" ovisno o scenariju
            }
            else if (clrType == typeof(int) || clrType == typeof(long))
            {
                return "integer";
            }
            else if (clrType == typeof(DateTime))
            {
                return "date";
            }
            else if (clrType == typeof(bool))
            {
                return "boolean";
            }
            else if (clrType == typeof(float) || clrType == typeof(double) || clrType == typeof(decimal))
            {
                return "float";
            }
            else
            {
                return "text"; // Defaultni tip
            }
        }
    }
}