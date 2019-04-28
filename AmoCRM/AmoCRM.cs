using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AmoCRM
{
    public class AmoCRMTasks : IDisposable
    {
        private string user_login = "alexnejchev@mail.ru";
        private string user_hash = "f8fb4d75318b7ffb10b70ad50196a26c923f0dd4";
        private CookieContainer cookieContainer = new CookieContainer();
        private Uri baseAddress = new Uri("https://alexnejchev.amocrm.ru");
        private Timer timer;

        public AmoCRMTasks()
        {
            Task.Run(async () => cookieContainer = await GetCookie(new CancellationToken())).GetAwaiter().GetResult();
            SetTimer();
        }

        public AmoCRMTasks(string user_login, string user_hash, Uri baseAddress)
        {
            this.user_login = user_login;
            this.user_hash = user_hash;
            this.baseAddress = baseAddress;
            Task.Run(async () => cookieContainer = await GetCookie(new CancellationToken())).GetAwaiter().GetResult();
            SetTimer();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Task.Run(async () => cookieContainer = await GetCookie(new CancellationToken())).GetAwaiter().GetResult();
        }

        private void SetTimer()
        {
            timer = new System.Timers.Timer(840000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }       

        public async Task AddTaskAsync(CancellationToken ct, Item[] items)
        {
            string url = "/api/v2/tasks";
            
            RootObjectAddTask _param = new RootObjectAddTask();

            foreach (var item in items)
            {
                var item_add = new Add()
                {
                    element_id = item.element_id,
                    element_type = item.element_type,
                    complete_till_at = item.complete_till_at,
                    task_type = item.task_type,
                    text = item.text,
                    created_at = item.created_at,
                    updated_at = item.updated_at,
                    responsible_user_id = item.responsible_user_id,
                    created_by = item.created_by
                };
                _param.add.Add(item_add);
            }          

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var jsonObject = JObject.FromObject(_param);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content, ct);

                result.EnsureSuccessStatusCode();
            }
        }

        public async Task<JObject> GetAllTasks(CancellationToken ct)
        {
            JObject json = null;
            string url = "/api/v2/tasks";

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var result = await client.GetAsync(url);
                var bytes = await result.Content.ReadAsByteArrayAsync();
                Encoding encoding = Encoding.GetEncoding("utf-8");
                string data = encoding.GetString(bytes, 0, bytes.Length);
                json = JObject.Parse(data);
                result.EnsureSuccessStatusCode();
            }
            return json;
        }

        private async Task<CookieContainer> GetCookie(CancellationToken ct)
        {
            var _cookieContainer = new CookieContainer();
            string url = "/private/api/auth.php";

            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("type", "json"),
                    new KeyValuePair<string, string>("USER_LOGIN", user_login),
                    new KeyValuePair<string, string>("USER_HASH",user_hash)
                });

                var result = await client.PostAsync(url, content, ct);
                var cookies = handler.CookieContainer.GetCookies(baseAddress);
                result.EnsureSuccessStatusCode();
                foreach (Cookie cookie in cookies)
                {
                    _cookieContainer.Add(cookie);
                }
                return _cookieContainer;
            }
        }        

        #region IDisposable Support

        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    timer.Enabled = false;
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~AmoCRM()
        // {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
