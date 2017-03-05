using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Wojtek
{
    class MyAdapter : BaseAdapter<Records>
    {
        public List<Records> mResult;
        private Context mContext;

        public MyAdapter(Context context, List<Records> result)
        {
            mResult = result;
            mContext = context;
        }
        public override Records this[int position]
        {
            get
            {
                return mResult[position];
            }
        }

        public override int Count
        {
            get
            {
                return mResult.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.tab, null, false);
            }
           
            if (MainActivity.Color_theme==2) { row.SetBackgroundColor(Android.Graphics.Color.White); }
            else { row.SetBackgroundColor(Android.Graphics.Color.DarkGray); }

            TextView txtText = row.FindViewById<TextView>(Resource.Id.txtText);
            txtText.Text = mResult[position].value+" °C";
            TextView txtText2 = row.FindViewById<TextView>(Resource.Id.txtText2);
            txtText2.Text = "Data: "+mResult[position].actualdate;
            TextView txtText1 = row.FindViewById<TextView>(Resource.Id.txtText1);
            txtText1.Text = "Godzina: "+mResult[position].string_actualtime;
            if (mResult[position].value >= MainActivity.Alert) { row.SetBackgroundColor(Android.Graphics.Color.Red); }
            if (mResult[position].value >= MainActivity.Alert-(MainActivity.Alert*0.1) && (mResult[position].value < MainActivity.Alert)) { row.SetBackgroundColor(Android.Graphics.Color.Orange); }


            return row;
        }
    }
}