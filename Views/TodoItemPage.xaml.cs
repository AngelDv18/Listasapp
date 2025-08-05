using TodoApp.ViewModels;

namespace TodoApp.Views
{
    public partial class TodoItemPage : ContentPage
    {
        public TodoItemPage(TodoItemViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}