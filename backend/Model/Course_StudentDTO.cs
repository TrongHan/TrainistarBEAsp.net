using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Model
{
    public class Course_StudentDTO : UserAuthentication
    {
		public string idCourse { get; set; }
		public string idStudent { get; set; }
		public int diem { get; set; }
		
		public Course_StudentDTO(string idCourse_, string idStudent_, int diem_)
		{
			this.idCourse = idCourse_;
			this.idStudent = idStudent_;
			this.diem = diem_;
		}
	}
}
