//using System;
//using System.Linq;
using EFConsoleApp.Data;
using EFConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFConsoleApp
{
    class Program
    {

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

        static void Main(string[] args)
        {

            Bootstrap bootInstance = Bootstrap.GetInstance(args);
            bootInstance.ReadArgs();

            if (args.Length > 0 && args[0] == "fetch")
            {
                // Stvaranje instance klase Program
                var program = new Program();

                // Pozivanje instancijske metode
                program.PrintStudentCount();

                program.GetAllStudents();
                return;
            }

            using (var context = new SchoolContext())
            {
                
                // Osigurajte da su svi potrebni migracije primijenjene
                context.Database.Migrate();

                // Dodajte neke studente ako baza nije prazna
                if (!context.Students.Any())
                {
                    context.Students.Add(new Student { FirstName = "John", LastName = "Doe" });
                    context.Students.Add(new Student { FirstName = "Jane", LastName = "Smith" });
                    context.SaveChanges();
                }

                if (args.Length == 3 && args[0] == "add")
                {
                    context.Students.Add(new Student { FirstName = args[1], LastName = args[2] });
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
