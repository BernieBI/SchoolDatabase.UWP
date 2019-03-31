using System.Collections.Generic;

namespace SchoolDatabase.UWP.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public Course() { }
        public Course(string courseName)
        {
            this.CourseName = courseName;
        }
    }
}
