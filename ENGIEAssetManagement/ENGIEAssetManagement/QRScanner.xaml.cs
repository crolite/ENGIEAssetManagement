using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ENGIEAssetManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScanner : ContentPage 
    {
        //Create instance of the connector class
        FirebaseConnector firebaseStorageConn = new FirebaseConnector();
        public QRScanner() 
        {
            InitializeComponent();
            
        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                scanResultText.Text = result.Text;
            });
        }

        private async void On_Click(object sender, EventArgs e)
            //Method to be used when button pressed on QR Scanner page
        {
            //Use the firebase connection to get a download link for the file, using the scanned result as the filename to search for
            string path = await firebaseStorageConn.GetFile(scanResultText.Text);
            //Check if the file was found
            if (path != null)
            {
                //Show user the download link on the page, then open a browser page to begin file download
                scanResultText.Text = path;
                await Browser.OpenAsync(path, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                //Show the uset the scanned QR code does not relate to a file
                scanResultText.Text = "Invalid QR Code";
            }
        }
    }
}