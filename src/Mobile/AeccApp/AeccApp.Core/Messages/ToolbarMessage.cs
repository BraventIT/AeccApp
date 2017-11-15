namespace AeccApp.Core.Messages
{
    public class ToolbarMessage
    {
        public bool ShowLogo { get; set; }

        public ToolbarMessage(bool showLogo)
        {
            ShowLogo = showLogo;
        }
    }
}
