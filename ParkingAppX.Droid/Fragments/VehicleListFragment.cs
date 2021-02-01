using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Droid.Adapters;
using ParkingAppX.Droid.Utils.Views;
using ParkingAppX.Droid.ViewModels;
using Google.Android.Material.Dialog;
using ParkingAppX.Droid.ViewModels.Response;

namespace ParkingAppX.Droid.fragments
{
    public class VehicleListFragment : Fragment, VehicleListRefresh
    {

        #region Views
        private FloatingActionButton floatingActionButton;
        private VehicleAdapter mAdapter;
        private RecyclerView mRecyclerView;
        private AppCompatTextView ListEmpty;
        private FrameLayout Loader;
        #endregion

        private ReceiptViewModel ViewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_vehicle_list, container, false);
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            ReferencesViews(view);
            InitViewModel();
            Clicks();
            RefreshList();
        }

        private void ReferencesViews(View view)
        {
            floatingActionButton = (FloatingActionButton)view.FindViewById(Resource.Id.fab_add_vehicle);
            mRecyclerView = (RecyclerView)view.FindViewById(Resource.Id.vehicle_list);
            ListEmpty = (AppCompatTextView)view.FindViewById(Resource.Id.list_empty);
            Loader = (FrameLayout)view.FindViewById(Resource.Id.loader_vehicle);
        }

        private void InitViewModel()
        {
            ViewModel = new ViewModelProvider(this).Get(Java.Lang.Class.FromType(typeof(ReceiptViewModel))) as ReceiptViewModel;
            ViewModel.Context = RequireContext();
        }

        public void RefreshList()
        {
            Loader.IsVisible(true);
            ResourceData<List<Receipt>> result = ViewModel.GetVehiclesAsync();
            switch (result._status)
            {
                case (int)StatusData.SUCCESS:
                    Loader.IsVisible(false);
                    if (result._data != null)
                    {
                        mAdapter = new VehicleAdapter(this, result._data);
                        mRecyclerView.SetAdapter(mAdapter);
                        ListEmpty.IsVisible(false);
                    }
                    else
                    {
                        ListEmpty.IsVisible(true);
                    }
                    break;
                case (int)StatusData.ERROR:
                    Loader.IsVisible(false);
                    new MaterialAlertDialogBuilder(RequireContext())
                        .SetTitle(GetString(Resource.String.something_unexpected_happened))
                        .SetMessage(result._message)
                        .Show();
                    break;
            };

        }

        private void Clicks()
        {
            floatingActionButton.Click += (_, e) => new AddVehicleDialogFragment(this).Show(ChildFragmentManager, "Add Vehicle");
        }

    }
}
