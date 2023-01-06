using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Models.Group
{
    public class GroupList : ObservableObject
    {
        private string groupname;
        private string employeename;
        private string username;
        private double totalhour;
        private int selectedindex;
        private IList<CreateGroupDTO> titlegroup;

        public string GroupName
        {
            get => groupname;
            set => SetProperty(ref groupname, value);
        }
        public string EmployeeName
        {
            get => employeename;
            set => SetProperty(ref employeename, value);
        }
        public string UserName
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        public double TotalHour
        {
            get => totalhour;
            set => SetProperty(ref totalhour, value);
        }
        public IList<CreateGroupDTO> TitleGroup
        {
            get => titlegroup;
            set => SetProperty(ref titlegroup, value);
        }
        public int SelectedIndex
        {
            get => selectedindex;
            set => SetProperty(ref selectedindex, value);
        }

        public AssignGroupDTO SelectedGroup { get; set; }
        public string DayMonthYear{ get; set; }
        public bool IsRejected { get; set; }
    }
}
