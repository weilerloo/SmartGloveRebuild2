using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models
{
    public class UserBasicInfo
    {
        public string EmployeeNumber { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string DepartmentCode { get; set; }
        public string Role { get; set; }
        public int? RoleID { get; set; }

        public string EmployeeName
        {
            get => $"{GivenName} {Surname}";
        }

    }

    public enum RoleDetails
    {
        Employee = 1,
        Supervisor,
        HR,
        HOD,
    }
}
