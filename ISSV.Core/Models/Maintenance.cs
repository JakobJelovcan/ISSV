using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ISSV.Core.Models
{
    public class Maintenance : INotifyPropertyChanged
    {
        public Maintenance(int id, int deviceId, DateTime date, string reason, string workDone, string notes, string workOrder, string repairman, bool regularMaintenance)
        {
            Id = id;
            DeviceId = deviceId;
            Date = date;
            Reason = reason;
            WorkDone = workDone;
            Notes = notes;
            WorkOrder = workOrder;
            Repairman = repairman;
            RegularMaintenance = regularMaintenance;
        }

        public int Id { get; private set; }

        public int DeviceId { get; private set; }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    RaisePropertyChanged(nameof(Date));
                }
            }
        }
        private DateTime date;

        public string Reason
        {
            get { return reason; }
            set
            {
                if (reason != value)
                {
                    reason = value;
                    RaisePropertyChanged(nameof(Reason));
                }
            }
        }
        private string reason;

        public string WorkDone
        {
            get { return workDone; }
            set
            {
                if (workDone != value)
                {
                    workDone = value;
                    RaisePropertyChanged(nameof(WorkDone));
                }
            }
        }
        private string workDone;

        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    RaisePropertyChanged(nameof(Notes));
                }
            }
        }
        private string notes;

        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                if (workOrder != value)
                {
                    workOrder = value;
                    RaisePropertyChanged(nameof(WorkOrder));
                }
            }
        }
        private string workOrder;

        public string Repairman
        {
            get { return repairman; }
            set
            {
                if (repairman != value)
                {
                    repairman = value;
                    RaisePropertyChanged(nameof(Repairman));
                }
            }
        }
        private string repairman;

        public bool RegularMaintenance
        {
            get { return regularMaintenance; }
            set
            {
                if (regularMaintenance != value)
                {
                    regularMaintenance = value;
                    RaisePropertyChanged(nameof(RegularMaintenance));
                }
            }
        }
        private bool regularMaintenance;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
