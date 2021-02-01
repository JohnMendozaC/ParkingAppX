using System.Collections.Generic;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using ParkingAppX.Domain.Aggregate;
using ParkingAppX.Domain.Entity;
using ParkingAppX.Domain.Enums;
using ParkingAppX.Droid.fragments;

namespace ParkingAppX.Droid.Adapters
{
    public class VehicleAdapter : RecyclerView.Adapter
    {
        private VehicleListRefresh Refresh { get; set; }
        List<Receipt> ListReceipt { get; set; }

        public VehicleAdapter(VehicleListRefresh newRefresh, List<Receipt> list)
        {
            Refresh = newRefresh;
            ListReceipt = list;
        }

        public override int ItemCount => ListReceipt.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                    Inflate(Resource.Layout.vehicle_list_item, parent, false);
            VehicleViewHolder vh = new VehicleViewHolder(itemView);
            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var receipt = ListReceipt[position];
            (holder as VehicleViewHolder).Bind(receipt, Refresh);
        }
    }

    public class VehicleViewHolder : RecyclerView.ViewHolder
    {
        public AppCompatImageView Image { get; private set; }
        public AppCompatTextView Plate { get; private set; }
        public AppCompatTextView Date { get; private set; }

        public VehicleViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<AppCompatImageView>(Resource.Id.receipt_image);
            Plate = itemView.FindViewById<AppCompatTextView>(Resource.Id.receipt_item_plate);
            Date = itemView.FindViewById<AppCompatTextView>(Resource.Id.receipt_item_date);
        }

        public void Bind(Receipt receipt, VehicleListRefresh Refresh)
        {
            Plate.Text = receipt.Vehicle.Plate;
            Date.Text = receipt.GetEntryDateString();
            ChangeImageVehicle(Image, receipt.Vehicle);
            ItemView.Click += (_, e) => new OutVehicleDialogFragment(receipt, Refresh).Show(((AppCompatActivity)ItemView.Context).SupportFragmentManager, "Out Vehicle");
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

    }
}
