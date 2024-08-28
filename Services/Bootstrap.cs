using EFConsoleApp.Data;
using EFConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using ElasticSearch;

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

        void GetAllStudents()
        {
            using (var context = new SchoolContext())
            {
                // Dohvat svih studenata iz baze
                var students = context.Students.ToList();

                // Ispis podataka o studentima u konzolu
                Console.WriteLine("List of Students:");
                foreach (var student in students)
                {
                    Console.WriteLine($"Id: {student.StudentId}, First Name: {student.FirstName}, Last Name: {student.LastName}");
                }
            }
        }
        void PrintStudentCount()
        {
            Console.WriteLine("Student count function is called.");
            // Ovdje možete dodati logiku za dohvaćanje broja studenata ili nešto drugo
        }

        public void StartProcess()
        {
            this.ReadArgs();
            
            if (this.args.Length > 0 && this.args[0] == "fetch")
            {
                // Pozivanje instancijske metode
                this.PrintStudentCount();

                this.GetAllStudents();
                return;
            }

             using (var context = new SchoolContext())
            {
                
                // Migriranje MSSQL Databasea
                context.Database.Migrate();
                

                //Kreiranje Elastic indexa
                ElasticSearchService elastic = ElasticSearchService.GetInstance();
                elastic.CreateIndexes(context).Wait();

                // Dodajte neke studente ako baza nije prazna
                if (!context.Students.Any())
                {
                    context.Students.Add(new Student { FirstName = "John", LastName = "Doe" });
                    context.Students.Add(new Student { FirstName = "Jane", LastName = "Smith" });
                    context.SaveChanges();
                }

                if (this.args.Length == 3 && this.args[0] == "add")
                {
                    context.Students.Add(new Student { FirstName = this.args[1], LastName = this.args[2] });
                    context.SaveChanges();

                }

                // Dohvatite sve studente iz baze
                var students = context.Students.ToList();

                // Prikaz podataka u konzoli
                foreach (var student in students)
                {
                    Console.WriteLine($"Id: {student.StudentId}, First Name: {student.FirstName}, Last Name: {student.LastName}");
                }
            }
        }


    }
}
