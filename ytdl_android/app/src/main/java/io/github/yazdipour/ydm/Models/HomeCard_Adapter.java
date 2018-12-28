package io.github.yazdipour.ydm.Models;

import android.content.Intent;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.snappydb.SnappydbException;
import com.squareup.picasso.Picasso;

import io.github.yazdipour.ydm.App;
import io.github.yazdipour.ydm.Home;
import io.github.yazdipour.ydm.Item;
import io.github.yazdipour.ydm.R;

public class HomeCard_Adapter extends RecyclerView.Adapter<HomeCard_Adapter.HomeCardViewHolder> {
    LayoutInflater lay;
    private final String TAG="HomeCard_Adapter<<<";
    @Override
    public void onBindViewHolder(HomeCardViewHolder holder, int position) {
        try{
            holder.bindData(App.mainList.get(position));
        }catch(Exception ignore){}
    }

    @Override
    public HomeCardViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        lay = LayoutInflater.from(parent.getContext());
        return new HomeCardViewHolder(lay.inflate(R.layout.item_template_home_card, parent, false));
    }

    @Override
    public int getItemCount() {
        return App.mainList.size();
    }


    class HomeCardViewHolder extends RecyclerView.ViewHolder {
        TextView tv_title, tv_duration, tv_url;
        ImageView bg_img, delete_btn;

        HomeCardViewHolder(View itemView) {
            super(itemView);
            tv_title = itemView.findViewById(R.id.tv_title);
            tv_duration = itemView.findViewById(R.id.tv_duration);
            tv_url = itemView.findViewById(R.id.tv_url);
            bg_img = itemView.findViewById(R.id.imgVideo);
            delete_btn = itemView.findViewById(R.id.delete_btn);
        }

        void bindData(final Youtube_Item item) {
            try{
                tv_title.setText(item.getTitle());
                tv_duration.setText(item.getDuration());
                tv_url.setText("Id: "+item.getId());
                Picasso.with(lay.getContext()).load(item.getImg()).placeholder(R.drawable.playbtn)
                        .error(R.drawable.playbtn).into(bg_img);



                itemView.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        Log.d(TAG, "onClick: itemView");
                        Intent i = new Intent(itemView.getContext(), Item.class);
                        i.putExtra("item",App.gson.toJson(item));
                        view.getContext().startActivity(i);
                    }
                });
            }catch (Exception ignore){}

            delete_btn.setOnClickListener(new View.OnClickListener() {
                public void onClick(View v) {
                    try {
                        App.mainList.remove(item);
                        Home.adapter.notifyDataSetChanged();
                        App.cacheTheList();
                    } catch (SnappydbException e) {
                        e.printStackTrace();
                    }
                }
            });
        }
    }

}