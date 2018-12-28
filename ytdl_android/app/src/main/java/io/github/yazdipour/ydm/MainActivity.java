package io.github.yazdipour.ydm;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.Fragment;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.view.ViewGroup;

import com.gigamole.navigationtabstrip.NavigationTabStrip;
import com.microsoft.appcenter.AppCenter;
import com.microsoft.appcenter.analytics.Analytics;
import com.microsoft.appcenter.crashes.Crashes;

public class MainActivity extends AppCompatActivity {

    private final String TAG = "MainActivity<<<";
    private ViewPager mViewPager;
    private NavigationTabStrip mNavigationTabStrip;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        App.apiHandler.getToday(true);
        setUI();
        AppCenter.start(getApplication(), "a02953b6-6324-46a1-ba84-cee024fa5aaa",
                Analytics.class, Crashes.class);
    }



    private void setUI() {
        final Fragment[] fragments = new Fragment[]{
                new Home(),new Search(), new Account()};
        mViewPager = findViewById(R.id.vp);
        mViewPager.setOffscreenPageLimit(fragments.length);
        mNavigationTabStrip = findViewById(R.id.nts_bottom);
        mViewPager.setAdapter(new PagerAdapter() {
            @Override
            public int getCount() {
                return fragments.length;
            }

            @Override
            public boolean isViewFromObject(@NonNull final View view, @NonNull final Object object) {
                return view.equals(object);
            }

            @Override
            public void destroyItem(@NonNull final View container, final int position, @NonNull final Object object) {
                ((ViewPager) container).removeView((View) object);
            }

            @NonNull
            @Override
            public Object instantiateItem(@NonNull final ViewGroup container, final int position) {
                View view;
                if (position < fragments.length) {
                    Fragment fr = fragments[position];
                    view = fr.onCreateView(getLayoutInflater(), container, new Bundle());
                    container.addView(view);
                } else view = container.getRootView();
                return view;
            }
        });
        mNavigationTabStrip.setViewPager(mViewPager, 0);
        mNavigationTabStrip.setTabIndex(0, true);
    }
}