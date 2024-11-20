using Castle.DynamicProxy;

class Program
{
    static void Main(string[] args)
    {
        // Proxy oluşturucu
        var proxyGenerator = new ProxyGenerator();

        // Log Interceptor
        var logInterceptor = new LogInterceptor();

        // Proxy ile servis oluştur
        IMyService myService = proxyGenerator.CreateInterfaceProxyWithTarget<IMyService>(new MyService(), logInterceptor);

        // Metotları çağır
        myService.DoWork(4,3);        // Loglanır
        myService.DoOtherWork();   // Loglanmaz
    }
}
