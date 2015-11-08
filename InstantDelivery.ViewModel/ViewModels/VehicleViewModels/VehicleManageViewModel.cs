﻿using Caliburn.Micro;
using InstantDelivery.Core.Entities;
using InstantDelivery.Services;
using InstantDelivery.ViewModel.ViewModels.EmployeesViewModels;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InstantDelivery.ViewModel
{
    /// <summary>
    ///  Model widoku zarządzania pojazdami.
    /// </summary>
    public class VehicleManageViewModel : EmployeesViewModelBase
    {
        private readonly IEmployeeService employeesService;
        private readonly IVehiclesService vehiclesService;
        private readonly IWindowManager windowManager;
        private Employee selectedEmployee;
        /// <summary>
        /// Konstruktor modelu widoku
        /// </summary>
        /// <param name="employeesService"></param>
        /// <param name="windowManager"></param>
        /// <param name="vehiclesService"></param>
        public VehicleManageViewModel(IEmployeeService employeesService, IWindowManager windowManager, IVehiclesService vehiclesService)
        {
            this.employeesService = employeesService;
            this.windowManager = windowManager;
            this.vehiclesService = vehiclesService;
            Employees = employeesService.GetAll();
        }

        /// <summary>
        /// Flaga informująca o tym czy zaznaczony jest jakiś wiersz.
        /// </summary>
        public bool IsSelectedAnyRow
        {
            get { return SelectedEmployee != null; }
        }
        /// <summary>
        /// Metoda zwraca kolekcję wszystkich pracowników.
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<Employee> GetEmployees()
        {
            return employeesService.GetAll();
        }
        /// <summary>
        /// Aktualnie zaznaczony wiersz w tabeli danych.
        /// </summary>
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                NotifyOfPropertyChange();
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSelectedAnyRow)));
            }
        }
        /// <summary>
        /// Delegat zdarzenia kliknięcia w przycisk przechodzący do widoku edycji pojazdu.
        /// </summary>
        public async void EditVehicleForEmployee()
        {
            if (SelectedEmployee == null)
            {
                return;
            }
            var result = windowManager.ShowDialog(new SelectVehicleForEmployeeViewModel(employeesService, vehiclesService)
            {
                SelectedEmployee = SelectedEmployee,
                SelectedVehicle = SelectedEmployee.Vehicle,
                Vehicles = vehiclesService.GetAllAvailableAndCurrent(SelectedEmployee.Vehicle),
                HasVehicle = SelectedEmployee.Vehicle != null
            });
            await Task.Run(() =>
            {
                if (result != true)
                {
                    employeesService.Reload(SelectedEmployee);
                }
                else
                {
                    employeesService.Save();
                }
            });
        }

    }
}