using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDbLINQ;
internal class Employee
{
  public int Id { get; set; }
  public required string Firstname { get; set; }
  public required string Lastname { get; set; }
  public int Age { get; set; }
  public double Salary { get; set; }
  public required string Department { get; set; }

  public static Employee Parse(string line)
  {
    //0       1           2     3     4         5
    //id; firstname; lastname; age; salary; department
    //1; Carey; Sands; 53; 3980.6; Training
    string[] parts = line.Split(';');
    return new Employee()
    {
      Id = int.Parse(parts[0]),
      Firstname = parts[1],
      Lastname = parts[2],
      Age = int.Parse(parts[3]),
      Salary = double.Parse(parts[4]),
      Department = parts[5],
    };
  }
}
