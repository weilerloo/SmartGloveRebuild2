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
        public string EmployeeName { get; set; }
        public string GroupName { get; set; }
        public string ForeignerLocal { get; set; }
        public string Passport { get; set; }
        public string NRIC { get; set; }
        public string Payroll { get; set; }
        public string Plant { get; set; }
        public string Department { get; set; }
        public int TotalOTDay { get; set; }
        public double TotalHour { get; set; }
        public string Role { get; set; }
        public int? RoleID { get; set; }
        public string RefreshToken { get; set; }

        //public string EmployeeName
        //{
        //    get => $"{GivenName} {Surname}";
        //}

    }

    public enum RoleDetails
    {
        Employee = 1,
        Supervisor,
        HR,
        HOD,
        Clerk,
        Technician,
        Worker,
    }
}