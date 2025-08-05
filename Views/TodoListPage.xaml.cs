using TodoApp.ViewModels;

namespace TodoApp.Views
{
    public partial class TodoListPage : ContentPage
    {
        TodoListViewModel _viewModel;

        public TodoListPage(TodoListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}