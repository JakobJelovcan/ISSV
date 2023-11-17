﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ISSV.Core.Models
{
    public class Device : INotifyPropertyChanged
    {
        public Device(int id, int locationId, string deviceType, string serialNumber, bool active, DateTimeOffset maintenanceFrequency, DateTimeOffset warrantyPeriod, DateTime installationDate)
        {
            Id = id;
            LocationId = locationId;
            DeviceType = deviceType;
            SerialNumber = serialNumber;
            Active = active;
            MaintenanceFrequency = maintenanceFrequency;
            WarrantyPeriod = warrantyPeriod;
            InstallationDate = installationDate;
        }

        public int Id { get; private set; }

        public int LocationId { get; private set; }

        public string DeviceType
        {
            get { return deviceType; }
            set
            {
                if (deviceType != value)
                {
                    deviceType = value;
                    RaisePropertyChanged(nameof(DeviceType));
                }
            }
        }
        private string deviceType;

        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                if (serialNumber != value)
                {
                    serialNumber = value;
                    RaisePropertyChanged(nameof(SerialNumber));
                }
            }
        }
        private string serialNumber;

        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    RaisePropertyChanged(nameof(Active));
                }
            }
        }
        private bool active;

        public DateTimeOffset MaintenanceFrequency
        {
            get { return maintenanceFrequency; }
            set
            {
                if (maintenanceFrequency != value)
                {
                    maintenanceFrequency = value;
                    RaisePropertyChanged(nameof(MaintenanceFrequency));
                }
            }
        }
        private DateTimeOffset maintenanceFrequency;

        public DateTimeOffset WarrantyPeriod
        {
            get { return warrantyPeriod; }
            set
            {
                if (warrantyPeriod != value)
                {
                    warrantyPeriod = value;
                    RaisePropertyChanged(nameof(WarrantyPeriod));
                }
            }
        }
        private DateTimeOffset warrantyPeriod;

        public DateTime InstallationDate
        {
            get { return installationDate; }
            set
            {
                if (installationDate != value)
                {
                    installationDate = value;
                    RaisePropertyChanged(nameof(InstallationDate));
                }
            }
        }
        private DateTime installationDate;

        public ObservableCollection<Maintenance> Maintenances { get; private set; }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
