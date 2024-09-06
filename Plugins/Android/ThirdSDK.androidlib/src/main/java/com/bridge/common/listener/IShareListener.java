package com.bridge.common.listener;

/**
 * 分享监听
 */
public interface IShareListener {
    /**
     * 分享成功
     */
    void onSuccess();

    /**
     * 用户取消分享
     */
    void onCancel();

    /**
     * 分享失败
     * @param errCode 错误码
     * @param errMsg 错误信息
     */
    void onError(int errCode, String errMsg);
}
