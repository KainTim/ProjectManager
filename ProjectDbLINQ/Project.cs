using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDbLINQ;
internal class Project
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public override string ToString() => Name;
}
