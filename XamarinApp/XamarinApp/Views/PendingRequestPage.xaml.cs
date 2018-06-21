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
	public partial class PendingRequestPage : ContentPage
	{
        public PendingRequestViewModel _viewModel;
		public PendingRequestPage ()
		{
            _viewModel = new PendingRequestViewModel();
			InitializeComponent ();
            BindingContext = _viewModel;
        }

	}
}