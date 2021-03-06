﻿//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
    public class AppBarItemTrigger : TriggerBase<PhoneApplicationPage>
    {
        private IApplicationBarMenuItem _item;

        public AppBarItemType Type { get; set; }

        // This property is used to look up the target app bar item during initialization.
        public string Id { get; set; }

        #region Text dependency property

        // Use this property to bind the Text of an app bar button/menu item to a property of the view model.
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AppBarItemTrigger), new PropertyMetadata(TextPropertyChanged));

        private void ChangeText()
        {
            if (_item != null && !String.IsNullOrWhiteSpace(Text))
            {
                _item.Text = Text;
            }
        }

        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemTrigger)sender).ChangeText();
        }

        #endregion

        #region IsEnabled dependency property

        // true if the button is enabled; otherwise, false. The default value is true,
        // the same as that of ApplicationBarIconButton/ApplicationBarMenuItem.
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(AppBarItemTrigger), new PropertyMetadata(true, IsEnablePropertyChanged));

        private void ChangeIsEnabled()
        {
            if (_item != null)
            {
                _item.IsEnabled = IsEnabled;
            }
        }

        private static void IsEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemTrigger)sender).ChangeIsEnabled();
        }

        #endregion

        #region IconUri dependency property

        // Use this property to bind the IconUri of an app bar button/menu item to a property of the view model.
        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register("IconUri", typeof(Uri), typeof(AppBarItemTrigger), new PropertyMetadata(IconUriPropertyChanged));

        private void ChangeIconUri()
        {
            if (Type == AppBarItemType.Button && _item != null && IconUri != null)
            {
                ((IApplicationBarIconButton)_item).IconUri = IconUri;
            }
        }

        private static void IconUriPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemTrigger)sender).ChangeIconUri();
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            _item = AssociatedObject.ApplicationBar.FindItem(Type, Id);

            if (_item != null)
            {
                _item.Click += ItemClick;
                _item.IsEnabled = IsEnabled;

                // Do not change the Text of an app bar item if DisplayText is empty,
                // otherwise you'll get an InvalidOperationException/ArgumentNullException.
                if (!String.IsNullOrWhiteSpace(Text))
                {
                    _item.Text = Text;
                }

                // IconUri is only for app bar button, so this property will only be set when
                // the type of the item is AppBarItemType.Button.
                // Note: IconUri of the original app bar button cannot be null (otherwise ArgumentNullException),
                // and hence the need to make sure IconUri dependency property not null.
                if (Type == AppBarItemType.Button && IconUri != null)
                {
                    ((IApplicationBarIconButton)_item).IconUri = IconUri;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_item != null)
            {
                _item.Click -= ItemClick;
                // Uncomment the following line of code if you need to restore
                // the Text of app bar item after this behavior is detached.
                //_item.Text = ItemText;
            }
        }

        private void ItemClick(object sender, EventArgs e)
        {
            InvokeActions(null);
        }
    }
}
