using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SchoolDatabase.UWP.DataAccess;
using SchoolDatabase.UWP.Model;

using static System.Console;

namespace SchoolDatabase.consoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            ResetTerminal();
        }

        private static void ResetTerminal()
        {
            Clear();
            Console.WriteLine("__________________HOW TO USE ME__________________________" +
                              "\n" +
                              " - Show all students and courses:    show all " +
                              "\n" +
                              " - Add new student:                  new Student -<FullName>" +
                              "\n" +
                              " - Add new course:                   new Course -<CourseName>" +
                              "\n" +
                              " - Assign Student to Course:         assign -<StudentId> -<CourseId>" +
                              "\n" +
                              " - Unassign Student to Course:       unassign -<StudentId> -<CourseId>" +
                              "\n" +
                              " - View a Students assigned Courses: view -<studentId>" +
                              "\n"
            );
            TakeParams();
        }

        static void getdata()
        {
            using (var db = new SchoolContext())
            {
                Write(
                    "Fetching data in database...\n\n" +
                    "Students: \n"
                );
                foreach (var s in db.Students)
                {
                    WriteLine($" - {s.FullName}, id: {s.StudentId}");
                }

                Write("\n" +
                      "Courses: \n"
                );
                foreach (var c in db.Courses)
                {
                    WriteLine($" - {c.CourseName}, id: {c.CourseId}");
                }

                TakeParams();
            }
        }
        private static string GetInput()
        {
            var input = ReadLine();
            if (input == null || input.ToLower(CultureInfo.CurrentCulture) != "exit") return input;

            WriteLine("So long! :)");
            Thread.Sleep(1000);
            Environment.Exit(1);

            return input;
        }


        /// <summary>
        /// Takes the parameters from user. splitting string into parts then acting accordingly
        /// </summary>
        private static void TakeParams()
        {
            Write("\nReady for commands: ");
            var commands = GetInput().Split("-");

            var command = commands[0].Split(" ")[0];


            if (commands[0] == "show all")
                getdata();

            else if ((commands.Length < 2 || commands.Length > 3))
            {
                WriteLine("That's not the right amount of parameters");
            }

            else
            {
                switch (command.ToLower())
                {
                    case "new":
                        CreateNewObject(commands);
                        break;

                    case "assign":
                        AssignCourse(commands);
                        break;

                    case "unassign":
                        UnassignCourse(commands);
                        break;

                    case "view":
                        ViewStudent(commands);
                        break;
                    default:
                        WriteLine("I dont think i understand...");
                        break;
                }
            }
            TakeParams();
        }

        private static void UnassignCourse(string[] commands)
        {
            WriteLine("\nSaving changes to database");
            using (var db = new SchoolContext())
            {
                if (!int.TryParse(commands[1], out var studentId))
                {
                    WriteLine($"That is not a valid studentId");
                    return;
                }
                if (!int.TryParse(commands[1], out var courseId))
                {
                    WriteLine($"That is not a valid courseId");
                    return;
                }

                var student = db.Students.Find(studentId);
                var course = db.Courses.Find(courseId);

                if (student == null)
                {
                    WriteLine("Student does not exist");
                    return;
                }
                if (course == null)
                {
                    WriteLine("Course does not exist");
                    return;
                }

                //Creating a new studentCourse and then removing the one thats identical 
                db.StudentCourses.Remove(new StudentCourse
                {
                    Student = student,
                    Course = course
                });

                db.SaveChanges();
                WriteLine("Save successful");

                TakeParams();
            }
        }

        private static void ViewStudent(string[] commands)
        {
            using (var db = new SchoolContext())
            {

                if (!int.TryParse(commands[1], out var studentId))
                {
                    WriteLine($"That is not a valid studentId");
                    return;
                }

                //Fetching the attached courses in the same query 
                var student = db.Students.Find(studentId);
                var studentCourses = db.StudentCourses
                    .Where(sc => sc.StudentId == student.StudentId)
                    .Include(sc => sc.Course)
                    .ToList();

                if (studentCourses.Count == 0)
                {
                    WriteLine($"{student.FullName} is not assigned to any courses");
                    return;
                }
                else
                {
                    WriteLine($"{student.FullName} is assigned to the following courses");
                }
                foreach (var studentCourse in studentCourses)
                {
                    WriteLine(" - " + studentCourse.Course.CourseName);
                }
                TakeParams();
            }
        }

        private static void AssignCourse(string[] commands)
        {
            WriteLine("\nSaving changes to database");
            using (var db = new SchoolContext())
            {

                if (!int.TryParse(commands[1], out var studentId))
                {
                    WriteLine($"That is not a valid studentId");
                    return;
                }
                if (!int.TryParse(commands[2], out var courseId))
                {
                    WriteLine($"That is not a valid courseId");
                    return;
                }

                var student = db.Students.Find(studentId);
                var course = db.Courses.Find(courseId);

                if (student == null)
                {
                    WriteLine("Student does not exist");
                    return;
                }
                if (course == null)
                {
                    WriteLine("Course does not exist");
                    return;
                }

                db.StudentCourses.Add(new StudentCourse
                {
                    Student = student,
                    Course = course
                });

                db.SaveChanges();
                WriteLine("Save successful");

                TakeParams();
            }

        }

        private static void CreateNewObject(string[] commands)
        {
            var type = commands[0].Split(" ")[1].Trim().ToLower();

            using (var db = new SchoolContext())
            {
                if (type == "student")
                {
                    db.Students.Add(new Student( commands[1].Trim()) );
                }
                if (type == "course")
                {
                    db.Courses.Add(new Course( commands[1].Trim() ));
                }

                db.SaveChanges();
                WriteLine("Save successful");
                ResetTerminal();
            }
        }

    }
}
