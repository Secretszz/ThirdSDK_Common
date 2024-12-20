package com.bridge.common;

import android.app.Activity;
import android.app.Service;
import android.os.Build;
import android.os.Vibrator;
import android.util.Log;

import com.platform.tools.callback.BridgeCallback;
import com.platform.tools.response.GenericsResponse;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.ResponseBody;

public class CommonApi {
    private final static String TAG = CommonApi.class.getName();
    public final static int CODE_SUCCESS = 0;
    public final static String MESSAGE_SUCCESS = "success";
    public final static int CODE_ERROR = -1;

    /**
     * 单例
     * @return MobilePlatformTools单例
     */
    public static MobilePlatformTools getInstance(){
        return Holder.INSTANCE;
    }

    private static class Holder{
        public static MobilePlatformTools INSTANCE = new MobilePlatformTools();
    }

    private Vibrator vibrator;

    /**
     *
     * @param activity 主activity
     * @param onComplete 初始化回调
     */
    public void init(Activity activity, BridgeCallback onComplete){
        vibrator = (Vibrator)activity.getApplicationContext().getSystemService(Service.VIBRATOR_SERVICE);
        onComplete.onResponse(GenericsResponse.success());
    }

    /**
     * 振动
     * @param effectName 振动类型名称
     * @param onComplete 振动回调
     */
    public void vibratorByEffectType(String effectName, BridgeCallback onComplete){
        VibratorEffectType effectType = VibratorEffectType.valueOf(effectName);
        vibrator.cancel();
        if (Build.VERSION.SDK_INT < Build.VERSION_CODES.Q){
            vibrator.vibrate(VibratorEffectType.getVibrationEffectValue(effectType));
        } else {
            vibrator.vibrate(VibratorEffectType.getVibrationEffect(effectType));
        }
        onComplete.onResponse(GenericsResponse.success());
    }

    /**
     * 获取国家信息
     * @param onComplete 回调
     */
    public void getCountryInfo(BridgeCallback onComplete){
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
        onComplete.onResponse(GenericsResponse.success(result));
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
}
