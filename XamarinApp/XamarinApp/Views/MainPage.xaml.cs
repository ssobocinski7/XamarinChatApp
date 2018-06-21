﻿using System;
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
	public partial class MainPage : ContentPage
	{
        public MainViewModel _viewModel;
		public MainPage()
		{
            _viewModel = new MainViewModel();
			InitializeComponent();
            BindingContext = _viewModel;
        }
	}
}