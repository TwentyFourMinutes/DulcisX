using Microsoft.VisualStudio.Imaging.Interop;
using System;

namespace DulcisX.Core.Components
{
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
        IMoreContentInfoMessageBuilder WithHyperlink(string text, Uri uri, bool openInternally = false);
        IMoreContentInfoMessageBuilder WithHyperlink(string text, Action callback);
    }

    public interface IButtonInfoMessageBuilder
    {
        IButtonInfoMessageBuilder WithButton(string text, bool closeAfterClick = true);
        IButtonInfoMessageBuilder WithButton(string text, Action callback, bool closeAfterClick = true);
        InfoBarHandle Publish();
    }

    public interface IButtonIdentifierInfoMessageBuilder
    {
        IButtonIdentifierInfoMessageBuilder<TIdentifier> WithButton<TIdentifier>(string text, TIdentifier identifier);
    }

    public interface IButtonIdentifierInfoMessageBuilder<TIdentifier>
    {
        IButtonIdentifierInfoMessageBuilder<TIdentifier> WithButton(string text, TIdentifier identifier);
        ResultInfoBarHandle<TIdentifier> Publish();
    }

    public interface IMoreContentInfoMessageBuilder : IContentInfoMessageBuilder, IButtonInfoMessageBuilder, IButtonIdentifierInfoMessageBuilder
    {

    }
}
