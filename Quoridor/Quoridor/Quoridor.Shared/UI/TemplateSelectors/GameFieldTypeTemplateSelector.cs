using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Quoridor.Model;

namespace Quoridor.UI
{
    public class GameFieldTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalTemplate { get; set; }
        public DataTemplate HorizontalWallTemplate { get; set; }

        public DataTemplate VerticalWallTemplate { get; set; }

        public DataTemplate MicroWallTemplate { get; set; }


        public new DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var field = item as BoardField;

            if (field.IsField)
                return NormalTemplate;

            if (field.IsHorizontalWall)
                return HorizontalWallTemplate;
            if (field.IsMicroWall)
                return MicroWallTemplate;
            if (field.IsVerticalWall)
                return VerticalWallTemplate;

            throw new ArgumentException();
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var field = item as BoardField;

            if (field.IsField)
                return NormalTemplate;

            if (field.IsHorizontalWall)
                return HorizontalWallTemplate;
            if (field.IsMicroWall)
                return MicroWallTemplate;
            if (field.IsVerticalWall)
                return VerticalWallTemplate;

            throw new ArgumentException();
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var field = item as BoardField;

            if (field.IsField)
                return NormalTemplate;

            if (field.IsHorizontalWall)
                return HorizontalWallTemplate;
            if (field.IsMicroWall)
                return MicroWallTemplate;
            if (field.IsVerticalWall)
                return VerticalWallTemplate;

            throw new ArgumentException();
        }
    }
}