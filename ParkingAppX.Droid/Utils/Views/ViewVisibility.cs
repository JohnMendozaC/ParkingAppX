using Android.Views;

namespace ParkingAppX.Droid.Utils.Views
{
    public static class ViewVisibility
    {
        public static void IsVisible(this View v, bool visible)
        {
            v.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}
