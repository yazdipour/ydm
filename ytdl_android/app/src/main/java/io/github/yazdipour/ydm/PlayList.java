package io.github.yazdipour.ydm;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.stone.vega.library.VegaLayoutManager;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import io.github.yazdipour.ydm.Models.SearchItem_Adapter;
import io.github.yazdipour.ydm.Models.Youtube_Item;

public class PlayList extends Fragment {
    public PlayList() {
        dataList = new ArrayList<>();
    }
    List<Youtube_Item> dataList;
    RecyclerView.Adapter adapter;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_search, container, false);
        RecyclerView recyclerView = view.findViewById(R.id.main_recycler_view);
        recyclerView.setLayoutManager(new VegaLayoutManager());
        dataList = new ArrayList<>(Arrays.asList(new Youtube_Item[2]));
        adapter = new SearchItem_Adapter(dataList);
        recyclerView.setAdapter(adapter);
        return view;
    }

}
