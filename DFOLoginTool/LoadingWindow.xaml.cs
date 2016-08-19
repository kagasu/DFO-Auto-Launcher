using Codeplex.Data;
using Microsoft.Win32;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace DFOLoginTool
{
    /// <summary>
    /// LoadingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    var dfoDirectory = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Neople_DFO", "Path", "");
                    var dataJson = DynamicJson.Parse(File.ReadAllText("data.json", new UTF8Encoding()));
                    var wc = new WebClientEx();

                    if (dataJson["proxy"] != "")
                    {
                        wc.Proxy = new WebProxy(string.Format("http://{0}/", dataJson["proxy"]));
                    }

                    var data = new NameValueCollection();

                    data.Add("email", dataJson["id"]);
                    data.Add("password", dataJson["password"]);
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0");

                    var res = wc.UploadValues("https://member.dfoneople.com/launcher/restapi/login", data);
                    var json = DynamicJson.Parse((new UTF8Encoding()).GetString(res));
                    
                    wc.Headers.Add("Host", "member.dfoneople.com");
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0");
                    wc.Headers.Add("Referer", @"https://member.dfoneople.com/launcher/login");

                    string str = wc.DownloadString(string.Format("https://member.dfoneople.com/{0}", json["nextUrl"]));
                    var reg = new Regex("dfoglobal://([0-9a-zA-Z]{1,})/([0-9a-zA-Z]{1,})/([0-9a-zA-Z]{1,})/");
                    var regFound = reg.Match(str).Groups;

                    var launchArg = string.Format("9?52.0.226.21?7101?{0}?{1}?0?0?0?0?0?2?0?0?0?0?0?0?0?0?0?0", regFound[2].Value, regFound[3].Value);
                    
                    var result = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = string.Format(@"{0}DFO.exe", dfoDirectory),
                            Arguments = launchArg,
                            WorkingDirectory = dfoDirectory
                        }
                    }.Start();

                    if (result)
                    {
                        Environment.Exit(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Environment.Exit(0);
                }
            }).Start();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
