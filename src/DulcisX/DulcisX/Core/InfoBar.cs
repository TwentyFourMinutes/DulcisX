using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace DulcisX.Core
{
    public class InfoBar
    {
        private readonly IVsInfoBarUIFactory _uiFactory;
        private readonly IVsInfoBarHost _host;

        internal InfoBar(IVsInfoBarUIFactory uiFactory, IVsInfoBarHost host)
        {
            _uiFactory = uiFactory;
            _host = host;
        }

        private class InternalInfoMessageBuilder : IBaseInfoMessageBuilder, IMoreContentInfoMessageBuilder
        {
            private readonly List<IVsInfoBarTextSpan> _textSpans;
            private readonly List<IVsInfoBarActionItem> _actionButtons;
            private ImageMoniker _image;
            private readonly bool _hasCloseButton;

            private readonly IVsInfoBarUIFactory _uiFactory;
            private readonly IVsInfoBarHost _host;

            internal InternalInfoMessageBuilder(bool hasCloseButton, IVsInfoBarUIFactory uiFactory, IVsInfoBarHost host)
            {
                _textSpans = new List<IVsInfoBarTextSpan>();
                _actionButtons = new List<IVsInfoBarActionItem>();
                _hasCloseButton = hasCloseButton;
                _uiFactory = uiFactory;
                _host = host;
            }

            public IContentInfoMessageBuilder WithInfoImage()
                => WithImage(KnownMonikers.StatusInformation);

            public IContentInfoMessageBuilder WithWarningImage()
                => WithImage(KnownMonikers.StatusWarning);

            public IContentInfoMessageBuilder WithErrorImage()
                => WithImage(KnownMonikers.StatusError);

            public IContentInfoMessageBuilder WithImage(ImageMoniker image)
            {
                _image = image;

                return this;
            }
            public IMoreContentInfoMessageBuilder WithText(string text, bool bold = false, bool italic = false, bool underline = false)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException($"{nameof(text)} can not be null or empty.");
                }

                _textSpans.Add(new InfoBarTextSpan(text, bold, italic, underline));

                return this;
            }

            public IMoreContentInfoMessageBuilder WithUrl(string text, string url)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException($"{nameof(text)} can not be null or empty.");
                }

                _textSpans.Add(new InfoBarHyperlink(text));

                return this;
            }

            public IButtonInfoMessageBuilder WithButton(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException($"{nameof(text)} can not be null or empty.");
                }

                _actionButtons.Add(new InfoBarButton(text));

                return this;
            }

            public InfoBarHandle Publish()
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var model = new InfoBarModel(_textSpans, _actionButtons, _image, _hasCloseButton);

                var uiElement = _uiFactory.CreateInfoBar(model);

                _host.AddInfoBar(uiElement);

                return new InfoBarHandle(uiElement);
            }
        }

        public IBaseInfoMessageBuilder NewMessage(bool hasCloseButton = true)
            => new InternalInfoMessageBuilder(hasCloseButton, _uiFactory, _host);

        public void RemoveMessage(InfoBarHandle handle)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _host.RemoveInfoBar(handle.UIElement);
        }
    }

    public interface IBaseInfoMessageBuilder
    {
        IContentInfoMessageBuilder WithInfoImage();
        IContentInfoMessageBuilder WithWarningImage();
        IContentInfoMessageBuilder WithErrorImage();
        IContentInfoMessageBuilder WithImage(ImageMoniker image);
    }

    public interface IContentInfoMessageBuilder
    {
        IMoreContentInfoMessageBuilder WithText(string text, bool bold = false, bool italic = false, bool underline = false);
        IMoreContentInfoMessageBuilder WithUrl(string text, string url);
    }

    public interface IButtonInfoMessageBuilder
    {
        IButtonInfoMessageBuilder WithButton(string text);

        InfoBarHandle Publish();
    }

    public interface IMoreContentInfoMessageBuilder : IContentInfoMessageBuilder, IButtonInfoMessageBuilder
    {

    }

    public class InfoBarHandle
    {
        internal readonly IVsUIElement UIElement;

        internal InfoBarHandle(IVsUIElement uiElement)
        {
            UIElement = uiElement;
        }
    }
}
