using System;

namespace BTVN1
{
    internal class CourseOnline : Course
    {
        public string linkMeeting { get; set; }

        public CourseOnline() : base() { }
        Validation validator = new Validation();
        public override void Input()
        {
            base.Input();
            Console.Write("Enter meeting link: ");
            linkMeeting = Console.ReadLine();
            if (validator.validateStringInput(linkMeeting, 1, 200) == false)
            {
                return;
            }
        }

        public override string ToString()
        {
            return base.ToString() + $", meeting link: {linkMeeting}";
        }
        public static override void readFromLine(string line)
        {
            base.readFromLine(line);
            string[] items = line.Split('|');
            linkMeeting = items[4];
        }
    }
}
