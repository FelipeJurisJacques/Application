namespace Application.Source.Core
{
    public class Theme(string name, ThemeType type, string primaryColor, string secondaryColor)
    {
        private string _name = name;
        private ThemeType _type = type;
        private string _primaryColor = primaryColor;
        private string _secondaryColor = secondaryColor;

        // public ThemeType Current
        // {
        //     get { return _theme; }
        //     set
        //     {
        //         _theme = value;
        //         switch (value)
        //         {
        //             case ThemeType.LIGHT:
        //                 _js.InvokeVoidAsync("localStorage.setItem", "theme", "light");
        //                 break;
        //             case ThemeType.HIGH_CONTRAST:
        //                 _js.InvokeVoidAsync("localStorage.setItem", "theme", "high_contrast");
        //                 break;
        //             case ThemeType.DARK:
        //             default:
        //                 _js.InvokeVoidAsync("localStorage.setItem", "theme", "dark");
        //                 break;
        //         }
        //     }
        // }
    }

    public enum ThemeType
    {
        DARK,
        LIGHT,
        HIGH_CONTRAST,
    }
}
