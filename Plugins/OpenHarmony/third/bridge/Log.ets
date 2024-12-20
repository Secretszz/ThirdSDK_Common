import util from '@ohos.util';

/**
 * 日志等级
 */
enum LogLevel {
  /**
   * More detailed information.
   */
  verbose,

  /**
   * Detailed information on the flow through the system.
   */
  debug,

  /**
   * Interesting runtime events (startup/shutdown), should be conservative and keep to a minimum.
   */
  info,

  /**
   * Other runtime situations that are undesirable or unexpected, but not necessarily "wrong".
   */
  warn,

  /**
   * Other runtime errors or unexpected conditions.
   */
  error,

  /**
   * Severe errors that cause premature termination.
   */
  assert,
}

/**
 * 日志接口
 */
interface ILogger{
  /**
   * 输出日志
   * @param level 日志等级
   * @param tag 日志标签
   * @param message 日志内容
   */
  print(level: LogLevel, tag: string, message: string): void
}

/**
 * 默认日志实现类
 */
class DefaultLoggerImpl implements ILogger{
  print(level: LogLevel, tag: string, message: string): void {
    let logFunc: (message: string) => void =
      (() => {
        switch (level) {
          case LogLevel.verbose: return console.log
          case LogLevel.debug:   return console.debug
          case LogLevel.info:    return console.info
          case LogLevel.warn:    return console.warn
          case LogLevel.error:   return console.error
          case LogLevel.assert:   return console.assert
        }
      })()
    logFunc('[' + tag + '] ' + message)
  }
}

/**
 * 供外部调用的日志类
 */
export class Log{
  private static log:ILogger = new DefaultLoggerImpl

  /**
   * 设置自定义实现ILogger接口的日志类
   * @param impl
   */
  public static setLogImpl(impl: ILogger) {
    Log.log = impl
  }

  /**
   * 重设为默认
   */
  public static reset2DefaultLogImpl(){
    Log.log = new DefaultLoggerImpl
  }

  /**
   * 打印日志
   * @param level 日志等级
   * @param tag 日志标签
   * @param format 文本拼接格式
   * @param args
   */
  private static printf(level: LogLevel, tag: string, format: string, ...args: Object[]) {
    Log.log.print(level, tag, util.format(format, ...args))
  }

  /**
   * 输出LogLevel.debug级别的日志
   * @param tag
   * @param format
   * @param args
   */
  public static d(tag: string, format: string, ...args: Object[]) { Log.printf(LogLevel.debug, tag, format, ...args); }
  public static i(tag: string, format: string, ...args: Object[]) { Log.printf(LogLevel.info, tag, format, ...args); }
  public static w(tag: string, format: string, ...args: Object[]) { Log.printf(LogLevel.warn, tag, format, ...args); }
  public static e(tag: string, format: string, ...args: Object[]) { Log.printf(LogLevel.error, tag, format, ...args); }
}