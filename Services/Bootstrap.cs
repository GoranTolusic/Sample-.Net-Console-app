using EFConsoleApp.Data;
using EFConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using ElasticSearch;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace EFConsoleApp
{
    public class Bootstrap
    {
        private readonly IConfigurationRoot config;
        public string[] args;

        public SchoolContext context;

        public ElasticSearchService elastic = ElasticSearchService.GetInstance();

        public Bootstrap(IConfigurationRoot config, string[] args = null)
        {
            this.args = args ?? new string[] { };
            this.context = new SchoolContext();
            this.config = config;
        }

        public static Bootstrap GetInstance(IConfigurationRoot config, string[] args = null)
        {
            return new Bootstrap(config, args);
        }

        public void ReadArgs()
        {
            Console.WriteLine($"Read Args processed. Arguments:");
            foreach (var arg in this.args)
            {
                Console.WriteLine($"{arg}");
            }
        }

        void GetAllStudents()
        {
            // Dohvat svih studenata iz baze
            var students = this.context.Students.ToList();

            // Ispis podataka o studentima u konzolu
            Console.WriteLine("List of Students:");
            foreach (var student in students)
            {
                Console.WriteLine($"Id: {student.StudentId}, First Name: {student.FirstName}, Last Name: {student.LastName}");
            }
        }
        void PrintStudentCount()
        {
            Console.WriteLine("Student count function is called.");
            // Ovdje možete dodati logiku za dohvaćanje broja studenata ili nešto drugo
        }

        void SetupDatabases()
        {
            // Migriranje MSSQL Databasea
            this.context.Database.Migrate();

            //Kreiranje Elastic indexa
            this.elastic.CreateIndexes(this.context).Wait();
        }

        void Add()
        {
            if (this.args.Length == 3)
            {
                this.context.Students.Add(new Student { FirstName = this.args[1], LastName = this.args[2] });
                this.context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Operation require two additional arguments: First Name and Last Name");
                return;
            }
            Console.WriteLine($"Student added : {this.args[1]} {this.args[2]}");
        }

        void Fetch()
        {
            this.PrintStudentCount();
            this.GetAllStudents();
        }

        void SeedDummies()
        {
            // Dodajte neke studente ako baza nije prazna
            if (!this.context.Students.Any())
            {
                this.context.Students.Add(new Student { FirstName = "John", LastName = "Doe" });
                this.context.Students.Add(new Student { FirstName = "Jane", LastName = "Smith" });
                this.context.SaveChanges();
                Console.WriteLine($"John Doe and Jane Smith added as dummy data");
            }
            else
            {
                Console.WriteLine($"Data entries already exists. This operation works only if MS Sql database is empty");
            }
        }

        void Sync()
        {
            //this is going to be operation which will first delete all data from student index, and then copying all students from sql database into elastic
        }

        void Empty(String[] args)
        {
            if (this.args.Length < 2)
            {
                Console.WriteLine("Missing database argument: sql or elastic");
                return;
            }
            switch (args[1])
            {
                case "sql":
                    this.context.Database.ExecuteSqlRaw("DELETE FROM Students");
                    break;
                case "elastic":
                    //
                    break;
                default:
                    Console.WriteLine("Invalid database argument. Select sql or elastic");
                    break;
            }
        }

        async Task Controller(String[] args)
        {
            switch (args[0])
            {
                case "fetch":
                    this.Fetch();
                    break;
                case "add":
                    this.Add();
                    break;
                case "setup":
                    this.SetupDatabases();
                    break;
                case "dummy":
                    this.SeedDummies();
                    break;
                case "empty":
                    this.Empty(args);
                    break;

                case "serve":
                    await this.Serve();
                    break;
                default:
                    Console.WriteLine("Invalid operation");
                    break;
            }
        }

        async Task Serve()
        {
            // TODO: host variables stores in env/config files
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(this.config["Server:Host"]);
            listener.Start();
            Console.WriteLine($"HTTP Server is running on {this.config["Server:Host"]}");

            // TODO: Need to make new class for Server for whole further logic
            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;

                Console.WriteLine($"Http request event: {request.HttpMethod} {request.Url}");

                Console.WriteLine("Headers:");
                foreach (string headerKey in request.Headers.AllKeys)
                {
                    string headerValue = request.Headers[headerKey];
                    Console.WriteLine($"{headerKey}: {headerValue}");
                }

                string requestBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestBody = reader.ReadToEnd();
                }
                Console.WriteLine($"Request Body: {requestBody}");

                HttpListenerResponse response = context.Response;
                string responseString = "<html><body>HTTP server working properly!</body></html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }

        public async Task StartProcess()
        {
            this.ReadArgs();
            if (this.args.Length == 0)
            {
                Console.WriteLine("Select one of the operations: fetch, add, setup, dummy, empty, serve");
                return;
            }
            await this.Controller(this.args);
        }
    }
}
