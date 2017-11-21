namespace AeccApp.Core.Messages
{
    public class ToolbarMessage
    {
        public bool ShowLogo { get; set; }
        public string _NavigationTitle { get; set; }

        public ToolbarMessage(bool showLogo, string navigationTitle)
        {
            ShowLogo = showLogo;
            _NavigationTitle = navigationTitle;
        }
    }
}
