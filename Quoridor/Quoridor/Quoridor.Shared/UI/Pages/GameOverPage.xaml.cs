using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Quoridor.DataContracts;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Quoridor.UI
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverPage : Page
    {
        private Player player;

        public GameOverPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dict = (Dictionary<Type, object>) e.Parameter;
            if (dict == null)
                throw new Exception("Wrong parameter, should be of type Dictionary<Type, object>");
            player = dict[typeof (Player)] as Player;
            RichTextBlock.Text = (string) dict[typeof (string)];
        }


        private void TryAgainButton_OnClick(object sender, RoutedEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof (ChooseGameTypePage), player);
        }
    }
}