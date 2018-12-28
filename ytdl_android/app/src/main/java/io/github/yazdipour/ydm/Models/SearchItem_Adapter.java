package io.github.yazdipour.ydm.Models;

import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.squareup.picasso.Picasso;

import java.io.IOException;
import java.util.List;

import io.github.yazdipour.ydm.Account;
import io.github.yazdipour.ydm.App;
import io.github.yazdipour.ydm.Home;
import io.github.yazdipour.ydm.R;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Request;
import okhttp3.Response;

import static com.microsoft.appcenter.utils.HandlerUtils.runOnUiThread;

public class SearchItem_Adapter extends RecyclerView.Adapter {
    class SearchItem_ViewHolder extends RecyclerView.ViewHolder {
        TextView tv_item_name, tv_item_channel, tv_item_url;
        ImageView imageView;
        final String TAG = "SearchAdapter<<<";

        SearchItem_ViewHolder(View itemView) {
            super(itemView);
            this.tv_item_name = itemView.findViewById(R.id.tv_item_name);
            this.tv_item_url = itemView.findViewById(R.id.tv_item_url);
            this.tv_item_channel = itemView.findViewById(R.id.tv_item_channel);
            this.imageView = itemView.findViewById(R.id.imageView);
        }

        void bindData(final Youtube_Item item) {
            tv_item_name.setText(item.getTitle());
            tv_item_url.setText("Id: " + item.getId());
            tv_item_channel.setText("Channel: " + item.getChannel());
            Picasso.with(itemView.getContext()).load(item.getImg()).placeholder(R.drawable.playbtn)
                    .error(R.drawable.playbtn).into(imageView);

            itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    String url = App.apiHandler.getVideoLink(item.getId(), null);
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
                                    String respond = response.body().string();
                                    final Youtube_Item DLI = App.apiHandler.processNewVideo(respond);
                                    if(DLI==null)return;
                                    runOnUiThread(new Runnable() {
                                        @Override
                                        public void run() {
                                            try {
                                                App.mainList.add(DLI);
                                                App.cacheTheList();
                                                Home.adapter.notifyDataSetChanged();
                                                Home.llm.smoothScrollToPosition(Home.rcv, new RecyclerView.State(), Home.adapter.getItemCount());
                                                Toast.makeText(itemView.getContext(), "اضافه شده", Toast.LENGTH_LONG).show();

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
                                Log.e(TAG, "onFailure:  Error!  0x100", e);
                            }
                        });
                    }

                }
            });
        }
    }

    private List<Youtube_Item> dataList;

    public SearchItem_Adapter(List<Youtube_Item> dataList) {
        this.dataList = dataList;
    }

    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(parent.getContext());
        View view = inflater.inflate(R.layout.item_template_search_card, parent, false);
        return new SearchItem_ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        SearchItem_ViewHolder myHolder = (SearchItem_ViewHolder) holder;
        myHolder.bindData(dataList.get(position));
    }


    @Override
    public int getItemCount() {
        return dataList.size();
    }
}
