package com.bridge.common.listener;

/**
 * 初始化监听
 */
public interface IInitListener {
    /**
     * 初始化成功
     */
    void onSuccess();

    /**
     * 初始化失败
     * @param errCode 错误码
     * @param errMsg 错误信息
     */
    void onError(int errCode, String errMsg);
}
