using ISSV.Core.Helpers;
using ISSV.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [NotMapped]
        public int NumberOfMaintenances => Maintenances.Count;

        [NotMapped]
        public DateTimeOffset LastMaintenance => Maintenances.Where(m => m.RegularMaintenance).MaxOr(m => m.Date, InstallationDate);

        [NotMapped]
        public bool RequiresMaintenance => (LastMaintenance.AddMonths(maintenanceFrequency) > DateTimeOffset.Now) && Active;

        public List<Maintenance> Maintenances { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is Device device)
            {
                return device.Id == this.Id;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode() => base.GetHashCode();

        public void AddMaintenance(Maintenance maintenance)
        {
            Maintenances.Add(maintenance);
            //TODO: Raise property changed
        }

        public void RemoveMaintenance(Maintenance maintenance)
        {
            Maintenances.Remove(maintenance);
            //TODO: Raise property changed
        }

        public void Update(string deviceType, string serialNumber, bool active, int maintenanceFrequency, int warrantyPeriod, DateTimeOffset installationDate)
        {
            DeviceType = deviceType;
            SerialNumber = serialNumber;
            Active = active;
            MaintenanceFrequency = maintenanceFrequency;
            WarrantyPeriod = warrantyPeriod;
            InstallationDate = installationDate;
        }

        public void Delete()
        {
            Maintenances.ToArray().ForEach(m => m.Delete());
            DataService.Devices.Remove(this);
            Location.RemoveDevice(this);
        }

        private void RaisePropertyChanged(params string[] args)
        {
            args.ForEach(a => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a)));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
