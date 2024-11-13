using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDbLINQ;
public class ProjectContext
{
  public List<Project> Projects { get; } = new List<Project>();
  public List<Employee> Employees { get; } = new List<Employee>();
  public List<ProjectEmployee> ProjectEmployees { get; } = new List<ProjectEmployee>();

  public ProjectContext()
  {
    InitializeProjects();
    InitializeEmployees();
    InitializeProjectEmployees();
  }

  private void InitializeProjectEmployees()
  {
    int id = 0;
    foreach (var line in File.ReadAllLines(@"csv\project_employees.csv").Skip(1))
    {
      string[] parts = line.Split(';');
      var employeeName = parts[0];
      var projectName = parts[1];
      
      var employee = Employees
        .Single(x => $"{x.Lastname} {x.Firstname}".Equals(employeeName));
      var project = Projects
        .Single(x => x.Name.Equals(projectName));
      var projectEmployee = new ProjectEmployee()
      {
        Id = id,
        EmployeeId = employee.Id,
        ProjectId = project.Id,
        AssigningTime = DateTime.Now,
        Employee = employee,
        Project = project,
      };
      ProjectEmployees.Add(projectEmployee);
      id++;
    };
  }
  private void InitializeEmployees()
  {
    foreach (var line in File.ReadAllLines(@"csv\employees.csv").Skip(1))
    {
        Employees.Add(Employee.Parse(line));
    }
  }
  private void InitializeProjects()
  {
    foreach (var line in File.ReadAllLines(@"csv\projects.csv"))
    {
      Projects.Add(Project.Parse(line));
    }
  }
}
