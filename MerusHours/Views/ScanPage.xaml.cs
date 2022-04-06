using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MerusHours.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        ZXingBarcodeImageView imageViewTest;
        public ScanPage()
        {
            InitializeComponent();
            /*var stackLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            imageViewTest = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingBarcodeImageView"
            };            
            imageViewTest.BarcodeValue = "ZXing.Net.Mobile";
            var text = new Entry
            {
                Text = "TESTENTRY"
            };
            stackLayout.Children.Add(imageViewTest);
            stackLayout.Children.Add(text);
            Content = stackLayout;*/
        }

        private async void StartScanButton_Clicked(object sender, EventArgs e)
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            if (result != null) await DisplayAlert("Scanned!", "Text in QR code was " + result.Text, "Close");
            else
            {

            }
        }
    }
}