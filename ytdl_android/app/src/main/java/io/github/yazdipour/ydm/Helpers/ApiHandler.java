package io.github.yazdipour.ydm.Helpers;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.util.Base64;

import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.List;

import io.github.yazdipour.ydm.Account;
import io.github.yazdipour.ydm.App;
import io.github.yazdipour.ydm.Models.Link_Item;
import io.github.yazdipour.ydm.Models.Youtube_Item;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

import static com.microsoft.appcenter.utils.HandlerUtils.runOnUiThread;

public class ApiHandler {
    public final String BASE_URL = "https://shahriar.in/app/ydm/";
    private final String TAG = "ApiHandler<<<";
    public OkHttpClient ok = new OkHttpClient();

    public boolean isNetworkAvailable(Context _context) {
        ConnectivityManager connectivityManager
                = (ConnectivityManager) _context.getSystemService(Context.CONNECTIVITY_SERVICE);
        assert connectivityManager != null;
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnected();
    }

    public boolean isConnectingToInternet(Context _context) {
        ConnectivityManager connectivity = (ConnectivityManager) _context.getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivity != null) {
            NetworkInfo[] info = connectivity.getAllNetworkInfo();
            if (info != null)
                for (NetworkInfo anInfo : info)
                    if (anInfo.isConnected())
                        return true;
        }
        return false;
    }

    public String getRequest(String url) {
        return HttpRequest.get(url).body();
    }

    public String ReverseString(String input) {
        StringBuilder sb = new StringBuilder(input);
        sb.reverse();
        return sb.toString();
    }

    public String Base64Encode(String input) {
        return HttpRequest.Base64.encodeBytes(input.getBytes());
    }

    public String Base64Decode(String base64) {
        try {
            byte[] data = Base64.decode(base64, Base64.DEFAULT);
            return new String(data, "UTF-8");
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
            return "Err";
        }
    }

    public String MultiBase64Decode(String str, int i) {
        for (int j = 0; j < i; j++) str = Base64Decode(str);
        return str;
    }

    private String MultiBase64Encode(String str, int n) {
        for (int i = 0; i < n; i++) str = Base64Encode(str);
        return str;
    }

    public String secureEmail(String email) {
        return App.apiHandler.Base64Encode(App.apiHandler.ReverseString(
                App.apiHandler.Base64Encode("#" + email)));
    }

    public String deSecureEmail(String email) {
        return Base64Decode(ReverseString(Base64Decode(email).substring(1))).substring(1);
    }

    private Boolean HasCharge() {
        return App.user.nrCanDownload > 0 && App.Today != 0;
    }

    private String getToken() {
        return Base64Encode(ReverseString(Base64Encode(App.user.id + "|" + App.Today)));
    }

    public void getToday(final boolean skipUIUpdate) {
        String id = Integer.toString(App.user.id);
        String url = App.apiHandler.BASE_URL + "dl/getdate.php?i=" + Base64Encode(ReverseString(Base64Encode(id)));
        final Request req = new Request.Builder().url(url).build();
        ok.newCall(req).enqueue(new Callback() {
            @Override
            public void onResponse(Call call, Response response) throws IOException {
                String result = response.body().string().trim();
                result = MultiBase64Decode(result, 6);
                String[] arr = result.split("\\|");
                App.Today = Integer.parseInt(arr[0].replace("|", ""));
                App.user.nrCanDownload = Integer.parseInt(arr[1]);
                    if (App.user.nrCanDownload < 0) App.user.nrCanDownload = 0;
                if (!skipUIUpdate)
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            try {
                                Account.noTextView.setText(String.valueOf(App.user.nrCanDownload));
                            } catch (Exception ignore) {
                            }
                        }
                    });
            }
            @Override
            public void onFailure(Call call, IOException e) {}
        });
    }

    public String getVideoLink(String videoId, Context _context) {
        String tempId = videoId;
        if (videoId.contains("?v="))
            tempId = videoId.substring(videoId.indexOf("?v=") + 3);
        try {
            for (Youtube_Item item : App.mainList)
                if (item.getId().equals(tempId)) return ">" + "این ویدیو در لیست موجود است";
        } catch (Exception ignore) {
        }
        if (_context != null) {
            if (!isConnectingToInternet(_context))
                return ">" + "مشکل در اتصال به سرور";
            if (!HasCharge())
                return ">" + "اکانت خود را شارژ کنید";
        }
        return BASE_URL + "dl/getvideo.php?u=" + getToken() + "&i=" + Base64Encode(videoId);
    }

    public String getSearchUrl(String input, int maxRes, Context _context) {
        if (!isConnectingToInternet(_context))
            return ">" + "مشکل در اتصال به سرور";
        if (!HasCharge())
            return ">" + "اکانت خود را شارژ کنید";
        return BASE_URL + "search/search.php?q=" + ReverseString(Base64Encode(input))
                + "&maxResults=" + Base64Encode(Integer.toString(maxRes));
    }

    public Youtube_Item processNewVideo(String respond) {
        try {
            if (!respond.contains("[*]")) throw new Exception();
            String[] temp = respond.split("[*]");
            temp[0] = temp[0].substring(0, temp[0].length() - 1);
            temp[1] = temp[1].substring(1);
            final Youtube_Item DLI = App.gson.fromJson(temp[0], Youtube_Item.class);
            DLI.setDuration(App.apiHandler.convertDuration(DLI.getDuration()));
            List<Link_Item> LI = App.gson.fromJson(temp[1], new TypeToken<List<Link_Item>>() {
            }.getType());
            App.user.nrCanDownload--;
            App.cacheHandler.setCache("LI" + DLI.getId(), App.gson.toJson(LI));
            return DLI;
        } catch (Exception e) {
            return null;
        }
    }

    public String getDownloadLink(String id, String tag) {
        return BASE_URL + "dl/getvideo.php?u=" + getToken() + "&i=" + Base64Encode(id) + "&format=" + tag;
    }

    private String convertDuration(String dr) {
        try {
            int idr = Integer.valueOf(dr);
            return idr / 60 + ":" + idr % 60;
        } catch (Exception exp) {
            return "Error!";
        }
    }
}


//    public List<Youtube_Item> getPlayList(String input) {
//        if (str.Contains("mainList="))
//            str = str.SubString(str.IndexOf("mainList=") + 5);
//        str = CloseHelp.Reverse(CloseHelp.Base64Encode(str));
//        String url = "https://shahriar.in/app/ydm/search/playList.php?q=" + str;
//        String respond = await CloseHelp.DownloadPages(new CancellationToken(false), url);
//        if (respond.SubString(0, 3) == "Err")
//        {
//            return null;
//        }
//        return JsonConvert.DeserializeObject<List<Youtube_Item>>(respond);
//        return null;
//    }