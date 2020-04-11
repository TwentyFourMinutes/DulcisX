using Microsoft.VisualStudio.Imaging.Interop;
using System;

namespace DulcisX.Core
{
    /// <summary>
    /// Represents the base image builder of an InfoBar message.
    /// </summary>
    public interface IBaseInfoMessageBuilder
    {
        /// <summary>
        /// Adds an Info image to the InfoBar message.
        /// </summary>
        /// <returns>An <see cref="IContentInfoMessageBuilder"/> instance.</returns>
        IContentInfoMessageBuilder WithInfoImage();
        /// <summary>
        /// Adds a Warning image to the InfoBar message.
        /// </summary>
        /// <returns>An <see cref="IContentInfoMessageBuilder"/> instance.</returns>
        IContentInfoMessageBuilder WithWarningImage();
        /// <summary>
        /// Adds an Error image to the InfoBar message.
        /// </summary>
        /// <returns>An <see cref="IContentInfoMessageBuilder"/> instance.</returns>
        IContentInfoMessageBuilder WithErrorImage();
        /// <summary>
        /// Adds a custom image to the InfoBar message.
        /// </summary>
        /// <param name="image">The custom image to be used.</param>
        /// <returns>An <see cref="IContentInfoMessageBuilder"/> instance.</returns>
        IContentInfoMessageBuilder WithImage(ImageMoniker image);
    }

    /// <summary>
    /// Represents the message builder of an InfoBar message.
    /// </summary>
    public interface IContentInfoMessageBuilder
    {
        /// <summary>
        /// Adds a text span to the InfoBar message.
        /// </summary>
        /// <param name="text">The text of the span.</param>
        /// <param name="bold">Specifies whether or not the span should be bolded.</param>
        /// <param name="italic">Specifies whether or not the span should be italicized.</param>
        /// <param name="underline">Specifies whether or not the span should be underlined.</param>
        /// <returns>An <see cref="IMoreContentInfoMessageBuilder"/> instance.</returns>
        IMoreContentInfoMessageBuilder WithText(string text, bool bold = false, bool italic = false, bool underline = false);
        /// <summary>
        /// Adds a hyperlink span to the InfoBar message.
        /// </summary>
        /// <param name="text">The text of the hyperlink.</param>
        /// <param name="uri">The uri to which the hyperlink points.</param>
        /// <param name="openInternally">Specifies whether or not the link should be opened in a Visual Studio Browser window.</param>
        /// <returns>An <see cref="IMoreContentInfoMessageBuilder"/> instance.</returns>
        IMoreContentInfoMessageBuilder WithHyperlink(string text, Uri uri, bool openInternally = false);
        /// <summary>
        /// Adds a hyperlink span to the InfoBar message.
        /// </summary>
        /// <param name="text">The text of the hyperlink.</param>
        /// <param name="callback">A callback action which gets called if the user clicks on the hyperlink.</param>
        /// <returns>An <see cref="IMoreContentInfoMessageBuilder"/> instance.</returns>
        IMoreContentInfoMessageBuilder WithHyperlink(string text, Action callback);
    }

    /// <summary>
    /// Represents the button builder of an InfoBar message.
    /// </summary>
    public interface IButtonInfoMessageBuilder
    {
        /// <summary>
        /// Adds a button to the InfoBar message.
        /// </summary>
        /// <param name="text">The text to be displayed inside the button.</param>
        /// <param name="closeAfterClick">Specifies whether or not the InfoBar message should be removed after the user clicked the button.</param>
        /// <returns>An <see cref="IButtonInfoMessageBuilder"/> instance.</returns>
        IButtonInfoMessageBuilder WithButton(string text, bool closeAfterClick = true);
        /// <summary>
        /// Adds a button to the InfoBar message.
        /// </summary>
        /// <param name="text">The text to be displayed inside the button.</param>
        /// <param name="callback">A callback action which gets called if the user clicks on the button.</param>
        /// <param name="closeAfterClick">Specifies whether or not the InfoBar message should be removed after the user clicked the button.</param>
        /// <returns>An <see cref="IButtonInfoMessageBuilder"/> instance.</returns>
        IButtonInfoMessageBuilder WithButton(string text, Action callback, bool closeAfterClick = true);

        /// <summary>
        /// Displays the InfoBar message to the user.
        /// </summary>
        /// <param name="cancelCallback">A callback action which gets called if the user clicks on the close ('x') button.</param>
        /// <returns>A new unique <see cref="InfoBarHandle"/> instance pointing to the created InfoBar message. Used to manually remove the InfoBar message in <see cref="InfoBar.RemoveMessage(InfoBarHandle)"/>.</returns>
        InfoBarHandle Publish(Action cancelCallback = null);
    }

    /// <summary>
    /// Represents the identifier button builder of an InfoBar message.
    /// </summary>
    public interface IButtonIdentifierInfoMessageBuilder
    {
        /// <summary>
        /// Adds a button with an identifier to the InfoBar message.
        /// </summary>
        /// <typeparam name="TIdentifier">The type of the button identifier.</typeparam>
        /// <param name="text">The text to be displayed inside the button.</param>
        /// <param name="identifier">The value which uniquely identifies the button in a InfoBar message.</param>
        /// <returns>An <see cref="IButtonIdentifierInfoMessageBuilder{TIdentifier}"/> instance.</returns>
        IButtonIdentifierInfoMessageBuilder<TIdentifier> WithButton<TIdentifier>(string text, TIdentifier identifier);
    }

    /// <summary>
    /// Represents the identifier button builder of an InfoBar message.
    /// </summary>
    /// <typeparam name="TIdentifier">he type of the button identifiers.</typeparam>
    public interface IButtonIdentifierInfoMessageBuilder<TIdentifier>
    {
        /// <summary>
        /// Adds a button with an identifier to the InfoBar message.
        /// </summary>
        /// <param name="text">The text to be displayed inside the button.</param>
        /// <param name="identifier">The value which uniquely identifies the button in a InfoBar message.</param>
        /// <returns>An <see cref="IButtonIdentifierInfoMessageBuilder{TIdentifier}"/> instance.</returns>
        IButtonIdentifierInfoMessageBuilder<TIdentifier> WithButton(string text, TIdentifier identifier);

        /// <summary>
        /// Displays the InfoBar message to the user.
        /// </summary>
        /// <returns>A new unique <see cref="ResultInfoBarHandle{TIdentifier}"/> instance pointing to the created InfoBar message. Used to manually remove the InfoBar message in <see cref="InfoBar.RemoveMessage(InfoBarHandle)"/>.</returns>
        ResultInfoBarHandle<TIdentifier> Publish();
    }

    /// <summary>
    /// Represents the optional content message builder of an InfoBar message.
    /// </summary>
    public interface IMoreContentInfoMessageBuilder : IContentInfoMessageBuilder, IButtonInfoMessageBuilder, IButtonIdentifierInfoMessageBuilder
    {

    }
}
