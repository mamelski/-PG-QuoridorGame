﻿

#pragma checksum "C:\Users\Jakub\OneDrive\Projekt Grupowy\quoridor-projekt-grupowy\Quoridor\Quoridor\Quoridor.Shared\UI\Pages\AvailableUsersListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D90E062706D30103B69220E9F9C08F52"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quoridor.UI
{
    partial class AvailableUsersListPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView UsersListView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button RefreshListButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button InvitePlayerButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView InvitationsListView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button StartGameButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///UI/Pages/AvailableUsersListPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            UsersListView = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("UsersListView");
            RefreshListButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("RefreshListButton");
            InvitePlayerButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("InvitePlayerButton");
            InvitationsListView = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("InvitationsListView");
            StartGameButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("StartGameButton");
        }
    }
}



