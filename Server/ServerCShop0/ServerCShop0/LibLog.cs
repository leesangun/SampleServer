using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

//NuGet log4net
namespace Lib
{
    class LibLog
    {
        public static void Init()
        {
            var repository = LogManager.GetRepository();
            repository.Configured = true;

            // 콘솔 로그 패턴 설정	
            var consoleAppender = new ConsoleAppender();
            consoleAppender.Name = "Console";
            // 로그 패턴	
            consoleAppender.Layout = new PatternLayout("%d [%t] %-5p %c - %m%n");

            // 파일 로그 패턴 설정	
            var rollingAppender = new RollingFileAppender();
            rollingAppender.Name = "RollingFile";
            // 시스템이 기동되면 파일을 추가해서 할 것인가? 새로 작성할 것인가?	
            rollingAppender.AppendToFile = true;
            rollingAppender.DatePattern = "-yyyy-MM-dd HH-mm.'log'"; //분단위 로그
            // 로그 파일 설정	
            //rollingAppender.File = @"d:\work\test.log";
            rollingAppender.File = @"log/log";
            rollingAppender.StaticLogFileName = false;
            // 파일 단위는 날짜 단위인 것인가, 파일 사이즈인가?	
            rollingAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            // 로그 패턴	
            rollingAppender.Layout = new PatternLayout("%d [%t] %-5p %c - %m%n");

            var hierarchy = (Hierarchy)repository;
            hierarchy.Root.AddAppender(consoleAppender);
            hierarchy.Root.AddAppender(rollingAppender);
            rollingAppender.ActivateOptions();
            // 로그 출력 설정 All 이면 모든 설정이 되고 Info 이면 최하 레벨 Info 위가 설정됩니다.	
            //Info로 설정하게 되면 Fatal, Error, Warn, Info만 표시되고 Debug는 표지되지 않음
            hierarchy.Root.Level = log4net.Core.Level.All;

            //ILog logger = LogManager.GetLogger(this.GetType());

            // 로그 레벨 순위 입니다.
            /*
            logger.Fatal("fatal log");
            logger.Error("error log");
            logger.Warn("warn log");
            logger.Info("info log");
            logger.Debug("debug log");
            */
        }

        public static void Test()
        {
            LibLog.Init();
            ILog logger = LogManager.GetLogger(typeof(ServerCShop0.Program));
            logger.Info("로그");
        }
    }
}



