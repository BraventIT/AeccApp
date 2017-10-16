namespace AeccApp.Core.Messages
{
    public class TabMessage
    {
        public int TabIndex { get; set; }

        public TabMessage(int tabIndex)
        {
            TabIndex = tabIndex;
        }
    }
}
