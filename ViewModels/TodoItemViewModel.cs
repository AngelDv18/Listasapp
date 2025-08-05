using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class TodoItemViewModel : BaseViewModel
    {
        TodoItemDatabase database;

        public string ItemId
        {
            set
            {
                LoadItemId(value);
            }
        }

        string name = string.Empty;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string notes = string.Empty;
        public string Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        bool done;
        public bool Done
        {
            get => done;
            set => SetProperty(ref done, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TodoItemViewModel(TodoItemDatabase database)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.database = database;
            this.PropertyChanged += (_, __) => ((Command)SaveCommand).ChangeCanExecute();
        }

        bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        async void OnSave()
        {
            TodoItem newItem = new TodoItem()
            {
                ID = Id,
                Name = Name,
                Notes = Notes,
                Done = Done
            };

            await database.SaveItemAsync(newItem);
            await Shell.Current.GoToAsync("..");
        }

        async void LoadItemId(string itemId)
        {
            try
            {
                var item = await database.GetItemAsync(Convert.ToInt32(itemId));
                Id = item.ID;
                Name = item.Name;
                Notes = item.Notes;
                Done = item.Done;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }
    }
}
