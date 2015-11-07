﻿using Caliburn.Micro;
using InstantDelivery.Core.Entities;
using System;
using InstantDelivery.Services;

namespace InstantDelivery.ViewModel
{
    public class VehiclesAddViewModel : Screen
    {
        public IVehiclesService service;

        public VehiclesAddViewModel(IVehiclesService service)
        {
            NewVehicle = new Vehicle();
            this.service = service;
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
                NewVehicle = null;
        }

        public override void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        private Vehicle newVehicle;
        public Vehicle NewVehicle
        {
            get { return newVehicle; }
            set
            {
                newVehicle = value;
                NotifyOfPropertyChange();
            }
        }


        public void Save()
        {
            service.AddVehicle(NewVehicle);
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}