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
	public partial class RegisterPage : ContentPage
	{
        private RegisterViewModel _viewModel;
		public RegisterPage ()
		{
            _viewModel = new RegisterViewModel();
			InitializeComponent ();
            BindingContext = _viewModel;
        }
	}
}