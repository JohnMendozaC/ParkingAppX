
using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Lifecycle;
using Google.Android.Material.Dialog;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Utils;
using ParkingAppX.Droid.ViewModels;
using static Android.App.DatePickerDialog;
using ParkingAppX.Droid.fragments;
using ParkingAppX.Droid.Utils.Views;
using ParkingAppX.Droid.ViewModels.Response;

namespace ParkingAppX.Droid
{

    public class AddVehicleDialogFragment : AndroidX.Fragment.App.DialogFragment, IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {

        #region Views
        private AppCompatEditText EtPlate;
        private AppCompatEditText EtCylinderCapacity;
        private AppCompatTextView EtDateAdd;
        private AppCompatTextView EtTimeAdd;
        private AppCompatButton AddVehicle;
        private RadioButton RbCar;
        private FrameLayout Loader;
        #endregion

        private ReceiptViewModel ViewModel;
        private readonly VehicleListRefresh listRefresh;

        public AddVehicleDialogFragment(VehicleListRefresh listRefresh)
        {
            this.listRefresh = listRefresh;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.dialog_fragment_add_vehicle, container, false);
        }

        public override void OnStart()
        {
            base.OnStart();
            RequireDialog().Window.SetLayout(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent
                );
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            ReferencesViews(view);
            InitViewModel();
            ClicksEntry();
            ClickProcessing();
        }

        private void InitViewModel()
        {
            ViewModel = new ViewModelProvider(this).Get(Java.Lang.Class.FromType(typeof(ReceiptViewModel))) as ReceiptViewModel;
            ViewModel.Context = RequireContext();
        }

        private void ReferencesViews(View view)
        {
            EtPlate = (AppCompatEditText)view.FindViewById(Resource.Id.et_plate);
            EtCylinderCapacity = (AppCompatEditText)view.FindViewById(Resource.Id.et_cylinder_capacity);
            EtDateAdd = (AppCompatTextView)view.FindViewById(Resource.Id.et_date_add);
            EtTimeAdd = (AppCompatTextView)view.FindViewById(Resource.Id.et_time_add);
            AddVehicle = (AppCompatButton)view.FindViewById(Resource.Id.mb_add_vehicle);
            RbCar = (RadioButton)view.FindViewById(Resource.Id.rb_car);
            Loader = (FrameLayout)view.FindViewById(Resource.Id.loader_add_vehicle);
        }

        private void ClicksEntry()
        {
            EtDateAdd.Click += (_, e) =>
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(RequireContext(),
                                                               this,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day);
                dialog.Show();
            };

            EtTimeAdd.Click += (_, e) =>
            {
                DateTime currentTime = DateTime.Now;
                bool is24HourFormat = DateFormat.Is24HourFormat(RequireContext());
                TimePickerDialog dialog = new TimePickerDialog
                    (RequireContext(), this, currentTime.Hour, currentTime.Minute, is24HourFormat);

                dialog.Show();
            };

        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            var date = dayOfMonth.CompleteFormat() + "/" + (month + 1).CompleteFormat() + "/" + year;
            EtDateAdd.Text = GetString(Resource.String.date_to, date);
            EtDateAdd.Tag = date;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            var time = hourOfDay.CompleteFormat() + ":" + minute.CompleteFormat();
            EtTimeAdd.Text = GetString(Resource.String.time_to, time);
            EtTimeAdd.Tag = time;
        }

        private void ClickProcessing()
        {
            AddVehicle.Click += (_, e) =>
            {
                KeyValuePair<bool, long> resultValid = ValidateData();

                if (resultValid.Key)
                {
                    SaveData(resultValid.Value,GetReceipt());
                }
                else
                {
                    new MaterialAlertDialogBuilder(RequireContext())
                        .SetTitle(GetString(Resource.String.error))
                        .SetMessage(GetString(Resource.String.error_data))
                        .Show();
                }

            };
        }

        private Vehicle GetReceipt()
        {
            if (RbCar.Checked)
            {
                return new Car(EtPlate.Text.ToString());
            }
            else
            {
                int cylinderCapacity = int.Parse(EtCylinderCapacity.Text.ToString());
                return new Motorcycle(
                    EtPlate.Text.ToString(),
                    cylinderCapacity);
            }
        }

        private KeyValuePair<bool, long> ValidateData()
        {
            bool isValid;
            long date;

            date = (((string)EtDateAdd.Tag) + " " + ((string)EtTimeAdd.Tag)).GetLongDate();

            isValid = date > 0 &&
                EtPlate.Text.ToString() != null &&
                EtCylinderCapacity.Text.ToString() != null;

            return new KeyValuePair<bool, long>(isValid, date);
        }

        private void SaveData(long date, Vehicle vehicle)
        {
            Loader.IsVisible(true);
            ResourceData<string> result = ViewModel.EnterVehicle(date, vehicle);
            switch (result._status)
            {
                case (int)StatusData.SUCCESS:
                    Loader.IsVisible(false);
                    new MaterialAlertDialogBuilder(RequireContext())
                        .SetTitle(GetString(Resource.String.info))
                        .SetMessage(result._message)
                        .SetPositiveButton(GetString(Resource.String.confirm), (_, args) => {
                            listRefresh.RefreshList();
                            Dismiss();
                        })
                        .Show();
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
    }
}
