using SQLite;

namespace SmartGloveRebuild2.Models;


[Table("Employees")]
public class EmployeeModel
{
    [PrimaryKey, AutoIncrement]
    public int NO { get; set; }

    public string FRNLCL { get; set; }
    public string PLANT { get; set; }
    public string EMPNUM { get; set; }
    public string EmployeeName { get; set; }
    public string PASSPORT { get; set; }
    public object NRIC { get; set; }
    public string POST { get; set; }
    public string PAYROL { get; set; }


    //public string Name { get; set; }
    //public string Location { get; set; }
    //public string Details { get; set; }
    //public string Image { get; set; }
    //public int Population { get; set; }
    //public double Latitude { get; set; }
    //public double Longitude { get; set; }

}