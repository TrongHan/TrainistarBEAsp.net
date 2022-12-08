using System;

namespace backend.Model
{
    public class CourseDTO
    {
        public string idCourse { get; set; }
        public string idTeacher { get; set; }
        public string idManager { get; set; }
        public string nameCourse { get; set; }
        public string description { get; set; }
        public string idQuestionBank { get; set; }
        public DateTime history { get; set; }
    }
}
