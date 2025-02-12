package com.bridge.common;

import android.app.Activity;
import android.app.Service;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Vibrator;
import android.util.Log;

import com.bridge.common.listener.IBridgeListener;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.ResponseBody;

public class CommonApi {
    private final static String TAG = CommonApi.class.getName();

    /**
     * 单例
     * @return MobilePlatformTools单例
     */
    public static CommonApi getInstance(){
        return Holder.INSTANCE;
    }

    private Vibrator vibrator;

    /**
     *
     * @param activity 主activity
     * @param initListener 初始化回调
     */
    public void init(Activity activity, IBridgeListener initListener){
        vibrator = (Vibrator)activity.getApplicationContext().getSystemService(Service.VIBRATOR_SERVICE);
        initListener.onSuccess("");
    }

    /**
     * 振动
     * @param effectName 振动类型名称
     * @param vibratorListener 振动回调
     */
    public void vibratorByEffectType(String effectName, IBridgeListener vibratorListener){
        VibratorEffectType effectType = VibratorEffectType.valueOf(effectName);
        vibrator.cancel();
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.Q){
            vibrator.vibrate(VibratorEffectType.getVibrationEffectValue(effectType));
        } else {
            vibrator.vibrate(VibratorEffectType.getVibrationEffect(effectType));
        }
        vibratorListener.onSuccess("");
    }

    /**
     * 获取国家信息
     * @param listener 回调
     */
    public void getCountryInfo(IBridgeListener listener){
        String result = "";
        String ip = getIp();
        if (!ip.isEmpty()){
            OkHttpClient client = new OkHttpClient();
            Request request = new Request.Builder()
                    .url(String.format("https://ipinfo.io/%s/json", ip))
                    .build();
            try (Response response = client.newCall(request).execute()) {
                ResponseBody responseBody = response.body();
                if (responseBody != null) {
                    result = responseBody.string();
                }
            } catch (IOException e) {
                Log.e(TAG, "getCountryInfo: ", e);
            }
        }
        listener.onSuccess(result);
    }

    private String getIp(){
        String ip = "";
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url("https://api.ipify.org?format=json")
                .build();
        try (Response response = client.newCall(request).execute()) {
            ResponseBody responseBody = response.body();
            if (responseBody != null){
                String responseData = responseBody.string();
                JSONObject jsonObject = new JSONObject(responseData);
                ip = jsonObject.getString("ip");
            }
        } catch (IOException | JSONException e) {
            Log.e(TAG, "getIp: ", e);
        }
        return ip;
    }

    /**
     * 发起添加群流程。
     * @param activity 主Activity
     * @param key 由官网生成的key
     * @param listener 回调
     */
    public void joinQQGroup(Activity activity, String key, IBridgeListener listener) {
        try {
            Intent intent = new Intent();
            intent.setData(Uri.parse("mqqopensdkapi://bizAgent/qm/qr?url=http%3A%2F%2Fqm.qq.com%2Fcgi-bin%2Fqm%2Fqr%3Ffrom%3Dapp%26p%3Dandroid%26jump_from%3Dwebapi%26k%3D" + key));
            // 此Flag可根据具体产品需要自定义，如设置，则在加群界面按返回，返回手Q主界面，不设置，按返回会返回到呼起产品界面
            //intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK)
            activity.startActivity(intent);
            listener.onSuccess("");
        } catch (Exception e) {
            // 未安装手Q或安装的版本不支持
            listener.onError(-1, "");
        }
    }

    private static class Holder{
        public static CommonApi INSTANCE = new CommonApi();
    }
}
