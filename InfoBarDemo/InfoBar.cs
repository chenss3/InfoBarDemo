﻿/* 
 * InfoBar Contorl Prototype
 * @author Sophia Chen
 * t-soc@microsoft.com
 * 
 * C# Prototype for UWP InfoBar (Microsoft XAML Controls Team)
 * 
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Primitives;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Automation.Peers;
using Windows.ApplicationModel;
using System.ServiceModel.Channels;
using Windows.UI.Xaml.Documents;
using InfoBar;

namespace InfoBarDemo
{
    public enum InfoBarCloseReason
    {
        CloseButton,
        Programattic
    }
    public enum InfoBarSeverity
    {
        Critical,
        Warning,
        Informational,
        Success,
        Default,
        None
    }

    public class InfoBarClosedEventArgs : EventArgs
    {
        public InfoBarCloseReason Reason
        {
            get; set;
        }
    }

    public class InfoBarClosingEventArgs : EventArgs
    {
        public InfoBarCloseReason Reason
        {
            get; set;
        }
        public bool Cancel
        {
            get; set;
        }

    }
    public class CloseButtonClickEventArgs : EventArgs
    {
        public bool IsHandled
        {
            get; set;
        }
    }
    public sealed class InfoBar : ContentControl
    {
        Button _actionButton;
        Button _alternateCloseButton;
        TextBlock _title;
        TextBlock _message;
        HyperlinkButton _hyperlinkButton;
        IconSourceElement _standardIcon;
        IconSourceElement _userIcon;
        Grid _contentRootGrid;

        public event EventHandler<RoutedEventArgs> ActionButtonClick;
        public event TypedEventHandler<InfoBar, CloseButtonClickEventArgs> CloseButtonClick;
        public event TypedEventHandler<InfoBar, InfoBarClosedEventArgs> Closed;
        public event TypedEventHandler<InfoBar, InfoBarClosingEventArgs> Closing;

        private InfoBarCloseReason lastCloseReason = InfoBarCloseReason.Programattic;
        private bool alreadyRaised = false;
        private Popup pop;


        public InfoBar()
        {
            this.DefaultStyleKey = typeof(InfoBar);
        }


        T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            return child;
        }


        protected override void OnApplyTemplate()
        {
            _alternateCloseButton = GetTemplateChild<Button>("AlternateCloseButton");
            _actionButton = GetTemplateChild<Button>("ActionButton");
            _title = GetTemplateChild<TextBlock>("Title");
            _message = GetTemplateChild<TextBlock>("Message");
            _hyperlinkButton = GetTemplateChild<HyperlinkButton>("HyperlinkButton");
            _standardIcon = GetTemplateChild<IconSourceElement>("StandardIcon");
            _userIcon = GetTemplateChild<IconSourceElement>("UserIcon");
            _contentRootGrid = GetTemplateChild<Grid>("ContentRootGrid");

            UpdateButtonsState();
            UpdateSeverityState();
            OnIsOpenChanged();
            UpdateMargins();

            _alternateCloseButton.Click += new RoutedEventHandler(OnCloseButtonClick);
            _actionButton.Click += (s, e) => ActionButtonClick?.Invoke(s, e);
        }


        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyProperty property = e.Property;
            InfoBar infoBar = d as InfoBar;
            if (property == SeverityProperty)
            {
                infoBar.UpdateSeverityState();
            }
            else if (property == ActionButtonContentProperty || property == ShowCloseButtonProperty)
            {
                infoBar.UpdateButtonsState();
            }
            else if (property == IsOpenProperty)
            {
                infoBar.OnIsOpenChanged();
            }
            else if (property == IconSourceProperty)
            {
                infoBar.OnIconChanged();
            }
            else if (property == MessageProperty)
            {
                infoBar.checkMessage();
            }
            infoBar.UpdateMargins();
        }

        private void UpdateMargins()
        {
            if (_standardIcon != null)
            {
                if ((Title != null && Title != "") || (Message != null && Message != "") || ActionButtonContent != null || HyperlinkButtonContent != null || ShowCloseButton == true)
                {
                    _standardIcon.Margin = new Thickness(0, 12, 8, 12);
                }
                else
                {
                    _standardIcon.Margin = new Thickness(0, 12, 0, 12);
                }
            }

            if (_userIcon != null)
            {
                if ((Title != null && Title != "") || (Message != null && Message != "") || ActionButtonContent != null || HyperlinkButtonContent != null || ShowCloseButton == true)
                {
                    _userIcon.Margin = new Thickness(0, 12, 8, 12);
                }
                else
                {
                    _userIcon.Margin = new Thickness(0, 12, 0, 12);
                }
            }

            if (_title != null)
            {
                if (Title != null && Title != "")
                {
                    if ((_standardIcon != null || _userIcon != null) && (Message != null && Message != ""))
                    {
                        _title.Margin = new Thickness(0, 12, 8, 12);
                    }
                    else
                    {
                        _title.Margin = new Thickness(0, 12, 0, 12);
                    }
                }
                else
                {
                    _title.Margin = new Thickness(0, 0, 0, 0);
                }
            }

            if (_message != null)
            {
                if (Message != null && Message != "")
                {
                    if (ActionButtonContent != null || HyperlinkButtonContent != null)
                    {
                        _message.Margin = new Thickness(0, 12, 12, 12);
                    }
                    else
                    {
                        _message.Margin = new Thickness(0, 12, 0, 12);
                    }
                }
                else
                {
                    _message.Margin = new Thickness(0, 0, 0, 0);
                }
            }

            if (_actionButton != null)
            {
                if (ActionButtonContent != null)
                {
                    if (HyperlinkButtonContent != null)
                    {
                        _actionButton.Margin = new Thickness(0, 8, 12, 8);
                    }
                    else
                    {
                        _actionButton.Margin = new Thickness(0, 8, 4, 8);
                    }
                }
                else
                {
                    _actionButton.Margin = new Thickness(0, 0, 0, 0);
                }
            }

        }

        private void checkMessage()
        {
            if (_message != null)
            {
                if (_message.IsTextTrimmed)
                {
                    _message.TextWrapping = TextWrapping.WrapWholeWords;
                    _contentRootGrid.RowDefinitions.Add(new RowDefinition());
                    _contentRootGrid.RowDefinitions.Add(new RowDefinition());
                    _contentRootGrid.Children.Add(_message);
                    Grid.SetRow(_message, 1);
                    Grid.SetColumn(_message, 1);
                }
            }

        }


        /* Open Properties
         * 
         */
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(InfoBar), new PropertyMetadata(false, OnPropertyChanged));


        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowCloseButtonProperty =
            DependencyProperty.Register(nameof(ShowCloseButton), typeof(bool), typeof(InfoBar), new PropertyMetadata(true, OnPropertyChanged));


        /* Message Title Properties
         * 
         */
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoBar), new PropertyMetadata("", OnPropertyChanged));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InfoBar), new PropertyMetadata("", OnPropertyChanged));




        /* Action Button Properties
         * 
         */
        public object ActionButtonContent
        {
            get { return (object)GetValue(ActionButtonContentProperty); }
            set { SetValue(ActionButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null, OnPropertyChanged));


        public Style ActionButtonStyle
        {
            get { return (Style)GetValue(ActionButtonStyleProperty); }
            set { SetValue(ActionButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonStyleProperty =
            DependencyProperty.Register(nameof(ActionButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));


        public ICommand ActionButtonCommand
        {
            get { return (ICommand)GetValue(ActionButtonCommandProperty); }
            set { SetValue(ActionButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonCommandProperty =
            DependencyProperty.Register(nameof(ActionButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));


        public object ActionButtonCommandParameter
        {
            get { return (object)GetValue(ActionButtonCommandParameterProperty); }
            set { SetValue(ActionButtonCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ActionButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));

        /* Close Button Properties
         * 
         */

        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register(nameof(CloseButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));


        public ICommand CloseButtonCommand
        {
            get { return (ICommand)GetValue(CloseButtonCommandProperty); }
            set { SetValue(CloseButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));


        public object CloseButtonCommandParameter
        {
            get { return (object)GetValue(CloseButtonCommandParameterProperty); }
            set { SetValue(CloseButtonCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));


        /* Hyperlink Properties 
         * 
         */
        public Object HyperlinkButtonContent
        {
            get { return (object)GetValue(HyperlinkButtonContentProperty); }
            set { SetValue(HyperlinkButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HyperlinkButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HyperlinkButtonContentProperty =
            DependencyProperty.Register(nameof(HyperlinkButtonContent), typeof(object), typeof(InfoBar), new PropertyMetadata(null));

        public Style HyperlinkButtonStyle
        {
            get { return (Style)GetValue(HyperlinkButtonStyleProperty); }
            set { SetValue(HyperlinkButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HyperlinkButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HyperlinkButtonStyleProperty =
            DependencyProperty.Register(nameof(HyperlinkButtonStyle), typeof(Style), typeof(InfoBar), new PropertyMetadata(null));

        /* Severity-Related Properties
        * 
        */
        public InfoBarSeverity Severity
        {
            get { return (InfoBarSeverity)GetValue(SeverityProperty); }
            set { SetValue(SeverityProperty, value); }
        }

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(InfoBarSeverity), typeof(InfoBar), new PropertyMetadata(InfoBarSeverity.Default, OnPropertyChanged));

        public Color StatusColor
        {
            get { return (Color)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register(nameof(StatusColor), typeof(Color), typeof(InfoBar), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0)));


        public IconSource IconSource
        {
            get { return (IconSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(nameof(IconSource), typeof(IconSource), typeof(InfoBar), new PropertyMetadata(default, OnPropertyChanged));




        public bool Toast
        {
            get { return (bool)GetValue(ToastProperty); }
            set { SetValue(ToastProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToastProperty =
            DependencyProperty.Register(nameof(Toast), typeof(bool), typeof(InfoBar), new PropertyMetadata(false));







        // Methods that invoke the event handlers for Close Button and Action Button
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {

            lastCloseReason = InfoBarCloseReason.CloseButton;
            CloseButtonClickEventArgs args = new CloseButtonClickEventArgs();
            CloseButtonClick?.Invoke(this, args);
            if (args.IsHandled == false)
            {
                RaiseClosingEvent();
                alreadyRaised = false;
            }

        }


        void RaiseClosingEvent()
        {
            InfoBarClosingEventArgs args = new InfoBarClosingEventArgs();
            args.Reason = lastCloseReason;

            Closing?.Invoke(this, args);

            if (!args.Cancel)
            {
                alreadyRaised = true;
                IsOpen = false;
                Open(IsOpen);
            }
            else
            {
                // The developer has changed the Cancel property to true, indicating that they wish to Cancel the
                // closing of this tip, so we need to revert the IsOpen property to true.
                IsOpen = true;
            }
        }


        void RaiseClosedEvent()
        {
            InfoBarClosedEventArgs args = new InfoBarClosedEventArgs();
            args.Reason = lastCloseReason;
            Closed?.Invoke(this, args);

        }

        void OnIconChanged()
        {
            if (IconSource != null)
            {
                VisualStateManager.GoToState(this, "UserIconVisible", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "StandardIconVisible", false);
            }
        }

        // Updates Severity state of InfoBar
        void UpdateSeverityState()
        {
            OnIconChanged();
            if (Severity == InfoBarSeverity.Critical)
            {
                VisualStateManager.GoToState(this, "Critical", false);
            }
            else if (Severity == InfoBarSeverity.Warning)
            {
                VisualStateManager.GoToState(this, "Warning", false);
            }
            else if (Severity == InfoBarSeverity.Informational)
            {
                VisualStateManager.GoToState(this, "Informational", false);
            }
            else if (Severity == InfoBarSeverity.Success)
            {
                VisualStateManager.GoToState(this, "Success", false);
            }
            else if (Severity == InfoBarSeverity.None)
            {
                VisualStateManager.GoToState(this, "None", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Default", false);
            }
        }


        // Updates visibility of buttons
        void UpdateButtonsState()
        {
            if (ShowCloseButton)
            {
                if (ActionButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "BothButtonsVisible", false);
                }
                else
                {
                    VisualStateManager.GoToState(this, "CloseButtonVisible", false);
                }
            }
            else
            {
                if (ActionButtonContent != null)
                {
                    VisualStateManager.GoToState(this, "ActionButtonVisible", false);

                }
                else
                {
                    VisualStateManager.GoToState(this, "NoButtonsVisible", false);
                }
            }
        }


        // Updates if InfoBar is opened
        void OnIsOpenChanged()
        {

            if (IsOpen)
            {
                lastCloseReason = InfoBarCloseReason.Programattic;
                Open(IsOpen);

            }
            else if (!alreadyRaised)
            {

                RaiseClosingEvent();
                alreadyRaised = false;
            }


        }


        // Opens or closes the InfoBar
        private void Open(bool value)
        {
            InfoBarAutomationPeer infoBarPeer = FrameworkElementAutomationPeer.FromElement(this) as InfoBarAutomationPeer ?? null;
            if (value)
            {
                VisualStateManager.GoToState(this, "Visible", false);
                IsOpen = true;
                infoBarPeer.RaiseWindowOpenedEvent(Title + " "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      + Message);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", false);
                IsOpen = false;
                RaiseClosedEvent();
                infoBarPeer.RaiseWindowOpenedEvent("InfoBar dismissed");
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new InfoBarAutomationPeer(this);
        }

    }
}
