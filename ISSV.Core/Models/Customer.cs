using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool DisplayOnMap
        {
            get { return displayOnMap; }
            set
            {
                if (displayOnMap != value)
                {
                    displayOnMap = value;
                    RaisePropertyChanged(nameof(DisplayOnMap));
                }
            }
        }
        private bool displayOnMap;

        public List<Location> Locations { get; private set; }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
