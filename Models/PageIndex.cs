namespace JRPC_HMS.Models
{
    public class PageIndex
    {
        public PageIndex(int index, bool isActive)
        {
            Index = index;
            IsActive = isActive;
        }

        public int Index { get; set; }
        public bool IsActive { get; set; }
    }
}
