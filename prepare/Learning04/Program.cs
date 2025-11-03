using System;
using System.Reflection.PortableExecutable;

class Program
{
    class Assignment
    {
        private string Name;
        private string Topic;

        public Assignment(string name, string topic)
        {
            Name = name;
            Topic = topic;
        }

        public string GetSummary()
        {
            Console.WriteLine($"Student: {Name}");
            Console.WriteLine($"Topic: {Topic}");
            return $"{Name} - {Topic}";
        }
    }
    class MathAssignment : Assignment
    {
        private string Section;

        private string Problems;

        public MathAssignment(string name, string topic, string section, string problems)
            : base(name, topic)
        {
            Section = section;
            Problems = problems;
        }

        public string GetHomeworkList()
        {
            Console.WriteLine($"Section: {Section} - Problems: {Problems}");
            return $"Section: {Section} - Problems: {Problems}";
        }
    }
    class WritingAssignment : Assignment
    {
        private string Title;
        private string Author;

        public WritingAssignment(string name, string topic, string title)
            : base(name, topic)
        {
            Title = title;
            Author = name;
        }

        public string GetWritingInformation()
        {
            Console.WriteLine($"{Title} by {Author}");
            return $"{Title} by {Author}";
        }
    }
    static void Main(string[] args)
    {
        MathAssignment assignment = new MathAssignment("John Doe", "calculus", "5.4", "1-10");
        assignment.GetSummary();
        assignment.GetHomeworkList();
        WritingAssignment writing = new WritingAssignment("Jane Smith", "literature", "The Road Not Taken");
        writing.GetSummary();
        writing.GetWritingInformation();
    }
}