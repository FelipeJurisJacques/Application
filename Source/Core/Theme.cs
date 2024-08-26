namespace Application.Source.Core
{
    public class Theme(string name, ThemeType type, string primaryColor, string secondaryColor)
    {
        private string _name = name;
        private ThemeType _type = type;
        private string _primaryColor = primaryColor;
        private string _secondaryColor = secondaryColor;

        public string Name => _name;

        public ThemeType Type => _type;

        public string PrimaryColor => _primaryColor;

        public string SecondaryColor => _secondaryColor;
    }

    public enum ThemeType
    {
        DARK,
        LIGHT,
        HIGH_CONTRAST,
    }
}
