package io.github.yazdipour.ydm;

import android.Manifest;
import android.app.DownloadManager;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.jaredrummler.materialspinner.MaterialSpinner;
import com.snappydb.SnappydbException;
import com.squareup.picasso.Picasso;

import java.util.LinkedList;

import io.github.yazdipour.ydm.Models.Link_Item;
import io.github.yazdipour.ydm.Models.Youtube_Item;

public class Item extends AppCompatActivity {

    private final String TAG = "Item<<<";
    int indexItem = 0;
    Youtube_Item item;
    LinkedList<String> spinnerArray = new LinkedList<>();
    private String link = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item);
        String json = getIntent().getStringExtra("item");
        item = App.gson.fromJson(json, Youtube_Item.class);
        try {
            json = App.cacheHandler.getCache("LI" + item.getId());
        } catch (SnappydbException e) {
            finish();
        }
        final Link_Item[] links = App.gson.fromJson(json, Link_Item[].class);
        //Set UI
        TextView tv_title = findViewById(R.id.tv_title);
        TextView tv_duration = findViewById(R.id.tv_duration);
        TextView tv_url = findViewById(R.id.tv_url);
        final TextView tv_size = findViewById(R.id.tv_size);
        tv_title.setText(item.getTitle());
        tv_duration.setText(item.getDuration());
        tv_url.setText("Id: " + item.getId());
        Picasso.with(this).load(item.getImg()).placeholder(R.drawable.playbtn)
                .error(R.drawable.playbtn).into((ImageView) findViewById(R.id.imgBg));

        //Spinner
        final MaterialSpinner sp = findViewById(R.id.spinner);
        for (Link_Item l : links) {
            String str = l.getStr();
            spinnerArray.add(str);
            if (str.contains("audio")) break;
        }
        sp.setItems(spinnerArray);
        sp.setOnItemSelectedListener(new MaterialSpinner.OnItemSelectedListener<String>() {

            @Override
            public void onItemSelected(MaterialSpinner view, int i, long id, String itemS) {
                indexItem = i;
                link = App.apiHandler.getDownloadLink(item.getId(), links[indexItem].getTag().toString());
//                final String key = item.getId() + "_" + i;
//                try {
//                    if (App.cacheHandler.isExisting(key)) {
//                        final String size = App.cacheHandler.getCache(key);
//                        tv_size.setText(size);
//                    } else {
//                        try {
//                            Handler mainHandler = new Handler(getMainLooper());
//                            URL url = new URL(link);
//                            final URLConnection urlConnection = url.openConnection();
//
//                            Runnable myRunnable = new Runnable() {
//                                @Override
//                                public void run() {
//                                    try {
//                                        urlConnection.connect();
//                                        Log.d(TAG, "run: " + 1);
//                                        int lsize = urlConnection.getContentLength();
//                                        DecimalFormat df = new DecimalFormat();
//                                        df.setMaximumFractionDigits(2);
//                                        Log.d(TAG, "run: " + 1);
//                                        final String size = df.format(lsize / (1024 * 1024)) + " MB";
//                                        Log.d(TAG, "run: " + 1);
//                                        App.cacheHandler.setCache(key, size);
//                                        Log.d(TAG, "run: " + 1);
//                                        tv_size.setText(size);
//                                    } catch (Exception ex) {
//                                        Log.e(TAG, "run: ", ex);
//                                    }
//                                }
//                            };
//                        } catch (Exception e) {
//
//                        }
//                    }
//                } catch (SnappydbException e) {
//                    Log.e(TAG, "getVideoSize: ", e);
//                }
            }
        });
        //close
        findViewById(R.id.btn_close).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                finish();
            }
        });

        // share
        findViewById(R.id.btn_share).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                try {
                    Intent i = new Intent(Intent.ACTION_SEND);
                    i.setType("text/plain");
                    i.putExtra(Intent.EXTRA_TEXT, App.apiHandler.getDownloadLink(
                            item.getId(), links[indexItem].getTag().toString())
                    );
                    startActivity(Intent.createChooser(i, "Choose one:"));
                } catch (Exception e) {
                    Toast.makeText(Item.this, "Sharing Error! 0x80", Toast.LENGTH_SHORT).show();
                }
            }
        });
        // stream
        findViewById(R.id.btn_stream).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(Item.this, VideoPlayer.class);
                i.putExtra("link", link);
                startActivity(i);
            }
        });
        // youtube
        findViewById(R.id.btn_yt).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW,
                        Uri.parse("https://www.youtube.com/watch?v=" + item.getId()));
                startActivity(browserIntent);
            }
        });
        // dl
        findViewById(R.id.btn_dl).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (ContextCompat.checkSelfPermission(getBaseContext(), Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                    if (ActivityCompat.shouldShowRequestPermissionRationale(Item.this, Manifest.permission.WRITE_EXTERNAL_STORAGE))
                        Toast.makeText(Item.this, "Canceled", Toast.LENGTH_SHORT).show();
                    else
                        ActivityCompat.requestPermissions(Item.this, new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE}, 1);
                } else {
                    Toast.makeText(Item.this, "Downloading...", Toast.LENGTH_LONG).show();
                    sendToDownloadManager(link);
                }
            }
        });
    }

    private void sendToDownloadManager(String link) {
        Uri uri = Uri.parse(link);
        DownloadManager downloadManager = (DownloadManager) getSystemService(DOWNLOAD_SERVICE);
        DownloadManager.Request request = new DownloadManager.Request(uri);
        String[] types = {"audio", "mp4", "webm", "3gp"};
        String fileName = item.getTitle();
        fileName = fileName.replaceAll("[^a-zA-Z0-9\\.\\-]", "_");
        for (String type : types)
            if (spinnerArray.get(indexItem).contains(type)) {
                if (type.equals("audio")) type = "mp3";
                fileName = fileName + "." + type;
                break;
            }
        request.setTitle(fileName);
//        request.allowScanningByMediaScanner();
        request.setNotificationVisibility(DownloadManager.Request.VISIBILITY_VISIBLE_NOTIFY_COMPLETED);
        request.setDestinationInExternalPublicDir(Environment.DIRECTORY_DOWNLOADS, fileName);
        downloadManager.enqueue(request);
    }
}
