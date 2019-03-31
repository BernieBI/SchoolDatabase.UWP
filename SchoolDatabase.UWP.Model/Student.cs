using System.Collections.Generic;

namespace SchoolDatabase.UWP.Model
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }

        public Student() {}
        public Student(string fullname)
        {
            this.FullName = fullname;
        }
    }

}
