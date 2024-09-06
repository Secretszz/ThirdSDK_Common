package com.bridge.common.listener;

public interface IPayListener {
    /**
     * 支付成功
     */
    void onSuccess();

    /**
     * 用户取消
     */
    void onCancel();

    /**
     * 支付失败
     * @param errCode 错误码
     * @param errMsg 错误信息
     */
    void onError(int errCode, String errMsg);
}
