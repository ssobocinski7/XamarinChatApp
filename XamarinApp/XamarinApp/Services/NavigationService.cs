using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Views;

namespace XamarinApp.Services
{
    public class NavigationService
    {
        public async static Task NavigateTo(Page page)
        {
            await Application.Current.MainPage.FadeTo(0.3, 400);
            await Application.Current.MainPage.Navigation.PushAsync(page);
            await Application.Current.MainPage.FadeTo(1.0, 400);
        }
        public async static Task NavigateModalTo(Page page)
        {
            await Application.Current.MainPage.FadeTo(0.3, 400);
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            await Application.Current.MainPage.FadeTo(1.0, 400);
        }
        public async static Task BackToRecentPage()
        {
            await Application.Current.MainPage.FadeTo(0.3, 400);
            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.FadeTo(1.0, 400);
        }
    }
}
