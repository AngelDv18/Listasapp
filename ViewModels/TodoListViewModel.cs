using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Views;

namespace TodoApp.ViewModels
{
    public class TodoListViewModel : BaseViewModel
    {
        TodoItemDatabase database;
        public ObservableCollection<TodoItem> Items { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ItemTappedCommand { get; }
        public ICommand ToggleItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public TodoListViewModel(TodoItemDatabase database)
        {
            Title = "Todo List";
            Items = new ObservableCollection<TodoItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(OnAddItem);
            ItemTappedCommand = new Command<TodoItem>(OnItemSelected);
            ToggleItemCommand = new Command<TodoItem>(async (item) => await OnToggleItem(item));
            DeleteItemCommand = new Command<TodoItem>(async (item) => await OnDeleteItem(item));

            this.database = database;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await database.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(TodoItemPage));
        }

        async void OnItemSelected(TodoItem item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(TodoItemPage)}?{nameof(TodoItemViewModel.ItemId)}={item.ID}");
        }

        async Task OnToggleItem(TodoItem item)
        {
            if (item == null)
                return;

            await database.ToggleItemAsync(item);
            //await ExecuteLoadItemsCommand();
        }

        async Task OnDeleteItem(TodoItem item)
        {
            if (item == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert("Confirmar",
                $"¿Eliminar '{item.Name}'?", "Sí", "No");

            if (confirm)
            {
                await database.DeleteItemAsync(item);
                Items.Remove(item);
                //await ExecuteLoadItemsCommand();
            }
        }
    }
}
