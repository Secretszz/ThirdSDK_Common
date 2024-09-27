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

@end
