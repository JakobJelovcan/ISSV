using ISSV.Core.Helpers;
using ISSV.Core.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ISSV.Core.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public Customer()
        {
            Locations = new List<Location>();
        }

        public Customer(string name, string phoneNumber, string email, bool active)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Active = active;
            Locations = new List<Location>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        private string name;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    RaisePropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        private string phoneNumber;

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }
        private string email;

        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    RaisePropertyChanged(nameof(Active));
                    if (!active) Locations.ForEach(l => l.Active = false);
                }
            }
        }
        private bool active;

        [NotMapped]
        public int NumberOfLocations => Locations.Count;

        [NotMapped]
        public int NumberOfDevices => Locations.Sum(l => l.Devices.Count);

        [NotMapped]
        public int NumberOfRequiredMaintenances => Locations.Where(l => l.Active).Sum(l => l.NumberOfRequiredMaintenances);

        [NotMapped]
        public bool RequiresMaintenances => (NumberOfRequiredMaintenances > 0) && Active;

        public List<Location> Locations { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is Customer customer)
            {
                return customer.Id == this.Id;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode() => base.GetHashCode();

        public void AddLocation(Location location)
        {
            Locations.Add(location);
            RaisePropertyChanged(nameof(NumberOfLocations), nameof(NumberOfDevices), nameof(NumberOfRequiredMaintenances), nameof(RequiresMaintenances));
        }

        internal void RemoveLocation(Location location)
        {
            Locations.Remove(location);
            RaisePropertyChanged(nameof(NumberOfLocations), nameof(NumberOfDevices), nameof(NumberOfRequiredMaintenances), nameof(RequiresMaintenances));
        }

        public void Update(string name, string phoneNumber, string email, bool active)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Active = active;
        }

        public void Delete()
        {
            Locations.ToArray().ForEach(l => l.Delete());
            DataService.Customers.Remove(this);
        }

        private void RaisePropertyChanged(params string[] args)
        {
            args.ForEach(a => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a)));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
