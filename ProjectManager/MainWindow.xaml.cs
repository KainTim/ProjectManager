using ProjectDbLINQ;

using System.Linq;

namespace ProjectManager;

public partial class MainWindow : Window
{

  public MainWindow() => InitializeComponent();
  private ProjectContext db = new ProjectContext();

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    var cultureInfo = new System.Globalization.CultureInfo("en-US");
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    Thread.CurrentThread.CurrentCulture = cultureInfo;
    foreach (var department in db.Employees.Select(x => x.Department).Distinct())
    {
      var radioButton = new RadioButton
      {
        Content = department,
      };
      radioButton.Checked += RadioButton_Checked_Changed;
      radioButton.Unchecked += RadioButton_Checked_Changed;
      panDepartments.Children.Add(radioButton);
    }
    panDepartments.Children.OfType<RadioButton>().First().IsChecked = true;
    foreach (var project in db.Projects)
    {
      var checkBox = new CheckBox
      {
        Content = project,
        IsChecked = true,
      };
      checkBox.Checked += CheckBox_Checked_Changed;
      checkBox.Unchecked += CheckBox_Checked_Changed;
      panProjects.Children.Add(checkBox);
    }
    lstEmployees.ItemsSource = db.Employees;
    grdEmployees.ItemsSource = FilteredGridEmployees();
    lstEmployees.ItemsSource = FilteredListEmployees();
    CheckInputFields();
    txtDepartment.KeyUp += TxtDepartment_KeyUp;
    txtProjects.KeyUp += TxtProjects_KeyUp;
    btnAdd.Click += BtnAdd_Click;
  }

  private void BtnAdd_Click(object sender, RoutedEventArgs e)
  {
    var employee = new Employee()
    {
      Department = txtDepartment.Text,
      Age = int.Parse(txtAge.Text),
      Firstname = txtFirstname.Text,
      Lastname = txtLastname.Text,
      Salary = double.Parse(txtSalary.Text),
      Id = db.Employees.Select(x => x.Id).Max() + 1,
    };
    db.Employees.Add(employee);
    string[] projects = txtProjects.Text.Split(",");
    foreach (string project in projects)
    {
      ProjectEmployee pe = new ProjectEmployee()
      {
        AssigningTime = DateTime.Now,
        Employee = employee,
        EmployeeId = employee.Id,
        Id = db.ProjectEmployees.Select(x => x.Id).Max() + 1,
        Project = db.Projects.Where(x => x.Name == project).Single(),
        ProjectId = db.Projects.Where(x => x.Name == project).Single().Id
      };
      db.ProjectEmployees.Add(pe);
    }
    var employees = db.ProjectEmployees;
    grdEmployees.ItemsSource = FilteredGridEmployees();
    lstEmployees.ItemsSource = FilteredListEmployees();
  }
  private void TxtProjects_KeyUp(object sender, KeyEventArgs e) => CheckProjects();
  private void TxtDepartment_KeyUp(object sender, KeyEventArgs e) => CheckDepartment();

  private void CheckBox_Checked_Changed(object sender, RoutedEventArgs e)
  {
    grdEmployees.ItemsSource = FilteredGridEmployees();
    lstEmployees.ItemsSource = FilteredListEmployees();
  }

  private void RadioButton_Checked_Changed(object sender, RoutedEventArgs e)
  {
    lstEmployees.ItemsSource = FilteredListEmployees();
    grdEmployees.ItemsSource = FilteredGridEmployees();
  }
  public List<EmployeeRow> FilteredGridEmployees()
  {

    var selectedDepartment = panDepartments.Children
        .OfType<RadioButton>()
        .Where(x => x.IsChecked ?? false)
        .Single().Content;
    var employees = db.Employees
      .Where(x => selectedDepartment.ToString() == x.Department)
      .ToList();

    var selectedProjects = panProjects.Children.OfType<CheckBox>()
      .Where(x => x.IsChecked ?? false)
      .Select(x => x.Content.ToString()).ToList();

    var selectedProjectIds = db.Projects
      .Where(x => selectedProjects.Contains(x.Name))
      .Select(x => x.Id)
      .ToList();

    List<EmployeeRow> result = [];
    foreach (var employee in employees)
    {
      var assignedProjects = db.ProjectEmployees
        .Where(pe => pe.EmployeeId == employee.Id && selectedProjectIds.Contains(pe.ProjectId))
        .Select(x => x.Project.Name)
        .ToList();
      var projectAssignDates = db.ProjectEmployees
        .Where(pe => pe.EmployeeId == employee.Id && selectedProjectIds.Contains(pe.ProjectId))
        .Select(x => x.AssigningTime)
        .ToList();
      if (assignedProjects.Count == 0) continue;
      result.Add(new EmployeeRow()
      {
        DateString = string.Join(", ", projectAssignDates
        .Select(x => $"{x:dd.MM.yyyy}")),
        Department = employee.Department,
        Name = $"{employee.Firstname} {employee.Lastname}",
        Project = string.Join(", ", assignedProjects),
      });
    }
    return result;
  }
  public List<String> FilteredListEmployees()
  {

    var selectedDepartment = panDepartments.Children
        .OfType<RadioButton>()
        .Where(x => x.IsChecked ?? false)
        .Single().Content;
    var employees = db.Employees
      .Where(x => selectedDepartment.ToString() == x.Department)
      .ToList();

    var selectedProjects = panProjects.Children.OfType<CheckBox>()
      .Where(x => x.IsChecked ?? false)
      .Select(x => x.Content.ToString())
      .ToList();

    var selectedProjectIds = db.Projects
      .Where(x => selectedProjects.Contains(x.Name))
      .Select(x => x.Id)
      .ToList();

    List<String> result = [];
    foreach (var employee in employees)
    {
      var assignedProjects = db.ProjectEmployees
        .Where(pe => pe.EmployeeId == employee.Id && selectedProjectIds.Contains(pe.ProjectId))
        .Select(x => x.Project.Name)
        .Order()
        .ToList();
      var projectAssignDates = db.ProjectEmployees
        .Where(pe => pe.EmployeeId == employee.Id && selectedProjectIds.Contains(pe.ProjectId))
        .Select(x => x.AssigningTime)
        .ToList();
      if (assignedProjects.Count == 0) continue;
      result.Add($"{employee} [{string.Join(", ", assignedProjects)}]");
    }
    return result;
  }
  public void CheckInputFields()
  {
    CheckDepartment();
    CheckProjects();
  }

  private void CheckProjects()
  {
    var allProjects = db.Projects.Select(x => x.Name);
    if (allProjects.Contains(txtProjects.Text))
    {
      btnAdd.IsEnabled = true;
      txtProjects.Background = Brushes.White;
      return;
    }
    var valid = true;
    string[] selectedProjects = txtProjects.Text.Split(",");
    if (selectedProjects.Length <= 1) valid = false;
    foreach (var project in selectedProjects)
    {
      if (allProjects.Contains(project)) continue;
      valid = false;
    }
    if (!valid)
    {
      btnAdd.IsEnabled = false;
      txtProjects.Background = Brushes.Red;
    }
    else
    {
      btnAdd.IsEnabled = true;
      txtProjects.Background = Brushes.White;
    }
  }
  private void CheckDepartment()
  {
    var departments = db.Employees
      .Select(x => x.Department)
      .Distinct()
      .ToList();
    if (!departments.Contains(txtDepartment.Text))
    {
      txtDepartment.Background = Brushes.Red;
      btnAdd.IsEnabled = false;
    }
    else
    {
      txtDepartment.Background = Brushes.White;
      btnAdd.IsEnabled = true;
    }
  }
}
