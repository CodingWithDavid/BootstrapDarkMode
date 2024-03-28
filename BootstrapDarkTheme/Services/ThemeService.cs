using Microsoft.JSInterop;

namespace BootstrapDarkTheme
{
    public class ThemeService
    {
        private readonly IJSRuntime? js;
        private bool isDarkTheme = false;

        public bool IsDarkTheme 
        { 
            get => isDarkTheme;
            set {
                isDarkTheme = value;

                // Set the theme in the browser
                js.InvokeVoidAsync("setDarkTheme", value);

                // Notify the app that the theme has changed
                OnChange?.Invoke();
            }
        }

        public ThemeService(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task<bool> IsThemeDark() 
        {
            // Check if the browser is in dark mode
            var result = await js.InvokeAsync<bool>("isBrowerDarkTheme");
            return result;
        }

        public event Action? OnChange;

        public async Task ListenForThemeChanged()
        {
            // Listen for theme changes in the browser
            var ThemeObject = DotNetObjectReference.Create(this);
            await js.InvokeVoidAsync("addListerForThemeChanged", ThemeObject);
        }

        [JSInvokable]
        public async Task SetDarkTheme(bool isDark) => IsDarkTheme = isDark;
    }
}
