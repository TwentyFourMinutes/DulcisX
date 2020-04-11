using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Xaml.Behaviors;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace DulcisX.Core
{
    public static class InputMessageBox
    {
        private class BaseInputMessageBox : DialogWindow
        {
            internal bool Canceled { get; private set; } = true;
            internal string Value { get; private set; }

            private readonly InputMessageBoxModel _messageContext;

            private static Predicate<string> _defaultValidator = value => !string.IsNullOrWhiteSpace(value);

            private static bool _initialized;

            internal BaseInputMessageBox(string title, string content, string value, int maxLength = 256, Predicate<string> validator = null)
            {
                if (maxLength < 1)
                {
                    throw new ArgumentException("You can not have a lenght smaller than 1.", nameof(maxLength));
                }

                _messageContext = new InputMessageBoxModel(validator ?? _defaultValidator)
                {
                    Title = title,
                    Content = content,
                    Value = value,
                    MaxLength = maxLength
                };

                _messageContext.OnClose += OnBaseClose;
                _messageContext.OnDrag += OnBaseDrag;

                this.DataContext = _messageContext;

                Load(this);

                this.ShowModal();
            }

            private void OnBaseClose(string value, bool canceled)
            {
                Value = value;
                Canceled = canceled;
                _messageContext.OnClose -= OnBaseClose;
                _messageContext.OnDrag += OnBaseDrag;
                this.Close();
            }

            private void OnBaseDrag()
            {
                this.DragMove();
            }

            private static void Load(BaseInputMessageBox inputMessageBox)
            {
                if (!_initialized)
                {
                    Application.ResourceAssembly = Assembly.GetExecutingAssembly();

                    // Basically setting it to true would be more than enough, however the application won't copy the 'Microsoft.Xaml.Behaviors' dll to the applications directory.
                    // Nor will it load it, which wouldn't allow for loading with 'Assembly.LoadFrom'.

                    _initialized = typeof(Behavior).IsClass;
                }

                Uri resourceLocator = new Uri("Core/Components/UI/InputMessageBox.xaml", UriKind.Relative);

                Application.LoadComponent(inputMessageBox, resourceLocator);

            }
        }

        public static bool Show(string title, string content, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, string.Empty);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, int maxLength, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, string.Empty, maxLength);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, string.Empty, validator: validator);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, int maxLength, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, string.Empty, maxLength, validator);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, string defaultValue, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, defaultValue);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, string defaultValue, int maxLength, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, defaultValue, maxLength);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, string defaultValue, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, defaultValue, validator: validator);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }

        public static bool Show(string title, string content, string defaultValue, int maxLength, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, defaultValue, maxLength, validator);

            value = msgBox.Value;

            return !msgBox.Canceled;
        }
    }

    internal class InputMessageBoxModel : ObservableObject
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

        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value, nameof(Value));
            }
        }

        private int _maxLength;

        public int MaxLength
        {
            get => _maxLength;
            set
            {
                SetProperty(ref _maxLength, value, nameof(MaxLength));
            }
        }

        public ICommand SubmitCommand
        {
            get;
        }

        public ICommand CancelCommand
        {
            get;
        }

        public ICommand DragCommand
        {
            get;
        }

        internal event Action<string, bool> OnClose;
        internal event Action OnDrag;

        private readonly Predicate<string> _validator;

        internal InputMessageBoxModel(Predicate<string> validator)
        {
            _validator = validator;

            SubmitCommand = new DelegateCommand(OnSubmitCommandExecuted, CanExecuteSubmitCommand);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted);
            DragCommand = new DelegateCommand(OnDragCommand);
        }

        private void OnSubmitCommandExecuted(object parameter)
        {
            if (_validator == null || _validator(Value))
            {
                OnClose?.Invoke(Value, false);
            }
        }

        private void OnCancelCommandExecuted(object parameter)
        {
            OnClose?.Invoke(Value, true);
        }

        private bool CanExecuteSubmitCommand(object parameter)
        {
            return _validator(Value);
        }

        private void OnDragCommand(object parameter)
        {
            OnDrag?.Invoke();
        }
    }
}
