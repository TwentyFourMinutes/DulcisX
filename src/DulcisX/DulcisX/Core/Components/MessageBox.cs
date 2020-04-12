using DulcisX.Core.Enums;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Xaml.Behaviors;
using StringyEnums;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace DulcisX.Core
{
    public static class MessageBox
    {
        private class BaseMessageBox : DialogWindow
        {
            internal MessageBoxButtons Value { get; private set; }
            internal bool Canceled { get; private set; }

            private readonly MessageBoxModel _messageContext;

            private static bool _initialized;

            internal BaseMessageBox(string title, string content, MessageBoxButtons messageBoxButtons, Icon icon)
            {
                _messageContext = new MessageBoxModel(messageBoxButtons, icon)
                {
                    Title = title,
                    Content = content
                };

                _messageContext.OnResult += OnBaseResult;
                _messageContext.OnDrag += OnBaseDrag;
                _messageContext.OnCancel += OnBaseCancel;

                this.DataContext = _messageContext;

                Load(this);

                this.ShowModal();
            }

            private void OnBaseResult(MessageBoxButtons button)
            {
                Value = button;

                MessageBoxClosing();
            }

            private void OnBaseDrag()
            {
                this.DragMove();
            }
            private void OnBaseCancel()
            {
                Canceled = true;

                MessageBoxClosing();
            }

            private void MessageBoxClosing()
            {
                _messageContext.OnResult -= OnBaseResult;
                _messageContext.OnDrag += OnBaseDrag;
                _messageContext.OnCancel += OnBaseCancel;
                this.Close();
            }

            private static void Load(BaseMessageBox inputMessageBox)
            {
                if (!_initialized)
                {
                    Application.ResourceAssembly = Assembly.GetExecutingAssembly();

                    // Basically setting it to true would be more than enough, however the application won't copy the 'Microsoft.Xaml.Behaviors' dll to the applications directory.
                    // Nor will it load it, which wouldn't allow for loading with 'Assembly.LoadFrom'.

                    _initialized = typeof(Behavior).IsClass;
                }

                Uri resourceLocator = new Uri("Core/Components/UI/MessageBox.xaml", UriKind.Relative);

                Application.LoadComponent(inputMessageBox, resourceLocator);

            }
        }

        public static bool Show(string title, string content, MessageBoxButtons messageBoxButtons, Icon icon, out MessageBoxButtons button)
        {
            var msgBox = new BaseMessageBox(title, content, messageBoxButtons, icon);

            button = msgBox.Value;

            return !msgBox.Canceled;
        }
    }

    internal class MessageBoxModel : ObservableObject
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value, nameof(Title));
            }
        }

        private string _content;

        public string Content
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value, nameof(Content));
            }
        }

        private ObservableCollection<MessageBoxButtonData> _buttonsSource;

        public ObservableCollection<MessageBoxButtonData> ButtonsSource
        {
            get => _buttonsSource;
            set
            {
                SetProperty(ref _buttonsSource, value, nameof(Content));
            }
        }

        public ICommand DragCommand
        {
            get;
        }

        public ICommand CancelCommand
        {
            get;
        }

        public BitmapSource Icon
        {
            get;
        }

        internal event Action<MessageBoxButtons> OnResult;
        internal event Action OnCancel;
        internal event Action OnDrag;

        internal MessageBoxModel(MessageBoxButtons messageBoxButtons, Icon icon)
        {
            if (GetHammingWeight((uint)messageBoxButtons) > 4u)
            {
                throw new ArgumentException("More than four buttons are not supported.", nameof(messageBoxButtons));
            }

            DragCommand = new DelegateCommand(OnDragCommand);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);

            var collection = new ObservableCollection<MessageBoxButtonData>();

            foreach (var buttonName in messageBoxButtons.GetFlagRepresentation())
            {
                collection.Add(new MessageBoxButtonData(OnResultCommandExecuted) { Name = buttonName, Value = (int)buttonName.GetEnumFromRepresentation<MessageBoxButtons>() });
            }

            ButtonsSource = collection;

            Icon = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void OnResultCommandExecuted(object parameter)
        {
            var button = (MessageBoxButtons)(int)parameter;

            OnResult?.Invoke(button);
        }

        private void OnCancelCommandExecuted(object parameter)
        {
            OnCancel?.Invoke();
        }

        private void OnDragCommand(object parameter)
        {
            OnDrag?.Invoke();
        }

        private uint GetHammingWeight(uint value)
        {
            value -= ((value >> 1) & 0x55555555);
            value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
            return (((value + (value >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }

    internal class MessageBoxButtonData : DelegateCommand
    {
        public string Name { get; set; }

        public int Value { get; set; }

        internal MessageBoxButtonData(Action<object> callback) : base(callback)
        {

        }
    }
}
