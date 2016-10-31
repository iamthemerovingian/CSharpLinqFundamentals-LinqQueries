using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqQueries;
using EmployeeExtensions;

namespace LinqQueriesMili
{
    class Program
    {
        static void Main(string[] args)
        {
            SingleEmployeeQueries();
            UseLetKeyword();
            UseGroup();    
        }

        private static void UseGroup()
        {
            var repository = new EmployeeRepository();
            var queryByDepartment =
                from e in repository.GetAll()
                group e by e.DepartmentID into eGroup
                    orderby eGroup.Key descending
                    where eGroup.Key < 3
                    select new
                    {
                        DepartmentID = eGroup.Key,
                        Count = eGroup.Count(),
                        Employees = eGroup
                    };

            var queryByDepartment2 =
                repository.GetAll()
                .GroupBy(e => e.DepartmentID)
                .OrderByDescending(g => g.Key)
                .Where(g => g.Key < 3)
                .Select(g =>
                new
                {
                    DepartmentID = g.Key,
                    Count = g.Count(),
                    Employees = g
                }
                );

            foreach (var group in queryByDepartment)
            {
                Console.WriteLine($"DeprtmentID:{group.DepartmentID} ----- Employee Count:{group.Count}");
                foreach (var employee in group.Employees)
                {
                    Console.WriteLine($" \t {employee.DepartmentID} : {employee.Name}");
                }
            }

            foreach (var group in queryByDepartment2)
            {
                Console.WriteLine($"DeprtmentID:{group.DepartmentID} ----- Employee Count:{group.Count}");
                foreach (var employee in group.Employees)
                {
                    Console.WriteLine($" \t {employee.DepartmentID} : {employee.Name}");
                }
            }
        }

        private static void UseLetKeyword()
        {
            var repository = new EmployeeRepository();
            var query =
                from e in repository.GetAll()
                let lname = e.Name.ToLower()
                where lname == "Scott"
                select e.Name;
        }

        private static void SingleEmployeeQueries()
        {
            var repository = new EmployeeRepository();
            var query1 =
                (from e in repository.GetAll()
                where e.DepartmentID < 3 && e.ID < 10
                orderby e.DepartmentID descending,
                e.Name ascending
                select e).ToList();

            var query2 =
                repository.GetAll()
                .Where(e => e.DepartmentID < 3 && e.ID < 10)
                .OrderByDescending(e => e.DepartmentID)
                .OrderBy(e => e.Name);

            Write(query1);

            repository.Add(new Employee { ID = 7, DepartmentID = 2, Name = "Andy" });

            Write(query1);

            Write(query2);
        }

        static void Write(IEnumerable<Employee> employees)
        {
            foreach (Employee e in employees)
            {
                Console.WriteLine(e.Name);
            }
            Console.WriteLine();
        }

    }
}
