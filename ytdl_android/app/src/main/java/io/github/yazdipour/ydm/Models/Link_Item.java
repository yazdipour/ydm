package io.github.yazdipour.ydm.Models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Link_Item {
    public String getStr() {
        return getType() + " | " + getQuality();
    }
    @SerializedName("url")
    @Expose
    private String url;
    @SerializedName("type")
    @Expose
    private String type;
    @SerializedName("quality")
    @Expose
    private String quality;
    @SerializedName("tag")
    @Expose
    private Integer tag;

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getQuality() {
        return quality;
    }

    public void setQuality(String quality) {
        this.quality = quality;
    }

    public Integer getTag() {
        return tag;
    }

    public void setTag(Integer tag) {
        this.tag = tag;
    }
}
