package io.github.yazdipour.ydm;
import android.content.Context;

import com.google.gson.Gson;
import com.snappydb.SnappydbException;

import java.util.ArrayList;
import java.util.List;

import io.github.yazdipour.ydm.Helpers.ApiHandler;
import io.github.yazdipour.ydm.Helpers.CacheHandler;
import io.github.yazdipour.ydm.Models.User_Item;
import io.github.yazdipour.ydm.Models.Youtube_Item;

public class App {
    public static User_Item user;
    public static CacheHandler cacheHandler;
    public static int Today;
    public static Gson gson= new Gson();
    public static List<Youtube_Item> mainList=new ArrayList<>();
    public static ApiHandler apiHandler = new ApiHandler();
    static void setCacheHandler(Context cnt){
        cacheHandler=new CacheHandler(cnt);
    }

    public static void cacheTheList() throws SnappydbException {
        cacheHandler.setCache("MainList",gson.toJson(mainList));
    }
}
