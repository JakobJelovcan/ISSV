using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ISSV.Core.Models
{
    public class Location : INotifyPropertyChanged
    {
        public Location()
        {
            Devices = new List<Device>();
        }

        public Location(Customer customer, string name, Address address, string phoneNumber, string email, bool active)
        {
            Customer = customer;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Active = active;
            Devices = new List<Device>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public Customer Customer { get; private set; }

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

        public Address Address
        {
            get => address;
            set
            {
                if (address != value)
                {
                    address = value;
                    RaisePropertyChanged(nameof(Address));
                }
            }
        }
        private Address address;

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
        public int NumberOfDevices => Devices.Count;

        [NotMapped]
        public int NumberOfRequiredMaintenances => Devices.Where(d => d.RequiresMaintenance && d.Active).Count();

        [NotMapped]
        public bool RequiresMaintenances => (NumberOfRequiredMaintenances > 0) && Active;

        public List<Device> Devices { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is Location location)
            {
                return location.Id == this.Id;
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
