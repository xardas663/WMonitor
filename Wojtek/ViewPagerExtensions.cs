using Android.App;
using Android.Support.V4.View;

namespace Wojtek
{

    public static class ViewPagerExtensions
    {
        public static ActionBar.Tab GetViewPageTab(this ViewPager viewPager, ActionBar actionBar, string name)
        {
            var tab = actionBar.NewTab();
            tab.SetText(name);
            tab.TabSelected += (o, e) => {
                viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, false);
            };
            return tab;
        }
    }
}