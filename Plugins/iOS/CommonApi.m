//
//  CommonApi.m
//  UnityFramework
//
//  Created by 晴天 on 2024/9/23.
//

#import <Foundation/Foundation.h>
#import "CommonApi.h"

@implementation CommonApi

+(NSString *)objectToJson:(id)obj{
    if (obj == nil) {
        return nil;
    }
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:obj
                                                       options:0
                                                         error:&error];
 
    if ([jsonData length] && error == nil){
        return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }else{
        return nil;
    }
}


+(id)jsonToObject:(NSString *)json{
    //string转data
    NSData * jsonData = [json dataUsingEncoding:NSUTF8StringEncoding];
    //json解析
    id obj = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:nil];
    return obj;
}

+(NSDictionary*)jsonToNSDictionary:(NSString *)json{
    if (json == nil){
        return nil;
    }
    
    NSData* jsonData = [json dataUsingEncoding:NSUTF8StringEncoding];
    NSError* error;
    NSDictionary* dic = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&error];
    
    if (error){
        NSLog(@"解析json错误：%@, %@", json, error);
        return nil;
    }
    return dic;
}

//将NSDictinoay转换成NSObject对象
+(id)perseJsonObjectWitchDictionary:(NSDictionary*)dic clz:(Class)clz {
    //初始化对象
    id jObject = [[clz alloc]init];
    for (id key in dic) {
        [jObject setValue:[dic objectForKey:key] forKey:key];
    }
    return jObject;
}

+(id)perseJsonToObject:(Class)clz jsonStr:(NSString*)jsonString {
    NSError *error = nil;

    NSData *data = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
//将json字符串转换成相应的NSDictinoary或者NSArray
    id obj = [NSJSONSerialization JSONObjectWithData:data
                                             options:kNilOptions
                                               error:&error];
    if (error != nil) {
         NSLog(@"%@",error);
        return  nil;
    }
    //如果是NSDictionary对象
    if ([obj isKindOfClass:[NSDictionary class]]) {
//将NSDictinoay转换成NSObject对象
        id jObject = [self perseJsonObjectWitchDictionary:obj clz:clz];
        return jObject;
    }else if ([obj isKindOfClass:[NSArray class]]) {
//如果是NSArray
         NSMutableArray *mArray= [[NSMutableArray alloc]init];
       
        NSArray *jsAr = (NSArray*)obj;
        for (NSDictionary *dic in jsAr) {
            //将NSDictinoay转换成NSObject对象
             id jObject = [self perseJsonObjectWitchDictionary:dic clz:clz];
            [mArray addObject:jObject];
        }
        return mArray;

    }else {
         NSLog(@"NSJSONSerialization error");
        return nil;
    }
}

+(bool)init {
    return YES;
}

+(bool)vibrator:(int)effectType {
    UIImpactFeedbackGenerator* generator;
    if (effectType == VibratorEffectTypeLow) {
        generator = [[UIImpactFeedbackGenerator alloc]initWithStyle:UIImpactFeedbackStyleLight];
    } else if (effectType == VibratorEffectTypeHigh){
        generator = [[UIImpactFeedbackGenerator alloc]initWithStyle:UIImpactFeedbackStyleHeavy];
    } else {
        generator = [[UIImpactFeedbackGenerator alloc]initWithStyle:UIImpactFeedbackStyleMedium];
    }

    [generator impactOccurred];
    return YES;
}

+(NSString *)getCountryInfo{
    NSURL* url = [NSURL URLWithString:@"https://api.ipify.org?format=json"];
    NSError* error = nil;
    NSMutableString* ip = [NSMutableString stringWithContentsOfURL:url
                                                          encoding:NSUTF8StringEncoding
                                                             error:&error];
    NSData* data = [ip dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary* dict = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&error];
    
    url = [NSURL URLWithString:[NSString stringWithFormat:@"https://ipinfo.io/%@/json", [dict objectForKey:@"ip"]]];
    ip = [NSMutableString stringWithContentsOfURL:url encoding:NSUTF8StringEncoding error:&error];
    return ip;
}

@end

#pragma mark extern function

extern "C" void c_platform_tools_init(U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError){
    if ([MobilePlatformTools init]){
        onSuccess("");
    } else {
        onError(-1, "");
    }
}

extern "C" void c_platform_tools_vibrator(int effectType, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError){
    if ([MobilePlatformTools vibrator:effectType]){
        onSuccess("");
    } else {
        onError(-1, "");
    }
}

extern "C" void c_platform_tools_getCountryInfo(U3DBridgeCallback_Success onSuccess){
    NSString* resp = [MobilePlatformTools getCountryInfo];
    onSuccess([resp UTF8String]);
}
