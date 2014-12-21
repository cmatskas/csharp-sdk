using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using relayr_csharp_sdk;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ClientTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            testClient();
        }

        public async void testClient()
        {
            //HttpManager.Instance.OauthToken = "gl2wuz7OK.Pl_s_-gUOnmj.Ge_ZV.Y4K";

            //dynamic x = await HttpManager.Instance.PerformHttpOperation(HttpManager.httpOperation.UserGetInfo, null, null);

            //dynamic x = await HttpManager.Instance.PerformHttpOperation(HttpManager.httpOperation.DevicesUnderASpecificUserWithMeaning,
            //    new string[] { "a3533de9-9448-4f4f-828c-33d990909097", "temperature" });            

            //dynamic x = await HttpManager.Instance.PerformHttpOperation(HttpManager.httpOperation.DevicesUnderASpecificUser,
            //    new string[] { "21bf95ec-b515-4a41-b6b5-fcde98a51e3f" }, null);

            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("name", "Living room thermometer");
            content.Add("description", "This is a special humidity sensor");
            content.Add("public", "135");

            //dynamic x = await HttpManager.Instance.PerformHttpOperation(HttpManager.httpOperation.DevicesUpdateAttributes,
            //    new string[] { "a3533de9-9448-4f4f-828c-33d990909097" }, content);
        }
    }
}
