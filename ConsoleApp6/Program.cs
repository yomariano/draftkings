using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Solution
{
    class Solution
    {
        static void Main(string[] args)
        {
            ConsoleProcessor processor = new ConsoleProcessor();
            var commands = new List<string> {
                "add,1,Mariano,-1",
                "add,2,German,1",
                "add,3,Klaudio,1",
                "add,5,Manuela,3",
                "add,4,Pedro,-1",
                "move,3,4",
                "move,5,4",

                "print"
            };

            commands.ForEach(x =>
            {
                processor.ProcessLine(x);
            });
            
        }
    }

    public enum Actions
    {
        Add,
        Move,
        Remove,
        Print,
        Count
    }

    public class ConsoleProcessor
    {
        public OrgChart OrgChart { get; set; } = new OrgChart();

        public void ProcessAllLines()
        {
            string line = Console.ReadLine();
            if (Int32.TryParse(line, out int numLines))
            {
                for (int i = 0; i < numLines; i++)
                {
                    ProcessLine(Console.ReadLine());
                }
            }
        }

        public void ProcessLine(string line)
        {
            
            string[] parsedCommand = line.Split(',');

            // ignore empty lines
            if (parsedCommand.Length == 0)
            {
                return;
            }

            

            switch (parsedCommand[0])
            {
                case "add":
                    OrgChart.Add(parsedCommand[1], parsedCommand[2], parsedCommand[3]);
                    break;
                case "print":
                    OrgChart.Print();
                    break;
                case "remove":
                    OrgChart.Remove(parsedCommand[1]);
                    break;
                case "move":
                    OrgChart.Move(parsedCommand[1], parsedCommand[2]);
                    break;
                case "count":
                    Console.WriteLine(OrgChart.Count(parsedCommand[1]));
                    break;
            }

            Console.WriteLine();

        }
    }
}



public class Employee
{
    public Employee()
    {
        Employes = new List<Employee>();
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string ManagerId { get; set; }
    public List<Employee> Employes { get; set; }
}


public class OrgChart
{
    public OrgChart()
    {
        employees = new List<Employee>();
    }

    public List<Employee> employees { get; set; }
    public void Add(string id, string name, string managerId)
    {
        employees.Add(new Employee { Id = id, Name = name, ManagerId = managerId });
    }

    private void PrintEmpl(Employee empl, int space)
    {
        string margin = space == 0 ? "" : space == 1 ? " " : "  ";
        Console.WriteLine($"{margin}{empl.Name} [{empl.Id}]");
        if (empl.Employes.Any())
        {
            space += 1;

            foreach (var e in empl.Employes)
            {
                PrintEmpl(e, space);
            }
        }
    }

    public void Print()
    {

        foreach (var e in employees)
        {
            if (e.ManagerId != "-1")
            {
                var manager = employees.First(m => m.Id == e.ManagerId);
                manager.Employes.Add(e);
            }
        }

        var managers = employees.Where(x => x.ManagerId == "-1").ToList();

        foreach (var m in managers)
        {
            PrintEmpl(m, 0);
        }
    }

    public void Remove(string employeeId)
    {
        var empl = employees.FirstOrDefault(x => x.Id == employeeId);
        if (empl != null)
        {
            employees.Remove(empl);
        }

    }

    public void Move(string employeeId, string newManagerId)
    {
        var empl = employees.FirstOrDefault(x => x.Id == employeeId);
        var manager = employees.FirstOrDefault(x => x.Id == newManagerId);
        if (empl != null && manager != null)
        {
            Employee copy = empl;
            employees.Remove(empl);
            copy.ManagerId = newManagerId;
            employees.Add(copy);
        }
    }

    public int Count(string employeeId)
    {
        return employees.Count();
    }
}