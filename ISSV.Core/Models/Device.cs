using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISSV.Core.Models
{
    public class Device : INotifyPropertyChanged
    {
        public Device()
        {
            Maintenances = new List<Maintenance>();
        }

        public Device(Location location, string deviceType, string serialNumber, bool active, int maintenanceFrequency, int warrantyPeriod, DateTimeOffset installationDate)
        {
            Location = location;
            DeviceType = deviceType;
            SerialNumber = serialNumber;
            Active = active;
            MaintenanceFrequency = maintenanceFrequency;
            WarrantyPeriod = warrantyPeriod;
            InstallationDate = installationDate;
            Maintenances = new List<Maintenance>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public Location Location { get; private set; }

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

        public int MaintenanceFrequency
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
        private int maintenanceFrequency;

        public int WarrantyPeriod
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
        private int warrantyPeriod;

        public DateTimeOffset InstallationDate
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
        private DateTimeOffset installationDate;

        public List<Maintenance> Maintenances { get; private set; }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
