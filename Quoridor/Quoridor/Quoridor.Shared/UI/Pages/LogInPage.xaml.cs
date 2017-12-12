using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Quoridor.Events;

namespace Quoridor.UI
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage
    {
        private readonly QuoridorEventDispatcher _dispatcher;

        public LogInPage()
        {
            InitializeComponent();
            _dispatcher = QuoridorEventDispatcher.getInstance();
            if (!_dispatcher.IsRunning())
            {
                _dispatcher.Start();
            }
            _dispatcher.LoggedIn += _dispatcher_LoggedIn;
        }

        private void _dispatcher_LoggedIn(object sender, LogInEvent e)
        {
            var dispatcher = UsernameTextBox.Dispatcher;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.IsLogged)
                {
                    (Window.Current.Content as Frame).Navigate(typeof (ChooseGameTypePage), e.Me);
                }
                else
                {
                    var md = new MessageDialog("Failed to log in");
                    md.ShowAsync();
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogMeIn();
        }

        private void LogMeIn()
        {
            _dispatcher.LogIn(UsernameTextBox.Text, PasswordBox.Password);
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            SwitchBtnEnablement();
            SetStatus("Registration request sent. Awaiting response");
            var task =  _dispatcher.QuoridorWebService.Register(UsernameTextBox.Text, PasswordBox.Password);
            var response = await task;
            String content = (response == "Success") ? "Success. You may now log in" : response;
            var md = new MessageDialog(content);
            md.ShowAsync();
            SetStatus("");
            SwitchBtnEnablement();;
        }

        private void SetStatus(String text)
        {
            Status.Text = text;
        }

        private void SwitchBtnEnablement()
        {
            LogInBtn.IsEnabled = !LogInBtn.IsEnabled;
            RegisterBtn.IsEnabled = !RegisterBtn.IsEnabled;
        }
    }
}