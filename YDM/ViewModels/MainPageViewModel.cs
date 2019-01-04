using YDM.Share;
using System.ComponentModel;
using System.Collections.ObjectModel;
using YDM.Share.Models;

namespace YDM.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public string VersionLabel { get; } = $"YDM\n{Xamarin.Essentials.AppInfo.VersionString}";
        public ApiHandler apiHandler = new ApiHandler();
        public ObservableCollection<VideoItem> DownloadHistory => apiHandler.DownloadHistory;

        public MainPageViewModel() => InitViewModel();
        async void InitViewModel()
        {
            await apiHandler.InitApi();
            if (apiHandler.DownloadHistory?.Count == 0)
                await apiHandler.LoadVideoHistory();
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => ((INotifyPropertyChanged)DownloadHistory).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)DownloadHistory).PropertyChanged -= value;
        }
    }
}