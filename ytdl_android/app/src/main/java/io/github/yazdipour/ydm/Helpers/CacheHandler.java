package io.github.yazdipour.ydm.Helpers;

import android.content.Context;
import android.content.SharedPreferences;

import com.snappydb.DB;
import com.snappydb.DBFactory;
import com.snappydb.SnappydbException;

import java.util.ArrayList;

import io.github.yazdipour.ydm.Models.Link_Item;
import io.github.yazdipour.ydm.Models.Youtube_Item;

public class CacheHandler {
    private DB db;
    public CacheHandler(Context cnt) {
        try {
            db = DBFactory.open(cnt,"ydm");
        } catch (SnappydbException e) {
            e.printStackTrace();
        }
    }

    public String getCache(String key) throws SnappydbException {
        return db.get(key);
    }

    public void setCache(String key, String txt) throws SnappydbException {
        db.put(key, txt);

    }
    public boolean isExisting(String key) throws SnappydbException {
        return db.exists(key);
    }

    public void deleteCache(String key) throws SnappydbException {
        db.del(key);
    }

    private void closeDB(DB db) throws SnappydbException {
        db.close();
    }

    public void destroyDB(boolean sure) throws SnappydbException {
        if (sure) db.destroy();
    }
}