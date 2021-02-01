

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
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Droid.ViewModels;
using static Android.App.DatePickerDialog;
using ParkingAppX.Domain.Utils;
using ParkingAppX.Droid.Utils.Views;
using ParkingAppX.Droid.ViewModels.Response;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Enums;

namespace ParkingAppX.Droid.fragments
{
    public class OutVehicleDialogFragment : AndroidX.Fragment.App.DialogFragment, IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {
        #region Views
        private AppCompatTextView EtDateOut;
        private AppCompatTextView EtTimeOut;
        private AppCompatButton OutVehicle;
        public AppCompatImageView Image;
        public AppCompatTextView Plate;
        public AppCompatTextView Date;
        private FrameLayout Loader;
        #endregion

        private ReceiptViewModel ViewModel;
        private VehicleListRefresh refresh;
        private readonly Receipt receipt;

        public OutVehicleDialogFragment(Receipt receipt, VehicleListRefresh refresh)
        {
            this.receipt = receipt;
            this.refresh = refresh;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.dialog_fragment_out_vehicle, container, false);
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
            Loader.IsVisible(true);
            ChargeDataVehicle();
            InitViewModel();
            ClicksOut();
            ClickProcessing();
        }

        private void InitViewModel()
        {
            ViewModel = new ViewModelProvider(this).Get(Java.Lang.Class.FromType(typeof(ReceiptViewModel))) as ReceiptViewModel;
            ViewModel.Context = RequireContext();
        }

        private void ChargeDataVehicle()
        {
            Plate.Text = receipt.Vehicle.Plate;
            Date.Text = receipt.GetEntryDateString();
            ChangeImageVehicle(Image, receipt.Vehicle);
            Loader.IsVisible(false);
        }

        private void ChangeImageVehicle(AppCompatImageView i, Vehicle vehicle)
        {
            int img;
            if (typeof(Car).IsInstanceOfType(vehicle))
            {
                img = Resource.Drawable.ic_car;
            }
            else if (typeof(Motorcycle).IsInstanceOfType(vehicle))
            {
                img = ((vehicle as Motorcycle).CylinderCapacity > (int)Parking.MAX_CYLINDER_MOTORCYCLE)
                    ? Resource.Drawable.ic_moto500cc : Resource.Drawable.ic_scooter;
            }
            else
            {
                img = Resource.Drawable.ic_bicycle;
            }

            i.SetImageDrawable(i.Context.GetDrawable(img));
        }

        private void ReferencesViews(View view)
        {
            EtDateOut = view.FindViewById<AppCompatTextView>(Resource.Id.et_date_out);
            EtTimeOut = view.FindViewById<AppCompatTextView>(Resource.Id.et_time_out);
            OutVehicle = view.FindViewById<AppCompatButton>(Resource.Id.mb_out_vehicle);
            Image = view.FindViewById<AppCompatImageView>(Resource.Id.receipt_image);
            Plate = view.FindViewById<AppCompatTextView>(Resource.Id.receipt_item_plate);
            Date = view.FindViewById<AppCompatTextView>(Resource.Id.receipt_item_date);
            Loader = view.FindViewById<FrameLayout>(Resource.Id.loader_out_vehicle);
        }

        private void ClicksOut()
        {
            EtDateOut.Click += (_, e) =>
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(RequireContext(),
                                                               this,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day);
                dialog.Show();
            };

            EtTimeOut.Click += (_, e) =>
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
            EtDateOut.Text = GetString(Resource.String.date_to, date);
            EtDateOut.Tag = date;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            var time = hourOfDay.CompleteFormat() + ":" + minute.CompleteFormat();
            EtTimeOut.Text = GetString(Resource.String.time_to, time);
            EtTimeOut.Tag = time;
        }

        private void ClickProcessing()
        {
            OutVehicle.Click += (_, e) =>
            {
                KeyValuePair<bool, long> resultValid = ValidateData();

                if (resultValid.Key)
                {
                    SaveData(resultValid.Value, receipt);
                }
                else
                {
                    new MaterialAlertDialogBuilder(RequireContext())
                        .SetTitle(GetString(Resource.String.error))
                        .SetMessage(GetString(Resource.String.error_date))
                        .Show();
                }

            };
        }

        private KeyValuePair<bool, long> ValidateData()
        {
            bool isValid;
            long date;

            date = (((string)EtDateOut.Tag) + " " + ((string)EtTimeOut.Tag)).GetLongDate();

            isValid = date > 0 &&
                date > receipt.EntryDate;

            return new KeyValuePair<bool, long>(isValid, date);
        }

        private void SaveData(long date, Receipt receipt)
        {
            Loader.IsVisible(true);
            ResourceData<double> result = ViewModel.TakeOutVehicle(date, receipt);
            switch (result._status)
            {
                case (int)StatusData.SUCCESS:
                    Loader.IsVisible(false);
                    new MaterialAlertDialogBuilder(RequireContext())
                        .SetTitle(GetString(Resource.String.info))
                        .SetMessage(GetString(Resource.String.amount_to_pay, result._data.ToString()))
                        .SetPositiveButton(GetString(Resource.String.confirm), (_, args) =>
                        {
                            refresh.RefreshList();
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
