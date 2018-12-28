package io.github.yazdipour.ydm;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.SignInButton;
import com.google.android.gms.common.api.ApiException;
import com.google.android.gms.tasks.Task;
import com.wang.avi.AVLoadingIndicatorView;

import java.io.IOException;

import io.github.yazdipour.ydm.Models.User_Item;
import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.Request;
import okhttp3.Response;

public class LoginActivity extends AppCompatActivity {
    final String TAG = "Login<<<";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        final SharedPreferences sharedPref = getPreferences(Context.MODE_PRIVATE);
        String encryptMail = sharedPref.getString(getString(R.string.user_info), null);
        if (encryptMail != null) {
            if (encryptMail.length() > 2) {
                try {
                    Login(encryptMail, null, null);
                    return;
                } catch (Exception e) {
                    Log.e(TAG, "onCreate: ", e);
                }
            }
        }
        ((AVLoadingIndicatorView) findViewById(R.id.avl)).hide();
        SignInButton btn = findViewById(R.id.btn_login);
        btn.setSize(SignInButton.SIZE_WIDE);
        btn.setVisibility(View.VISIBLE);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                boolean connected = App.apiHandler.isNetworkAvailable(getApplicationContext());
                if (!connected) {
                    Toast.makeText(LoginActivity.this, "No internet connection", Toast.LENGTH_SHORT).show();
                } else {
                    GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                            .requestEmail().build();
                    GoogleSignInClient mGoogleSignInClient = GoogleSignIn.getClient(getBaseContext(), gso);
                    GoogleSignInAccount account = GoogleSignIn.getLastSignedInAccount(getBaseContext());
                    if (account == null) {
                        Intent signInIntent = mGoogleSignInClient.getSignInIntent();
                        startActivityForResult(signInIntent, 99);
                    } else {
                        String photo="",name="";
                        if(account.getPhotoUrl()!=null)photo=account.getPhotoUrl().toString();
                        if(account.getDisplayName()!=null)name=account.getDisplayName();
                        Login(App.apiHandler.secureEmail(account.getEmail()),name , photo);
                    }
                }
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 99) {
            Task<GoogleSignInAccount> completedTask = GoogleSignIn.getSignedInAccountFromIntent(data);
            try {
                GoogleSignInAccount account = completedTask.getResult(ApiException.class);
                String photo="",name="";
                if(account.getPhotoUrl()!=null)photo=account.getPhotoUrl().toString();
                if(account.getDisplayName()!=null)name=account.getDisplayName();
                Login(App.apiHandler.secureEmail(account.getEmail()),name , photo);
            } catch (ApiException e) {
                Log.e(TAG, "signInResult:failed code=" + e);
            }
        }
    }

    private void Login(String secureKey, String name, String photo) {
        boolean connected = App.apiHandler.isNetworkAvailable(getApplicationContext());
        if (!connected) {
            Toast.makeText(LoginActivity.this, "No internet connection", Toast.LENGTH_SHORT).show();
        } else {
            if (secureKey.length() < 2) {
                logOut("Error in Login 0x90");
                return;
            }
            try {
                getUserInfo(secureKey, getBaseContext(), name, photo);
            } catch (Exception e) {
                Log.e(TAG, "Login: ", e);
            }
        }
    }

    private void saveEEMail(String toSave) {
        final SharedPreferences sharedPref = getPreferences(Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = sharedPref.edit();
        editor.putString(getString(R.string.user_info), toSave);
        editor.apply();
    }

    private void logOut(String msg) {
        Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT).show();
        PreferenceManager.getDefaultSharedPreferences(this).edit().clear().apply();
    }

    private void getUserInfo(final String mail, final Context cntx, String name, String photo) {
        String url = App.apiHandler.BASE_URL + "auth/getuser.php?i=" + mail;
        if (name != null) {
            name = App.apiHandler.Base64Encode(App.apiHandler.ReverseString(App.apiHandler.Base64Encode(name)));
            photo = App.apiHandler.Base64Encode(App.apiHandler.ReverseString(App.apiHandler.Base64Encode(photo)));
            url = App.apiHandler.BASE_URL + "auth/getAndroidUser.php?i=" + mail + "&n=" + name + "&p=" + photo;
        }
        final Request req = new Request.Builder().url(url).build();
        final String finalPhoto = photo;
        App.apiHandler.ok.newCall(req).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                logOut("Error in Login 0x130");
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                String result = response.body().string();
                if (result.length() < 2) throw new IOException();
                App.user = App.gson.fromJson(result, User_Item.class);
                if (App.user.picture.length() < 2) App.user.picture = finalPhoto;
                saveEEMail(mail);
                Intent i = new Intent(cntx, MainActivity.class);
                startActivity(i);
                finish();
            }
        });
    }
}