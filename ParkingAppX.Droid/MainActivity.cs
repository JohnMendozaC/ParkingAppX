using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using ParkingAppX.Droid.fragments;

namespace ParkingAppX.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SupportFragmentManager.BeginTransaction()
            .Add(Resource.Id.fragmentContainerView, new VehicleListFragment())
            .Commit();

        }
    }
}