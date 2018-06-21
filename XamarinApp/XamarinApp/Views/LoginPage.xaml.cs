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
	public partial class LoginPage : ContentPage
	{
        public LoginViewModel _viewModel;
		public LoginPage ()
		{
            _viewModel = new LoginViewModel();
			InitializeComponent();
            BindingContext = _viewModel;
		}
	}
}