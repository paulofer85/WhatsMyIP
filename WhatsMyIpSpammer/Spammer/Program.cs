using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogicWhatsMyIp;
using System.Diagnostics;
using Common;

namespace Spammer
{
    class Program
    {

        private static BusinessLogicSpam bLSpam;
        public static Course[] courses;


        static void Main(string[] args)
        {

            bLSpam = new BusinessLogicSpam();
            courses = bLSpam.GetCourses("CoursesBD.json");

            Console.WriteLine("Cursos disponibles:");
            for (int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name}");
            }

            Console.Write("Ingresa el número correspondiente al curso que deseas elegir: ");
            int selectedCourseIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedCourseIndex) || selectedCourseIndex < 1 || selectedCourseIndex > courses.Length)
            {
                Console.WriteLine("Numero invalido. Por favor ingrese un numero valido.");
                Console.Write("Ingresa el número correspondiente al curso que deseas elegir: ");
            }

            var selectedCourse = courses[selectedCourseIndex - 1];

            Console.Write("Ingrese fecha del curso (formato dd/mm/AA): ");
            string date= Console.ReadLine();
            Console.Write("Horario del curso (formato '20 a 22'): ");
            string hoursStartEnd = Console.ReadLine();
            Console.Write("Cantidad de clases (solo numero ej. 5): ");
            string amountClasses = Console.ReadLine();
            Console.Write("Precio (ej. $10000): ");
            string cost = Console.ReadLine();

            selectedCourse.Message = selectedCourse.Message
                .Replace("#DATE", date)
                .Replace("#START_END", hoursStartEnd)
                .Replace("#AMOUNT_CLASSES", amountClasses)
                .Replace("#COST", cost);

            Console.WriteLine($"Seleccionaste: {selectedCourse.Name}");
            Console.WriteLine($"Subject: {selectedCourse.Subject}");
            Console.WriteLine($"Message: \r\n{selectedCourse.Message.Replace("<br />", "\r\n")}");

            Console.Write("Confirmar selección (Y/N): ");
            string confirmation = Console.ReadLine();
            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                // Process the selected course
                Console.WriteLine("Seleccion de curso confirmada procediendo envio de mails.");
                Console.WriteLine("\r\n\r\n\r\n ====================================================================================\r\n\r\n");
                bLSpam.ProcessMails(selectedCourse);
            }
            else
            {
                Console.WriteLine("Selección de cursos cancelada.");
            }

            //bLSpam.ProcessFailedEmails("sacar.txt");
            //bLSpam.UpdateMailsWithBounced("mails.txt", "sacar.txt");
            //bLSpam.GenerateJSONFromFile("mails.txt");
            Console.ReadKey();
        }
    }
}
