﻿

#pragma checksum "C:\Users\Jakub\OneDrive\Projekt Grupowy\quoridor-projekt-grupowy\Quoridor\Quoridor\Quoridor.Shared\UI\Pages\GamePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D3B6F55D6B625D55AED520895F867790"
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
    partial class GamePage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Quoridor.Converters.BoardElementTypeToColorConverter BoardElementTypeToColorConverter; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Quoridor.UI.BoardSizeProvider boardSizeProvider; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Quoridor.UI.GameFieldTypeTemplateSelector FieldTypeTemplateSelector; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBox StatusTextBox; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ItemsControl GameBoardView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///UI/Pages/GamePage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            BoardElementTypeToColorConverter = (global::Quoridor.Converters.BoardElementTypeToColorConverter)this.FindName("BoardElementTypeToColorConverter");
            boardSizeProvider = (global::Quoridor.UI.BoardSizeProvider)this.FindName("boardSizeProvider");
            FieldTypeTemplateSelector = (global::Quoridor.UI.GameFieldTypeTemplateSelector)this.FindName("FieldTypeTemplateSelector");
            StatusTextBox = (global::Windows.UI.Xaml.Controls.TextBox)this.FindName("StatusTextBox");
            GameBoardView = (global::Windows.UI.Xaml.Controls.ItemsControl)this.FindName("GameBoardView");
        }
    }
}


