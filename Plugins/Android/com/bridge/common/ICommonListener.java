package com.bridge.common;

/**
 * 通用监听回调
 */
public interface ICommonListener {
    /**
     * 监听回调 - 错误
     * @param errCode 错误码
     * @param errMsg 错误信息
     */
    void onResult(int errCode, String errMsg);
}
