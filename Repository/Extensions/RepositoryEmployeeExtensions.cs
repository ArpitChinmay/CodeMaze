﻿using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        {
            return employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));
        }

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            /*  
             *  Check if query string is empty, if yes, by default, order results by employee name. 
             */
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            { return employees.OrderBy(e => e.Name); }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if(string.IsNullOrWhiteSpace(orderQuery))
            { return employees.OrderBy(e => e.Name); }

            return employees.OrderBy(orderQuery);
        }
    }
}
