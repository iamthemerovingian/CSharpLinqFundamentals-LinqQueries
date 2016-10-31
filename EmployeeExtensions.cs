using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqQueries;

namespace EmployeeExtensionsWrong
{
    public static class EmployeeExtensions
    {
        public static IEnumerable<Employee> Where(
                                           this IEnumerable<Employee> sequence, 
                                           Func<Employee, bool> predicate)
        {
            List<Employee> list = new List<Employee>();
            foreach(Employee employee in sequence)
            {
                if (predicate(employee))
                {
                    list.Add(employee);
                }
            }
            return list; // This method will make the where keyword do work every time it is called. This is greedy.
        }
    }
}

namespace EmployeeExtensions
{
    public static class EmployeeExtensions
    {
        public static IEnumerable<Employee> Where(this IEnumerable<Employee> sequence,Func<Employee, bool> predicate)
        {
            foreach (Employee e in sequence)
            {
                if (predicate(e))
                {
                    yield return e; //This yield return allows the code to be lazy and only return when the query is excecuted and not when it is called. This is called deffered excecution. This is lazy.
                }
            }
        }
    }
}