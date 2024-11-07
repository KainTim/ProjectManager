using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDbLINQ;
internal class ProjectEmployee
{
  public int Id { get; init; }
  public int EmployeeId { get; init; }
  public int ProjectId { get; init; }
  public DateTime AssigningTime { get; set; }
  public Employee Employee { get; set; } = null!;
  public Project Project { get; set; } = null!;
}
