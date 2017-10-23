using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Support.V4.App;

namespace Wojtek
{
    public class GenericFragmentPagerAdaptor:FragmentStatePagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();
        public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
        }

        public override int GetItemPosition(Java.Lang.Object objectValue)
        {
            return PositionNone;
        }

        public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _fragmentList.Add(new GenericViewPagerFragment(view));
        }

        public void AddFragment(GenericViewPagerFragment fragment)
        {
            _fragmentList.Add(fragment);
        }

        public override int Count
        {
            get 
            {
                return _fragmentList.Count;
            }
        }    

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {          
            return  _fragmentList[position];            
        }
    }
}