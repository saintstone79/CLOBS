using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Globalization;
using CLOBS2.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CLOBS2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ObservInfoPage : Page
    {
        private bool bStarted = false;
        private ObservationMode _objMode;
        public ObservInfoPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            StomperNo.SelectedIndex = 1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ObservationMode && e.Parameter != null)
            {
                _objMode = (ObservationMode) e.Parameter;
            }
            base.OnNavigatedTo(e);
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            if (ObserverName.Text.Trim().Length < 2)
            {
                ErrorMessage.Text = "Please enter observer name";
                return;
            }

            if (SchoolName.Text.Trim().Length < 2)
            {
                ErrorMessage.Text = "Please enter school name";
                return;
            }

            if (TeacherName.Text.Trim().Length < 2)
            {
                ErrorMessage.Text = "Please enter teacher name";
                return;
            }

            ObservationInfoData objInfoData = new ObservationInfoData();

            objInfoData.ObjMode = _objMode;
            objInfoData.ObservationDuration = TimeSpan.ParseExact(ObservationTime.SelectedItem.ToString(), (@"m\:ss"), CultureInfo.CurrentCulture);
            objInfoData.GapDuration = TimeSpan.ParseExact(GapTime.SelectedItem.ToString(), (@"m\:ss"), CultureInfo.CurrentCulture);
            TimeSpan tsTimeEpasped;
            if (TimeSpan.TryParse(VideoSync.Text, out tsTimeEpasped) == false)
            {
                ErrorMessage.Text = "Please enter valid time format in VIDEO SYNC (eg. 00:05:12)";
                return;
            }
            objInfoData.VideoElasped = tsTimeEpasped;

            int nStomper = StomperNo.SelectedIndex + 1; // stomper index start 1
            objInfoData.StomperNumber = nStomper;
            if (nStomper == 1 && STOMPER1.Text.Trim().Length < 2)
            {
                ErrorMessage.Text = "Enter STOMPER1 name";
                return;
            }
            else if (nStomper == 2 && (STOMPER1.Text.Trim().Length < 2 || STOMPER2.Text.Trim().Length < 2))
            {
                ErrorMessage.Text = "Enter all STOMPER names";
                return;
            }
            else if (nStomper == 3 && (STOMPER1.Text.Trim().Length < 2 || STOMPER2.Text.Trim().Length < 2 || STOMPER3.Text.Trim().Length < 2))
            {
                ErrorMessage.Text = "Enter all STOMPER names";
                return;
            }

            objInfoData.ObserverName = ObserverName.Text.Trim();
            objInfoData.SchoolName = SchoolName.Text.Trim();
            objInfoData.TeacherName = TeacherName.Text.Trim();
            objInfoData.Stomper1 = STOMPER1.Text.Trim();
            objInfoData.Stomper2 = STOMPER2.Text.Trim();
            objInfoData.Stomper3 = STOMPER3.Text.Trim();
            objInfoData.ObservationNote = ObservationNote.Text.Trim();
            objInfoData.ObservationDate = ObservationDate.Date.DateTime;
            objInfoData.NoGapTime = NoGapTime.IsChecked.Value;
                        
            if (bStarted == false)
            {
                Start.Content = "Save";                
                GapTime.IsEnabled = false;
                ObservationTime.IsEnabled = false;
                NoGapTime.IsEnabled = false;
                Exit.Visibility = Visibility.Collapsed;
                bStarted = true;
            }
            this.Frame.Navigate(typeof(MainPage), objInfoData);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void StomperNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int nIndex = StomperNo.SelectedIndex;
            if (nIndex == 0)
            {
                STOMPER2.IsEnabled = false;
                STOMPER3.IsEnabled = false;
            }
            else if (nIndex == 1)
            {
                STOMPER2.IsEnabled = true;
                STOMPER3.IsEnabled = false;
            }
            else
            {
                STOMPER2.IsEnabled = true;
                STOMPER3.IsEnabled = true;
            }
        }

        private void NoGapTime_Checked(object sender, RoutedEventArgs e)
        {            
            GapTime.IsEnabled = false;
        }

        private void NoGapTime_Unchecked(object sender, RoutedEventArgs e)
        {         
            GapTime.IsEnabled = true;
        }
    }
}
