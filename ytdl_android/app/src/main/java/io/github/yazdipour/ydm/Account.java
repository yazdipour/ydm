package io.github.yazdipour.ydm;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import de.hdodenhof.circleimageview.CircleImageView;

public class Account extends Fragment {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        setRetainInstance(true);
        super.onCreate(savedInstanceState);
    }
    public static TextView noTextView;
    @Override
    public View onCreateView(LayoutInflater inflater, final ViewGroup container, Bundle savedInstanceState) {
        final View view = inflater.inflate(R.layout.fragment_account, container, false);
        noTextView= view.findViewById(R.id.tv_user_no);
        try {
            CircleImageView profileImage = view.findViewById(R.id.profile_image);
            if (App.user.picture.length() > 0)
                Picasso.with(view.getContext()).load(App.user.picture).placeholder(R.drawable.avatar).error(R.drawable.avatar).into(profileImage);
            ((TextView) view.findViewById(R.id.tv_user_mail)).setText(App.user.email);
            noTextView.setText(String.valueOf(App.user.nrCanDownload));
        } catch (Exception ignored) {}
//        btn_refresh
        view.findViewById(R.id.btn_refresh).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                App.apiHandler.getToday(false);
            }
        });
        // Charge
        view.findViewById(R.id.btn_charge).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(App.apiHandler.BASE_URL + "pay/?e=" + App.user.email));
                container.getContext().startActivity(browserIntent);
            }
        });
        // EMail
        view.findViewById(R.id.btn_err).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(Intent.ACTION_SENDTO,Uri.parse("mailto:shahriar.yazdipour@outlook.com"));
                intent.putExtra(Intent.EXTRA_SUBJECT, "Problem in YDM.Android");
                intent.putExtra(Intent.EXTRA_TEXT, "Please write what is the problem!");
                container.getContext().startActivity(intent);
            }
        });
        view.findViewById(R.id.btn_uwp).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://goo.gl/QM2KC5"));
                container.getContext().startActivity(browserIntent);
            }
        });
        view.findViewById(R.id.btn_apps).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent browserIntent = new Intent(Intent.ACTION_VIEW, Uri.parse(App.apiHandler.BASE_URL + "../"));
                container.getContext().startActivity(browserIntent);
            }
        });
        return view;
    }
}
