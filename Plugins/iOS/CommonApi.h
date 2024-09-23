//
//  CommonApi.h
//  UnityFramework
//
//  Created by 晴天 on 2023/10/19.
//

/**
 绑定第三方账号成功事件
 */
typedef void(* U3DBridgeCallback_Success)(const char *);
typedef void(* U3DBridgeCallback_Cancel)();
typedef void(* U3DBridgeCallback_Error)(int, const char *);

@interface CommonApi : NSObject
+(NSString *)objectToJson:(id)obj;
+(id)jsonToObject:(NSString *)json;
+(NSDictionary*)jsonToNSDictionary:(NSString *)json;
+(id)perseJsonObjectWitchDictionary:(NSDictionary*)dic clz:(Class)clz;
+(id)perseJsonToObject:(Class)clz jsonStr:(NSString*)jsonString;
@end
