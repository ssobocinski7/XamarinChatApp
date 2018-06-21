using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddFriendPage : ContentPage
	{
        public AddFriendViewModel _viewModel;
        public AddFriendPage()
        {
            _viewModel = new AddFriendViewModel();
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}
