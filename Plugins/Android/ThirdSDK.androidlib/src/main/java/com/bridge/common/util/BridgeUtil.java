package com.bridge.common.util;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.util.Log;

import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Iterator;

public class BridgeUtil {

    private final static String TAG = BridgeUtil.class.getName();

    /**
     * 将json字符串转成Bundle
     * @param json 记录事件的参数
     * @return 参数Bundle
     */
    public static Bundle json2Bundle(String json){
        try {
            JSONObject jsonObject = new JSONObject(json);
            Iterator<String> keys = jsonObject.keys();
            Bundle parameters = new Bundle();
            while (keys.hasNext()){
                String key = keys.next();
                String value = (String) jsonObject.get(key);
                if (!value.isEmpty()){
                    parameters.putString(key, value);
                }
            }
            return parameters;
        } catch (Exception ex) {
            Log.e(TAG, "json2Bundle: ", ex);
            return null;
        }
    }

    /**
     * 图片文件转bitmap
     * @param imagePath 图片路径
     * @return bitmap
     * @throws IOException 文件读写错误
     */
    public static Bitmap getBitmap(String imagePath) throws IOException {
        if (imagePath == null || imagePath.isEmpty()){
            Log.e(TAG, "getBitmap: imagePath === path is empty === " + imagePath);
            return null;
        }

        File imageFile = new File(imagePath);
        if (!imageFile.exists()){
            Log.e(TAG, "getBitmap: imagePath === file is empty === " + imagePath);
            return null;
        }

        FileInputStream inputStream = new FileInputStream(imageFile);
        Bitmap bitmap = BitmapFactory.decodeStream(inputStream);
        inputStream.close();
        return bitmap;
    }

    /**
     * 字节转图片
     * @param bytes 图片字节数据
     * @return 图片
     */
    public static Bitmap byteArrayToBitmap(byte[] bytes){
        if (bytes == null || bytes.length == 0){
            Log.e(TAG, "getBitmap: === bytes is empty === ");
            return null;
        }
        return BitmapFactory.decodeByteArray(bytes, 0, bytes.length);
    }
}
