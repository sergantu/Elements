namespace CodeBase.GameLevel
{
    public class MatchInfo
    {
        public bool IsChecked;
        public bool ShouldDestroy;
        public ElementType ElementType;

        public MatchInfo(bool isChecked, bool destroy, ElementType elementType)
        {
            IsChecked = isChecked;
            ShouldDestroy = destroy;
            ElementType = elementType;
        }
        
        public MatchInfo(bool isChecked, bool destroy)
        {
            IsChecked = isChecked;
            ShouldDestroy = destroy;
        }
    }
}