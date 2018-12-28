package io.github.yazdipour.ydm.Models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import io.github.yazdipour.ydm.App;

/**
 * Created by Master on 1/6/2018.
 */

public class Youtube_Item {
    @SerializedName("Id")
    @Expose
    private String id;
    @SerializedName("Title")
    @Expose
    private String title;
    @SerializedName("Duration")
    @Expose
    private String duration;
    @SerializedName("Views")
    @Expose
    private String views;

    @SerializedName("Channel")
    @Expose
    private String Channel;

    public String getChannel() {
        return Channel;
    }

    public void setChannel(String Channel) {
        this.Channel = Channel;
    }

    public String getImg() {
        return App.apiHandler.BASE_URL+"dl/getimage.php?i=" + id;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDuration() {
        return duration;
    }

    public void setDuration(String duration) {
        this.duration = duration;
    }

    public String getViews() {
        return views;
    }

    public void setViews(String views) {
        this.views = views;
    }
}