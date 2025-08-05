using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using SQLite;

namespace TodoApp.Models
{
    public class TodoItem : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        string notes = string.Empty;
        public string Notes
        {
            get => notes;
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        bool done;
        public bool Done
        {
            get => done;
            set
            {
                if (done != value)
                {
                    done = value;
                    CompletedDate = done ? DateTime.Now : null;
                    OnPropertyChanged(nameof(Done));
                    OnPropertyChanged(nameof(CompletedDate));
                }
            }
        }

        DateTime createdDate = DateTime.Now;
        public DateTime CreatedDate
        {
            get => createdDate;
            set
            {
                if (createdDate != value)
                {
                    createdDate = value;
                    OnPropertyChanged(nameof(CreatedDate));
                }
            }
        }

        DateTime? completedDate;
        public DateTime? CompletedDate
        {
            get => completedDate;
            set
            {
                if (completedDate != value)
                {
                    completedDate = value;
                    OnPropertyChanged(nameof(CompletedDate));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
