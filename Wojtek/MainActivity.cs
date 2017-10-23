using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using System.ComponentModel;
using Android.Content.PM;
using OxyPlot.Xamarin.Android;
using System.Globalization;


namespace Wojtek
{
    [Activity(Label = "WiMonitor", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon")]
    public class ResultsRepository
    {
        public List<Records> Results = new List<Records>();
        public List<Records> Average = new List<Records>();
        public List<Records> Results_chart = new List<Records>();
    }


    public class MainActivity : FragmentActivity
    {       
         
        IFormatProvider culture = new CultureInfo("en-US", true);           
        private static DateTime today = DateTime.Now;
        private static string today_string;       
        public static int alert;
        private static int color_theme;
        private static int amount;

        SwipeRefreshLayout mSwipeRefreshLayout;
        ListViewAdapter adapter_results;
        ListViewAdapter adapter_avg;

        #region set&get methods
       

        protected static DateTime Today
        {
            get => today;

            set
            {
                today = value;
            }
        }

        protected static string Today_string
        {
            get
            {
                return today_string;
            }

            set
            {
                today_string = value;
            }
        }

        public static int Alert
        {
            get
            {
                return alert;
            }

            set
            {
                alert = value;
            }
        }

        public static int Color_theme
        {
            get
            {
                return color_theme;
            }

            set
            {
                color_theme = value;
            }
        }

        protected static int Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        #endregion


        protected override void OnCreate(Bundle bundle)
        {
            Today_string = Today.ToString("yyyy-MM-dd");
            MResults.Clear();
            MResults_chart.Clear();
            MAverage.Clear();

            SQLconnection.connect(false);
            
            
            if (Color_theme == 1) { SetTheme(Android.Resource.Style.ThemeMaterial); }
            if (Color_theme == 2) { SetTheme(Android.Resource.Style.ThemeHoloLight); }
            if (Color_theme == 3) { SetTheme(Android.Resource.Style.ThemeHolo); }
                     
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);          
                   
                                   
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
           

            adaptor.AddFragmentView((i, v, b) =>
            {
              
                var view = i.Inflate(Resource.Layout.tab, v, false);
                adapter_results = new ListViewAdapter(this, MResults);
                var listView = view.FindViewById<ListView>(Resource.Id.myListView);             
                listView.Adapter = adapter_results;
                refresh(view);
               
                return view;

            });

            adaptor.AddFragmentView((i, v, b) =>
            {
               
                var view = i.Inflate(Resource.Layout.tab, v, false);
                adapter_avg = new ListViewAdapter(this, MAverage);
                var listView = view.FindViewById<ListView>(Resource.Id.myListView);             
                listView.Adapter = adapter_avg;
                refresh(view);                          
                
                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {
                
                var view = i.Inflate(Resource.Layout.chart, v, false);
                PlotView plotview = view.FindViewById<PlotView>(Resource.Id.plot_view);
                PlotBuilder model = new PlotBuilder();
                plotview.Model = model.CreatePlotModel();
                
                return view;
            });

            adaptor.AddFragmentView((i, v, b) =>
            {
                var view2 = i.Inflate(Resource.Layout.settings, v, false);
                Button button = view2.FindViewById<Button>(Resource.Id.save_settings);
                EditText amount= view2.FindViewById<EditText>(Resource.Id.editAmount);
                EditText alert = view2.FindViewById<EditText>(Resource.Id.editAlert);
                button.Click += Button_Click;
                amount.Text = Amount.ToString();
                alert.Text=Alert.ToString();                           
               

                return view2;
            });

            
            pager.Adapter = adaptor;
            
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Wyniki"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Średnia"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Wykres"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "Ustawienia"));
          
           


        }

        private void Button_Click(object sender, EventArgs e)
        {

            EditText amount = FindViewById<EditText>(Resource.Id.editAmount);
            EditText alert = FindViewById<EditText>(Resource.Id.editAlert);
            RadioButton radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            RadioButton radioButton3 = FindViewById<RadioButton>(Resource.Id.radioButton3);
            RadioGroup color_theme_radio_button = FindViewById<RadioGroup>(Resource.Id.color_theme_button);

            if (color_theme_radio_button.CheckedRadioButtonId == Resource.Id.radioButton1) Color_theme = 1;
            if (color_theme_radio_button.CheckedRadioButtonId == Resource.Id.radioButton2) Color_theme = 2;
            if (color_theme_radio_button.CheckedRadioButtonId == Resource.Id.radioButton3) Color_theme = 3;

            Alert = Convert.ToInt16(alert.Text);
            Amount = Convert.ToInt16(amount.Text);

       
            SQLconnection.connect(true);
            Toast.MakeText(this, "Pamiętaj, że niektóre zmiany wymagają ponownego uruchomienia aplikacji", ToastLength.Short).Show();
        }
      

        private void MSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            RunOnUiThread(() =>
            {
                adapter_avg.NotifyDataSetChanged();
                adapter_results.NotifyDataSetChanged();
                Toast.MakeText(this, "Odświeżono", ToastLength.Short).Show();
                mSwipeRefreshLayout.Refreshing = false;

            });
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {            
            SQLconnection.connect(false);
        }     
        
        public void refresh(Android.Views.View view)
        {
            mSwipeRefreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeLayout);
            mSwipeRefreshLayout.SetColorScheme(Android.Resource.Color.HoloBlueBright, Android.Resource.Color.HoloBlueDark, Android.Resource.Color.HoloGreenDark, Android.Resource.Color.HoloRedLight);
            mSwipeRefreshLayout.Refresh += MSwipeRefreshLayout_Refresh;
        }
    }
}

