this.InfoBar.NewMessage()
            .WithInfoImage()
            .WithText("The ")
            .WithText(node.GetFullName(), underline: true)
            .WithText(" file got saved.")
            .WithButton("Okay")
            .Publish();