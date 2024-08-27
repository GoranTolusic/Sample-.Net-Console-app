using EFConsoleApp.Data;

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

        public void CreateIndexes(SchoolContext context)
        {
            {
                //Scan models for props and keys.
                var model = context.Model;
                foreach (var entityType in model.GetEntityTypes())
                {
                    //TODO: na temelju ovih podataka napraviti requestove prema elastic api-u i kreirati index modele za studenta i profesora

                    Console.WriteLine($"Entity: {entityType.Name}");

                    Console.WriteLine("Properties:");
                    foreach (var property in entityType.GetProperties())
                    {
                        Console.WriteLine($" - {property.Name} ({property.ClrType.Name})");
                    }

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
        }
    }
}