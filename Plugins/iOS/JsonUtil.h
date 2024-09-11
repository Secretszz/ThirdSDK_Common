//
//  JsonUtil.h
//  UnityFramework
//
//  Created by 晴天 on 2023/10/10.
//

#import <objc/runtime.h>

@interface JsonUtil : NSObject
+(NSString *)objectToJson:(id)obj;
+(id)jsonToObject:(NSString *)json;
+(NSDictionary*)jsonToNSDictionary:(NSString *)json;
+(id)perseJsonObjectWitchDictionary:(NSDictionary*)dic clz:(Class)clz;
+(id)perseJsonToObject:(Class)clz jsonStr:(NSString*)jsonString;
@end
