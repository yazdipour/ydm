package io.github.yazdipour.ydm;

import android.content.DialogInterface;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.text.InputType;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.Toast;

import com.google.gson.reflect.TypeToken;
import com.stone.vega.library.VegaLayoutManager;
import com.wang.avi.AVLoadingIndicatorView;

import java.io.IOException;
import java.util.List;

import io.github.yazdipour.ydm.Models.SearchItem_Adapter;
import io.github.yazdipour.ydm.Models.Youtube_Item;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Request;
import okhttp3.Response;

import static com.microsoft.appcenter.utils.HandlerUtils.runOnUiThread;


public class Search extends Fragment {
    private VegaLayoutManager vlm;

    public Search() {
    }

    final String TAG = "SearchFragment<<<";
    RecyclerView.Adapter adapter;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_search, container, false);
        final RecyclerView recyclerView = view.findViewById(R.id.main_recycler_view);
        final AVLoadingIndicatorView avl= view.findViewById(R.id.avl);avl.hide();
        vlm=new VegaLayoutManager();
        recyclerView.setLayoutManager(vlm);
        view.findViewById(R.id.fab_search).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    AlertDialog.Builder builder = new AlertDialog.Builder(view.getContext());
                    builder.setTitle("Search for video:");
                    final EditText input = new EditText(view.getContext());
                    input.setInputType(InputType.TYPE_CLASS_TEXT);
                    builder.setView(input);
                    builder.setPositiveButton("Search", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            String txt = input.getText().toString().trim();
                            try {
                                if (txt.length() < 2) throw new Exception("دوباره تلاش کنید");
                                else {
                                    String url = App.apiHandler.getSearchUrl(txt, 20, view.getContext());
                                    if (url.charAt(0) == '>') {
                                        if (url.length() > 1)
                                            Toast.makeText(view.getContext(), url.substring(1), Toast.LENGTH_LONG).show();
                                        else
                                            Toast.makeText(view.getContext(), "Error! 0x70", Toast.LENGTH_LONG).show();
                                    } else {
                                        final Request req = new Request.Builder().url(url).build();
                                        App.apiHandler.ok.newCall(req).enqueue(new Callback() {
                                            @Override
                                            public void onResponse(Call call, Response response) throws IOException {
                                                try {
                                                    avl.show();
                                                    String respond = response.body().string();
                                                    if (respond.length() < 3) return;
                                                    final List<Youtube_Item> DLI = App.gson.fromJson(respond, new TypeToken<List<Youtube_Item>>() {
                                                    }.getType());
                                                    runOnUiThread(new Runnable() {
                                                        @Override
                                                        public void run() {
                                                            adapter = new SearchItem_Adapter(DLI);
                                                            vlm.smoothScrollToPosition(recyclerView,new RecyclerView.State(),1);
                                                            recyclerView.setAdapter(adapter);
                                                            avl.hide();
                                                        }
                                                    });
                                                } catch (Exception ex) {
                                                    Log.e(TAG, "onResponse: ", ex);
                                                    avl.hide();
                                                }
                                            }
                                            @Override
                                            public void onFailure(Call call, IOException e) {
//                                                Toast.makeText(view.getContext(), "Error!  0x80", Toast.LENGTH_LONG).show();
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
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.cancel();
                        }
                    });
                    builder.show();
                } catch (Exception e) {
                    Log.e(TAG, "onClick: ", e);
                }
            }
        });

        return view;
    }
}