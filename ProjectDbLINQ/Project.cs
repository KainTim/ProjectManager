using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDbLINQ;
public class Project
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public override string ToString() => Name;

  public static Project Parse(string line)
  {
    string[] parts = line.Split(";");
    return new Project()
    {
      Id = int.Parse(parts[0]),
      Name = parts[1],
    };
  }
}
