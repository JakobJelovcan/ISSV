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
                }
            }
        }
        private bool active;

        [NotMapped]
        public int NumberOfLocations => Locations.Count;

        [NotMapped]
        public int NumberOfDevices => Locations.Sum(l => l.Devices.Count);

        [NotMapped]
        public int NumberOfRequiredMaintenances => Locations.Sum(l => l.NumberOfRequiredMaintenances);

        [NotMapped]
        public bool RequiresMaintenances => NumberOfRequiredMaintenances > 0;

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

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
