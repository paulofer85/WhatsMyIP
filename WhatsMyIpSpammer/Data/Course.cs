namespace Data
{
    public class Course
    {
        public Course(string subject, string message)
        {
            Subject = subject;
            Message = message;
        }

        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
