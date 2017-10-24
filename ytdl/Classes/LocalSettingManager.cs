using System;
using Windows.Storage;
namespace ytdl.Classes {
    static class LocalSettingManager {
        public static string ReadSetting(string address,bool roam=false)
        {
            var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
            try {
                return ls.Values[address].ToString();
            }
            catch(Exception) { return "[ERROR!]"; }
        }
        public static bool ExistsSetting(string address,bool roam = false) { 
            return ReadSetting(address,roam) != "[ERROR!]";
        }
        public static bool SaveSetting(string address,string setting,bool roam = false) {
            var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
            try {
                ls.Values[address] = setting;
                return true;
            }
            catch(Exception) { return false; }
        }
		public static void RemoveSetting(string address,bool roam = false) { 
            var ls = !roam ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
            try {
                ls.Values.Remove(address);
            }
            catch(Exception) { }
        }

    }
}