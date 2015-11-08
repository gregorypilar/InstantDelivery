﻿using InstantDelivery.Core;
using InstantDelivery.Core.Entities;
using System.Linq;

namespace InstantDelivery.Services
{
    /// <summary>
    /// Warstwa serwisu pracowników
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly InstantDeliveryContext context;
        /// <summary>
        /// Konstruktor warstwy serwisu
        /// </summary>
        /// <param name="context"></param>
        public EmployeeService(InstantDeliveryContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Zmienia pojazd przypisany do pracownika
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="selectedVehicle"></param>
        public void ChangeEmployeesVehicle(Employee employee, Vehicle selectedVehicle)
        {
            var owner = context.Employees.FirstOrDefault(o => o.Id == employee.Id);
            var vehicle = selectedVehicle == null ? null :
                            context.Vehicles.FirstOrDefault(c => c.Id == selectedVehicle.Id);
            if (owner != null)
            {
                owner.Vehicle = vehicle;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Zwraca wszystkich pracowników z bazy danych
        /// </summary>
        /// <returns></returns>
        public IQueryable<Employee> GetAll()
        {
            return context.Employees;
        }
        /// <summary>
        /// Aktualizuje dane pracownika
        /// </summary>
        /// <param name="employee"></param>
        public void Reload(Employee employee)
        {
            context.Entry(employee).Reload();
        }
        /// <summary>
        /// Usuwa pracownika z bazy danych
        /// </summary>
        /// <param name="employee"></param>
        public void RemoveEmployee(Employee employee)
        {
            foreach (var package in employee.Packages
                .Where(p => p.Status == PackageStatus.InDelivery))
            {
                package.Status = PackageStatus.New;
            }
            context.Employees.Remove(employee);
            context.SaveChanges();
        }
        /// <summary>
        /// Dodaje pracownika do bazy danych
        /// </summary>
        /// <param name="employee"></param>
        public void AddEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
        }
        /// <summary>
        /// Zapisuje aktualne zmiany
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }
    }
}