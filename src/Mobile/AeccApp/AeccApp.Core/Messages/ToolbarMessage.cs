namespace AeccApp.Core.Messages
{
    public class ToolbarMessage
    {
        public bool ShowLogo { get; set; }
        public string Title { get; set; }

        public ToolbarMessage(bool showLogo, string navigationTitle= "")
        {
            ShowLogo = showLogo;
            Title = navigationTitle;
        }

        public ToolbarMessage(string navigationTitle): this(false, navigationTitle)
        {
        }
    }
}
