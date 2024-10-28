package com.bridge.common;

import android.os.Build;
import android.os.VibrationEffect;

import androidx.annotation.RequiresApi;


public enum VibratorEffectType {
    LOW(1),
    MIDDLE(2),
    HIGH(3);

    public final static int LOW_VALUE = 1;
    public final static int MIDDLE_VALUE = 2;
    public final static int HIGH_VALUE = 4;

    VibratorEffectType(int value){
        this.value = value;
    }

    private final int value;

    public int getValue(){
        return value;
    }

    public static int getVibrationEffectValue(VibratorEffectType effectType){
        switch (effectType){
            case LOW:
                return LOW_VALUE;
            case MIDDLE:
                return MIDDLE_VALUE;
            case HIGH:
                return HIGH_VALUE;
        }
        return MIDDLE_VALUE;
    }

    @RequiresApi(api = Build.VERSION_CODES.Q)
    public static VibrationEffect getVibrationEffect(VibratorEffectType effectType){
        switch (effectType){
            case LOW:
                return VibrationEffect.createPredefined(VibrationEffect.EFFECT_TICK);
            case MIDDLE:
                return VibrationEffect.createPredefined(VibrationEffect.EFFECT_CLICK);
            case HIGH:
                return VibrationEffect.createPredefined(VibrationEffect.EFFECT_HEAVY_CLICK);
        }
        return VibrationEffect.createPredefined(VibrationEffect.EFFECT_CLICK);
    }
}
