using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SchoolDatabase.UWP.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace SchoolApplication.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        public StudentViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            this.ViewModel = new StudentViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }

    public class StudentViewModel
    {
        static String baseUri = "http://localhost:53257/api/";
        static Uri studentUri = new Uri($"{baseUri}students/");
        static Uri courseUri = new Uri($"{baseUri}courses/");
        HttpClient _httpClient = new HttpClient();

        public ObservableCollection<Student> StudentList { get; } = new ObservableCollection<Student>();
        public ObservableCollection<Course> CourseList { get; } = new ObservableCollection<Course>();

        public static Student[] studentsDummyData = { new Student("Arne"), new Student("Kjell"), };

        public StudentViewModel()
        {
            getData();
        }

        public async void getData()
        {
          
            // Load books from a cloud service.


            var studentsResult = await _httpClient.GetAsync(studentUri);
            var coursesResult = await _httpClient.GetAsync(courseUri);

            var studentJson = await studentsResult.Content.ReadAsStringAsync();
            var courseJson = await coursesResult.Content.ReadAsStringAsync();

            var studentList = JsonConvert.DeserializeObject<Student[]>(studentJson);
            foreach (var student in studentList)
            {
                this.StudentList.Add(student);
            }

            var courseList = JsonConvert.DeserializeObject<Course[]>(courseJson);
            foreach (var course in courseList)
            {
                this.CourseList.Add(course);
            }
            
            
        }

    }

}
