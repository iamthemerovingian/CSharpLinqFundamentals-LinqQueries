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
        }

        private static void SingleEmployeeQueries()
        {
            var query1 =
                from e in new EmployeeRepository().GetAll()
                where e.DepartmentID < 3 && e.ID < 10
                orderby e.DepartmentID descending,
                e.Name ascending
                select e;

            var query2 =
                new EmployeeRepository().GetAll()
                .Where(e => e.DepartmentID < 3 && e.ID < 10)
                .OrderByDescending(e => e.DepartmentID)
                .OrderBy(e => e.Name);

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
