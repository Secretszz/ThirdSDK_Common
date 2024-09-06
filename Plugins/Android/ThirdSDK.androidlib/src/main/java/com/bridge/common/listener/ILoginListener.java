package com.bridge.common.listener;

public interface ILoginListener {
    /**
     * 分享成功
     */
    void onSuccess(String accessToken);

    /**
     * 用户取消登录
     */
    void onCancel();

    /**
     * 分享失败
     *
     * @param errCode 错误码
     * @param errMsg  错误信息
     */
    void onError(int errCode, String errMsg);
}
