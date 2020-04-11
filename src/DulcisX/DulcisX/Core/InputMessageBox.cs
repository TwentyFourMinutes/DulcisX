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
            private readonly InputMessageBoxModel _messageContext;

            private static Predicate<string> _defaultValidator = value => !string.IsNullOrWhiteSpace(value);

            internal bool _canceled = true;
            internal string _value;

            private static bool _initialized;

            internal BaseInputMessageBox(string title, string content, string value, Predicate<string> validator = null)
            {
                _messageContext = new InputMessageBoxModel(validator ?? _defaultValidator)
                {
                    Title = title,
                    Content = content,
                    Value = value
                };

                _messageContext.OnClose += OnBaseClose;
                _messageContext.OnDrag += OnBaseDrag;

                this.DataContext = _messageContext;

                Load(this);

                this.ShowModal();
            }

            private void OnBaseClose(string value, bool canceled)
            {
                _value = value;
                _canceled = canceled;
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

                Uri resourceLocator = new Uri("UI/InputMessageBox.xaml", UriKind.Relative);

                Application.LoadComponent(inputMessageBox, resourceLocator);

            }
        }

        public static bool Show(string title, string content, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, string.Empty);

            value = msgBox._value;

            return msgBox._canceled;
        }

        public static bool Show(string title, string content, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, string.Empty, validator);

            value = msgBox._value;

            return msgBox._canceled;
        }

        public static bool Show(string title, string content, string defaultValue, out string value)
        {
            var msgBox = new BaseInputMessageBox(title, content, defaultValue);

            value = msgBox._value;

            return msgBox._canceled;
        }

        public static bool Show(string title, string content, string defaultValue, Predicate<string> validator, out string value)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));

            var msgBox = new BaseInputMessageBox(title, content, defaultValue, validator);

            value = msgBox._value;

            return msgBox._canceled;
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
