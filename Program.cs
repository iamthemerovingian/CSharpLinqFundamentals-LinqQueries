using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace LinqQueries
{
    class Program
    {
        static void Main(string[] args)
        {
           // SimpleEmployeeQueries();
            //UseLetKeyword();
           // UseGroup();
           // UseJoin();
            //Composition();
            DynamicQuery();
        }

        private static void DynamicQuery()
        {
            var repository = new EmployeeRepository();

            var query = repository.GetAll()
                                  .AsQueryable()
                                  .OrderBy("Name")
                                  .Where("DepartmentID = 1");
            Write(query);
            
                
        }

        private static void Composition()
        {
            var repository = new EmployeeRepository();

            var query =
                from e in repository.GetByDepartmentID(1)
                where e.Name.Length < 6
                select e;

            Write(query);
        }

        private static void UseJoin()
        {
            var employeesRepository = new EmployeeRepository();
            var departmentRepository = new DepartmentRepository();

            var query =
                from d in departmentRepository.GetAll()
                join e in employeesRepository.GetAll()
                 on
                    d.ID equals e.DepartmentID 
                    into ed
                select new { Department = d.Name, 
                             Employees = ed };

            foreach (var group in query)
            {
                Console.WriteLine(group.Department);
                foreach (var employee in group.Employees)
                {
                    Console.WriteLine("\t" + employee.Name);
                }
            }


        }

        private static void UseGroup()
        {
            var repository = new EmployeeRepository();

            var queryByDepartment =
                from e in repository.GetAll()
                group e by e.DepartmentID
                    into eGroup
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
                              });

            foreach (var group in queryByDepartment2)
            {
                Console.WriteLine("DID: {0}, Count:{1}", 
                    group.DepartmentID,
                    group.Count);
                foreach (var employee in group.Employees)
                {
                    Console.WriteLine("\t{0}:{1}", employee.DepartmentID, employee.Name);
                }
            }

        }

        private static void UseLetKeyword()
        {
            var repository = new EmployeeRepository();
            
            var query =
                from e in repository.GetAll()
                let lname = e.Name.ToLower()
                where lname == "scott"
                select lname;

        }

        private static void SimpleEmployeeQueries()
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
