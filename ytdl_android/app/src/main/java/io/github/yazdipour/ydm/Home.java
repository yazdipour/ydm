package io.github.yazdipour.ydm;

import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.text.InputType;
import android.util.Log;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.snappydb.SnappydbException;
import com.thunderpunch.lib.layoutmanager.LadderLayoutManager;
import com.thunderpunch.lib.layoutmanager.LadderSimpleSnapHelper;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;

import io.github.yazdipour.ydm.Models.HomeCard_Adapter;
import io.github.yazdipour.ydm.Models.Youtube_Item;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Request;
import okhttp3.Response;

import static com.microsoft.appcenter.utils.HandlerUtils.runOnUiThread;

public class Home extends Fragment {
    final String TAG = "Home<<<";

    public static LadderLayoutManager llm;
    public static HomeCard_Adapter adapter;
    public static RecyclerView rcv;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, final ViewGroup container,
                             final Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_home, container, false);
        TextView tv_home_no_video = view.findViewById(R.id.tv_home_no_video);
        llm = new LadderLayoutManager(0.75f, 0.85f, LadderLayoutManager.VERTICAL)
                .setChildDecorateHelper(new LadderLayoutManager.DefaultChildDecorateHelper(container.getResources().getDimension(R.dimen.item_max_elevation)));
        llm.setMaxItemLayoutCount(9);
        llm.setChildPeekSize((int) TypedValue.applyDimension(
                TypedValue.COMPLEX_UNIT_DIP, 75, container.getResources().getDisplayMetrics()));
        rcv = view.findViewById(R.id.rcv);
        rcv.setLayoutManager(llm);
        new LadderSimpleSnapHelper().attachToRecyclerView(rcv);
        adapter = new HomeCard_Adapter();
        rcv.setAdapter(adapter);
        view.findViewById(R.id.fab_add).setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                try {
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.getContext());
                    builder.setTitle("Enter Youtube Url:");
                    final EditText input = new EditText(view.getContext());
                    input.setInputType(InputType.TYPE_CLASS_TEXT);
                    builder.setView(input);
                    builder.setPositiveButton("Get", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            String txt = input.getText().toString().trim();
                            try {
                                if (txt.length() < 4) throw new Exception("Problem with Url");
                                else {
                                    String url = App.apiHandler.getVideoLink(txt, view.getContext());
                                    if (url.charAt(0) == '>') {
                                        if (url.length() > 1)
                                            Toast.makeText(view.getContext(), url.substring(1), Toast.LENGTH_LONG).show();
                                        else
                                            Toast.makeText(view.getContext(), "Error! 0x94", Toast.LENGTH_LONG).show();
                                    } else {
                                        final Request req = new Request.Builder().url(url).build();
                                        App.apiHandler.ok.newCall(req).enqueue(new Callback() {
                                            @Override
                                            public void onResponse(Call call, Response response) throws IOException {
                                                try {
                                                    String respond = response.body().string();
                                                    final Youtube_Item DLI = App.apiHandler.processNewVideo(respond);
                                                    if (DLI == null) return;
                                                    runOnUiThread(new Runnable() {
                                                        @Override
                                                        public void run() {
                                                            try {
                                                                App.mainList.add(DLI);
                                                                App.cacheTheList();
                                                                adapter.notifyDataSetChanged();
                                                                llm.smoothScrollToPosition(rcv, new RecyclerView.State(), adapter.getItemCount());

                                                                Account.noTextView.setText(String.valueOf(App.user.nrCanDownload));
                                                            } catch (Exception e) {
                                                                e.printStackTrace();
                                                            }
                                                        }
                                                    });
                                                } catch (Exception ex) {
                                                    Log.e(TAG, "onResponse: ", ex);
                                                }
                                            }

                                            @Override
                                            public void onFailure(Call call, IOException e) {
                                                Toast.makeText(view.getContext(), "Error!  0x100", Toast.LENGTH_LONG).show();
                                            }
                                        });
                                    }
                                }
                            } catch (Exception e) {
                                Toast.makeText(view.getContext(), e.getMessage(), Toast.LENGTH_LONG).show();
                            }
                        }
                    });
                    builder.setNegativeButton("Close", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {dialog.cancel();}
                    });
                    builder.show();
                } catch (Exception e) {
                    Log.e(TAG, "onClick: ", e);
                }
            }
        });

        loadItemsFromCache(view.getContext());
        if (adapter.getItemCount() < 1)
            tv_home_no_video.setVisibility(View.VISIBLE);
        return view;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
    }

    private void loadItemsFromCache(Context cntx) {
        try {
            App.setCacheHandler(cntx);
            if (App.cacheHandler.isExisting("MainList")) {
                try {
                    String temps = App.cacheHandler.getCache("MainList");
                    Youtube_Item[] temp = App.gson.fromJson(temps, Youtube_Item[].class);
                    if (temp.length > 0) {
                        App.mainList = Arrays.asList(temp);
                        App.mainList = new ArrayList<>(App.mainList);
                        adapter.notifyDataSetChanged();
                    }
                } catch (Exception ex) {
                    Log.e(TAG, "loadItemsFromCache: ", ex);
                }
            }
        } catch (SnappydbException e) {
            Log.e(TAG, "loadItemsFromCache: ", e);
        }
    }
}